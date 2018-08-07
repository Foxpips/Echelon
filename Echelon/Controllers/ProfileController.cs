using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Echelon.Mediators;
using Echelon.Models.ViewModels;

namespace Echelon.Controllers
{
    [Authorize]
    [RequireHttps]
    public class ProfileController : BaseController
    {
        private readonly ProfileMediator _profileMediator;
        private string Email => Request.GetOwinContext().Authentication.User.Identity.Name;

        public ProfileController(ProfileMediator profileMediator)
        {
            _profileMediator = profileMediator;
        }

        public async Task<ActionResult> Index()
        {
            return View(await _profileMediator.GetUser(Email));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(ProfileViewModel profileViewModel)
        {
            if (ModelState.IsValid)
            {
                await _profileMediator.UpdateUser(profileViewModel, Email);
                return View();
            }

            ModelState.AddModelError("", @"Please enter a Username!");
            return View(profileViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UploadAvatar(AvatarViewModel avatarViewModel)
        {
            if (ModelState.IsValid)
            {
                await _profileMediator.UploadUserAvatar(avatarViewModel.File, Email, Server.MapPath("~/UserAvatars/"));
                return RedirectToAction("Index");
            }

            return View("Index");
        }
    }
}