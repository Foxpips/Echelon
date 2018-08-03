using System.Threading.Tasks;
using System.Web.Mvc;
using Echelon.Mediators;
using Echelon.Models.ViewModels;

namespace Echelon.Controllers
{
    [RequireHttps]
    public class ResetPasswordController : BaseController
    {
        private readonly ResetPasswordMediator _resetPasswordMediator;

        public ResetPasswordController(ResetPasswordMediator registerMediator)
        {
            _resetPasswordMediator = registerMediator;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(ForgottenPasswordModel forgottenPasswordModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", @"Please enter a valid email!");
                return View();
            }

            await _resetPasswordMediator.ResetPassword(forgottenPasswordModel.Email, Url.Action("ResetPassword", "ResetPassword", null, Request.Url?.Scheme));

            return View();
        }

        public async Task<ActionResult> ResetPassword(string id)
        {
            var resetPasswordViewModel = await _resetPasswordMediator.GetUserToReset(id);
            if (resetPasswordViewModel != null)
            {
                return View(resetPasswordViewModel);
            }
            return RedirectToAction("Index","Error");
        }

        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel resetPasswordModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", @"Please enter a valid password!");
                return View();
            }

            await _resetPasswordMediator.UpdateUser(resetPasswordModel);
            return View();
        }
    }
}