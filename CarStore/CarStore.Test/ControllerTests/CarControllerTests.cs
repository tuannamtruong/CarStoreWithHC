using CarStore.Controllers;
using CarStore.Models;
using CarStore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Linq;
using Xunit;

namespace CarStore.Test.ControllerTests
{
    public class CarControllerTests
    {
        [Fact]
        public void List_CarIsFilteredByCategory_CorrectlyShowAmountOfPage()
        {
            Mock<ICarRepository> mock = new Mock<ICarRepository>();
            mock.Setup(m => m.Cars).Returns((new Car[]
            {
                new Car{CarId=1,Name="FCar",Category="One"},
                new Car{CarId=2,Name="SCar",Category="Two"},
                new Car{CarId=3,Name="TCar",Category="One"},
                new Car{CarId=4,Name="FoCar",Category="Two"},
                new Car{CarId=5,Name="FiCar",Category="One"},
            }).AsQueryable<Car>());
            CarController controller = new CarController(mock.Object) { Pagesize = 2 };
            PagingInfo getPagingInfo(ViewResult result) => (result?.ViewData?.Model as CarListViewModel)?.PagingInfo;

            Assert.Equal(2, getPagingInfo(controller.List("One")).AmountPages);
            Assert.Equal(3, getPagingInfo(controller.List("One")).AmountItem);
            Assert.Equal(1, getPagingInfo(controller.List("Two")).AmountPages);
            Assert.Equal(2, getPagingInfo(controller.List("Two")).AmountItem);
            Assert.Equal(3, getPagingInfo(controller.List(null)).AmountPages);
            Assert.Equal(5, getPagingInfo(controller.List(null)).AmountItem);
        }

        [Fact]
        public void List_AmountItemBiggerThanPageSize_SucceedPaginate()
        {
            Mock<ICarRepository> mock = new Mock<ICarRepository>();
            mock.Setup(m => m.Cars).Returns((new Car[]
            {
                new Car{CarId=1,Name="FCar"},
                new Car{CarId=2,Name="SCar"},
                new Car{CarId=3,Name="TCar"},
                new Car{CarId=4,Name="FoCar"},
                new Car{CarId=5,Name="FiCar"},
            }).AsQueryable<Car>());
            CarController controller = new CarController(mock.Object) { Pagesize = 3 };
            CarListViewModel pageLinkTagHelper = controller.List(null, 2).ViewData.Model as CarListViewModel;
            Car[] carResult = pageLinkTagHelper.Cars.ToArray();
            Assert.True(carResult.Length == 2);
            Assert.Equal("FoCar", carResult[0].Name);
            Assert.Equal("FiCar", carResult[1].Name);
        }

        [Fact]
        public void List_AmountItemBiggerThanPageSize_CorrectlyViewModelPagingInfo()
        {
            Mock<ICarRepository> mock = new Mock<ICarRepository>();
            mock.Setup(m => m.Cars).Returns((new Car[]
            {
                new Car{CarId=1,Name="FCar"},
                new Car{CarId=2,Name="SCar"},
                new Car{CarId=3,Name="TCar"},
                new Car{CarId=4,Name="FoCar"},
                new Car{CarId=5,Name="FiCar"},
            }).AsQueryable<Car>());
            CarController controller = new CarController(mock.Object) { Pagesize = 3 };
            CarListViewModel viewModel = controller.List(null, 2).ViewData.Model as CarListViewModel;
            PagingInfo result = viewModel.PagingInfo;
            Assert.Equal(5, result.AmountItem);
            Assert.Equal(2, result.AmountPages);
            Assert.Equal(2, result.CurrentPage);
            Assert.Equal(3, result.ItemsPerPage);
        }

        [Fact]
        public void List_CarWithDifferentsCategories_SucceedFiltering()
        {
            Mock<ICarRepository> mock = new Mock<ICarRepository>();
            mock.Setup(m => m.Cars).Returns((new Car[]
            {
                new Car{CarId=1,Name="FCar",Category="One"},
                new Car{CarId=2,Name="SCar",Category="Two"},
                new Car{CarId=3,Name="TCar",Category="One"},
                new Car{CarId=4,Name="FoCar",Category="Two"},
                new Car{CarId=5,Name="FiCar",Category="One"},
            }).AsQueryable<Car>());
            CarController controller = new CarController(mock.Object) { Pagesize = 3 };
            CarListViewModel viewModelResult = controller.List("One").ViewData.Model as CarListViewModel;
            Car[] carsResult = viewModelResult.Cars.ToArray();
            Assert.Equal(3, viewModelResult.Cars.Count());
            Assert.Equal("One", viewModelResult.CurrentCategory);
            Assert.True(carsResult[0].Name == "FCar" && carsResult[0].Category == "One");
            Assert.True(carsResult[1].Name == "TCar" && carsResult[1].Category == "One");
            Assert.True(carsResult[2].Name == "FiCar" && carsResult[2].Category == "One");
        }
    }
}
