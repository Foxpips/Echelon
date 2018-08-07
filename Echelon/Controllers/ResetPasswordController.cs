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
            if (ModelState.IsValid)
            {
                await _resetPasswordMediator.ResetPassword(forgottenPasswordModel.Email,
                    Url.Action("ResetPasswordForm", "ResetPassword", null, Request.Url?.Scheme));

                return RedirectToAction("ConfirmResetPassword");
            }

            ModelState.AddModelError("", @"Please enter a valid email!");
            return View(forgottenPasswordModel);
        }

        [HttpGet]
        public ActionResult ConfirmResetPassword()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> ResetPasswordForm(string id)
        {
            var resetPasswordViewModel = await _resetPasswordMediator.GetUserToResetById(id);
            if (resetPasswordViewModel != null)
            {
                return View(resetPasswordViewModel);
            }
            return RedirectToAction("Index", "Error");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPasswordForm(ResetPasswordViewModel resetPasswordModel)
        {
            if (ModelState.IsValid)
            {
                await _resetPasswordMediator.UpdateUser(resetPasswordModel);
                return RedirectToAction("ResetPasswordSuccess");
            }

            ModelState.AddModelError("", @"Please enter a valid password!");
            return View(resetPasswordModel);
        }

        public ActionResult ResetPasswordSuccess()
        {
            return View();
        }
    }
}