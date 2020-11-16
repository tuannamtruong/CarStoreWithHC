using CarStore.Models;
using CarStore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace CarStore.Controllers
{
    public class CartController : Controller
    {
        private ICarRepository repository;
        private Cart cart;

        public CartController(ICarRepository repository, Cart cartService)
        {
            this.repository = repository;
            cart = cartService;
        }

        /// <summary>
        /// Showing items in the cart
        /// </summary>
        /// <param name="returnUrl">State of cart after a modification</param>
        [Route("MyCart")]
        public ViewResult Index(string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                Cart = cart,
                ReturnUrl = returnUrl
            });
        }
        public RedirectToActionResult AddToCart(int carId, string returnUrl)
        {
            Car car = repository.Cars.FirstOrDefault(x => x.CarId == carId);
            // If a car with this id is existed in repo, add to cart
            if (car != null)
                cart.Add(car);
            return RedirectToAction("Index", new { returnUrl });
        }
        public RedirectToActionResult RemoveToCart(int carId, string returnUrl)
        {
            Car car = repository.Cars.FirstOrDefault(x => x.CarId == carId);
            // If a car with this id is existed in repo, remove from cart
            if (car != null)
                cart.Remove(car);
            return RedirectToAction("Index", new { returnUrl });
        }
    }
}