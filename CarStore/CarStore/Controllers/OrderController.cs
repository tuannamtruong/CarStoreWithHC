using CarStore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace CarStore.Controllers
{
    public class OrderController : Controller
    {
        private IOrderRepository repository;
        private Cart cart;

        public OrderController(IOrderRepository repository, Cart cartService)
        {
            this.repository = repository;
            this.cart = cartService;
        }

        #region View unshipped orders

        [HttpPost]
        [Authorize]
        public IActionResult MarkShipped(int orderId)
        {
            Order order = repository.Orders.FirstOrDefault(o => o.OrderId == orderId);
            if (order != null)
            {
                order.IsShipped = true;
                repository.SaveOrder(order);
            }
            return RedirectToAction(nameof(List));
        }
        [Authorize]
        public ViewResult List() => View(repository.Orders.Where(o => !o.IsShipped));

        #endregion

        #region Checkout

        public ViewResult Checkout() => View(new Order());
        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            if (!cart.GetPickedItems.Any())
                ModelState.AddModelError("", "Your shop cart is empty :(.");
            if (ModelState.IsValid)
            {
                order.CartItems = cart.GetPickedItems.ToArray();
                repository.SaveOrder(order);
                return RedirectToAction(nameof(Completed));
            }
            else
                return View(order);
        }
        public ViewResult Completed()
        {
            cart.Clear();
            return View();
        }

        #endregion Checkout
    }
}