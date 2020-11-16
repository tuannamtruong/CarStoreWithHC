using CarStore.Controllers;
using CarStore.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CarStore.Test.ControllerTests
{
    public class OrderControllerTests
    {
        [Fact]
        public void Checkout_EmptyCart_DenyCheckout()
        {
            Mock<IOrderRepository> mockRepo = new Mock<IOrderRepository>();
            Cart cart = new Cart();
            OrderController orderController = new OrderController(mockRepo.Object, cart);
            Order order = new Order();
            var result = orderController.Checkout(order) as ViewResult;
            // Verify that the Controller repond to EmptyCart with an Error
            Assert.False(orderController.ModelState.IsValid);
            // As the ModelState is set to error, no Create operation is executed
            mockRepo.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never);  //m = mockRepo.Object
            // Controller responses with the default view
            Assert.True(string.IsNullOrEmpty(result.ViewName));
        }

        [Fact]
        public void Checkout_InvalidShippingDetails_DenyCheckout()
        {
            Mock<IOrderRepository> mockRepo = new Mock<IOrderRepository>();
            Cart cart = new Cart();
            cart.Add(new Car());
            OrderController orderController = new OrderController(mockRepo.Object, cart);
            orderController.ModelState.AddModelError("error", "error");
            var result = orderController.Checkout(new Order()) as ViewResult;
            // As the ModelState is set to error, no Create operation is executed
            mockRepo.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never);  //m = mockRepo.Object
            // Controller responses with the default view
            Assert.True(string.IsNullOrEmpty(result.ViewName));
            // Check that an invalid model is passed to the view
            Assert.False(result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public void Checkout_CartWithItemAndValidDetails_Success()
        {
            Mock<IOrderRepository> mockRepo = new Mock<IOrderRepository>();
            Cart cart = new Cart();
            cart.Add(new Car());
            OrderController orderController = new OrderController(mockRepo.Object, cart);
            var result = orderController.Checkout(new Order()) as RedirectToActionResult;
            mockRepo.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Once);  //m = mockRepo.Object
            Assert.Equal("Completed", result.ActionName);
        }
    }
}
