using System.Web;
using Echelon.Misc.Attributes;

namespace Echelon.Models.ViewModels
{
    public class ProfileViewModel
    {
        [FileSize(250000)]
        [FileTypes("jpg, jpeg, png, gif")]
        public HttpPostedFileBase File { get; set; }

        public string AvatarUrl { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public bool UserNameEnabled { get; set; }
    }
}