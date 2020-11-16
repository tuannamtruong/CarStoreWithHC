using CarStore.Models;
using CarStore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace CarStore.Controllers
{
    public class CarController : Controller
    {
        private ICarRepository repository;
        public int Pagesize { get; set; } = 3;

        public CarController(ICarRepository repository)
        {
            this.repository = repository;
        }

        public ViewResult List(string currentCategory, int currentPage = 1)
        {
            return View(new CarListViewModel()
            {
                Cars = repository.Cars
                           .Where(c => c.Category == currentCategory || currentCategory == null)
                           .OrderBy(c => c.CarId)
                           .Skip((currentPage - 1) * Pagesize)
                           .Take(Pagesize),
                PagingInfo = new PagingInfo()
                {
                    ItemsPerPage = Pagesize,
                    CurrentPage = currentPage,
                    AmountItem = currentCategory == null ? repository.Cars.Count() :
                               repository.Cars.Where(c => c.Category == currentCategory).Count(),
                },
                CurrentCategory = currentCategory,
            });
        }
    }
}