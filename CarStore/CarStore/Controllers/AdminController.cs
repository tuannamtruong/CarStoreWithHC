using CarStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace CarStore.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private ICarRepository repository;
        public AdminController(ICarRepository repository)
        {
            this.repository = repository;
        }
        public ViewResult Index() => View(repository.Cars);

        public ViewResult Edit(int carId) => View(repository.Cars.FirstOrDefault(c => c.CarId == carId));

        public ViewResult Create() => View("Edit", new Car());
        [HttpPost]
        public IActionResult Edit(Car Car)
        {
            if (ModelState.IsValid)
            {
                repository.AddOrUpdate(Car);
                TempData["message"] = $"{Car.Name} has been saved";
                return RedirectToAction("Index");
            }
            else // there is something wrong with the data values
                return View(Car);
        }

        [HttpPost]
        public IActionResult Delete(int carId)
        {
            Car car = repository.Delete(carId);
            if (car != null)
                TempData["message"] = $"{car.Name} is succesfully deleted";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult SeedDatabase()
        {
            //SeedData.EnsurePopulated(HttpContext.RequestServices);
            return RedirectToAction(nameof(Index));
        }
    }
}