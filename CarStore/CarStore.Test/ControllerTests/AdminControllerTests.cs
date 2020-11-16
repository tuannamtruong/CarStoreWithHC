using CarStore.Controllers;
using CarStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CarStore.Test.ControllerTests
{
    public class AdminControllerTests
    {
        [Fact]
        public void Delete_ValidCar_SucceedDelete()
        {
            Car car = new Car { CarId = 4, Name = "FoCar", Category = "Four" };
            Mock<ICarRepository> mock = new Mock<ICarRepository>();
            mock.Setup(m => m.Cars).Returns((new Car[]
            {
                new Car{CarId=1,Name="FCar",Category="One"},
                new Car{CarId=2,Name="SCar",Category="Two"},
                new Car{CarId=3,Name="TCar",Category="One"},
                car
            }).AsQueryable<Car>());
            AdminController adminController = new AdminController(mock.Object);
            adminController.Delete(car.CarId);
            mock.Verify(m => m.Delete(car.CarId));
        }

        [Fact]
        public void Edit_ValidNewCarWithInvalidModelState_FailedSave()
        {
            Mock<ICarRepository> mock = new Mock<ICarRepository>();
            AdminController adminController = new AdminController(mock.Object);
            Car car = new Car { Name = "FCar", Category = "One" };
            adminController.ModelState.AddModelError("error", "error");
            IActionResult result = adminController.Edit(car);
            mock.Verify(m => m.AddOrUpdate(It.IsAny<Car>()), Times.Never());
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Edit_ValidNewCar_SucceedSave()
        {
            Mock<ICarRepository> mock = new Mock<ICarRepository>();
            Mock<ITempDataDictionary> tempData = new Mock<ITempDataDictionary>();
            AdminController adminController = new AdminController(mock.Object)
            {
                TempData = tempData.Object
            };
            Car car = new Car { Name = "FCar", Category = "One" };
            IActionResult result = adminController.Edit(car);
            mock.Verify(m => m.AddOrUpdate(car));
            Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", (result as RedirectToActionResult).ActionName);
        }
        [Fact]
        public void Edit_InvalidCarId_ReceivingNull()
        {
            Mock<ICarRepository> mock = new Mock<ICarRepository>();
            mock.Setup(m => m.Cars).Returns((new Car[]
            {
                new Car{CarId=1,Name="FCar",Category="One"},
                new Car{CarId=2,Name="SCar",Category="Two"},
                new Car{CarId=3,Name="TCar",Category="One"},
            }).AsQueryable<Car>());
            AdminController adminController = new AdminController(mock.Object);
            Car result = GetViewModel<Car>(adminController.Edit(4));
            Assert.Null(result);
        }

        [Fact]
        public void Edit_ValidCarId_SucceedReceivingCarObject()
        {
            Mock<ICarRepository> mock = new Mock<ICarRepository>();
            mock.Setup(m => m.Cars).Returns((new Car[]
            {
                new Car{CarId=1,Name="FCar",Category="One"},
                new Car{CarId=2,Name="SCar",Category="Two"},
                new Car{CarId=3,Name="TCar",Category="One"},
            }).AsQueryable<Car>());
            AdminController adminController = new AdminController(mock.Object);
            Car p1 = GetViewModel<Car>(adminController.Edit(1));
            Car p2 = GetViewModel<Car>(adminController.Edit(2));
            Car p3 = GetViewModel<Car>(adminController.Edit(3));
            Assert.Equal(1, p1.CarId);
            Assert.Equal(2, p2.CarId);
            Assert.Equal(3, p3.CarId);
        }

        [Fact]
        public void Index_DifferentCarExistInDatabase_SucceedRetrievedAllData()
        {
            Mock<ICarRepository> mock = new Mock<ICarRepository>();
            mock.Setup(m => m.Cars).Returns((new Car[]
            {
                new Car{CarId=1,Name="FCar",Category="One"},
                new Car{CarId=2,Name="SCar",Category="Two"},
                new Car{CarId=3,Name="TCar",Category="One"},
            }).AsQueryable<Car>());
            AdminController adminController = new AdminController(mock.Object);
            Car[] result = GetViewModel<IEnumerable<Car>>(adminController.Index())?.ToArray();
            Assert.Equal(3, result.Length);
            Assert.Equal("FCar", result[0].Name);
            Assert.Equal("SCar", result[1].Name);
            Assert.Equal("TCar", result[2].Name);
        }
        /// <summary>
        /// Unpack the result from the action method and get the view model data.
        /// </summary>
        private T GetViewModel<T>(IActionResult result) where T : class
        {
            return (result as ViewResult)?.ViewData.Model as T;
        }
    }
}
