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
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", @"Please enter a Username!");
                return View(profileViewModel);
            }

            await _profileMediator.UpdateUser(profileViewModel, Email);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UploadAvatar(ProfileViewModel profileViewModel)
        {
            await _profileMediator.UploadUserAvatar(profileViewModel.File, Email, Server.MapPath("~/UserAvatars/"));
            return RedirectToAction("Index");
        }
    }
}