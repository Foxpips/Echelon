using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Echelon.Mediators;
using Echelon.Models.ViewModels;

namespace Echelon.Controllers
{
    [Authorize]
    [RequireHttps]
    public class ProfileController : Controller
    {
        private readonly ProfileMediator _profileMediator;
        private string Email => Request.GetOwinContext().Authentication.User.Identity.Name;

        public ProfileController(ProfileMediator profileMediator)
        {
            _profileMediator = profileMediator;
        }

        [Authorize]
        public async Task<ActionResult> Index()
        {
            return View(await _profileMediator.GetUser(Email));
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(ProfileViewModel profileViewModel)
        {
            await _profileMediator.UpdateUser(profileViewModel, Email);
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UploadAvatar(ProfileViewModel profileViewModel)
        {
            if (ModelState.IsValid)
            {
                 await _profileMediator.UploadUserAvatar(profileViewModel.File, Email, Server.MapPath("~/UserAvatars/"));
            }

            return RedirectToAction("Index");
        }
    }
}