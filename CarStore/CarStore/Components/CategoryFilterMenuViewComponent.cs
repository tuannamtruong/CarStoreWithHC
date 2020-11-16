using CarStore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace CarStore.Components
{
    public class CategoryFilterMenuViewComponent : ViewComponent
    {
        private ICarRepository repository;

        public CategoryFilterMenuViewComponent(ICarRepository repository)
        {
            this.repository = repository;
        }

        public IViewComponentResult Invoke()
        {
            ViewBag.SelectedCategory = RouteData?.Values["currentCategory"];
            return View(repository.Cars.OrderBy(c => c.Category).Select(c => c.Category).Distinct());
        }
    }
}
