using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using Echelon.Models.BusinessModels;
using Echelon.Models.ViewModels;

namespace Echelon.Controllers
{
    [Authorize]
    [RequireHttps]
    public class NavigationController : Controller
    {
        private IMapper _mapper;

        public NavigationController(IMapper mapper)
        {
            _mapper = mapper;
        }

        public ActionResult Index()
        {
            return View(new CategoriesViewModel()
            {
                Categories = new List<Category>()
                {
                    new Category{Name = "Gaming"},
                    new Category{Name = "Music"},
                    new Category{Name = "Work"},
                    new Category{Name = "Anime"}
                }
            });
        }
    }
}