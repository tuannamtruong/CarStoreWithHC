using CarStore.Components;
using CarStore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Routing;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CarStore.Test
{
    public class CategoryFilterMenuViewComponentTests
    {
        [Fact]
        public void Invoke_DifferenceCarsWithDifferenceCategories_SucceedFiltering()
        {
            Mock<ICarRepository> mock = new Mock<ICarRepository>();
            mock.Setup(m => m.Cars).Returns((new Car[]
            {
                new Car{CarId = 1, Category ="First",Name="SubjectOne"},
                new Car{CarId = 2, Category ="Second",Name="SubjectTwo"},
                new Car{CarId = 3, Category ="First",Name="SubjectThird"},
                new Car{CarId = 4, Category ="First",Name="SubjectFourth"},
            }).AsQueryable<Car>);
            CategoryFilterMenuViewComponent viewComponent = new CategoryFilterMenuViewComponent(mock.Object);
            var viewResult = ((IEnumerable<string>)(viewComponent.Invoke() as ViewViewComponentResult).ViewData.Model).ToArray();
            Assert.True(Enumerable.SequenceEqual(new string[] { "First", "Second" }, viewResult));
        }

        [Fact]
        public void Invoke_CategoryIsSelected_SelectedCategoryIsHighlighted()
        {
            Mock<ICarRepository> mock = new Mock<ICarRepository>();
            mock.Setup(m => m.Cars).Returns((new Car[]
            {
                new Car{CarId = 1, Category ="First",Name="SubjectOne"},
                new Car{CarId = 2, Category ="Second",Name="SubjectTwo"},
            }).AsQueryable<Car>);
            CategoryFilterMenuViewComponent viewComponent = new CategoryFilterMenuViewComponent(mock.Object);
            viewComponent.ViewComponentContext = new ViewComponentContext() { ViewContext = new ViewContext { RouteData = new RouteData() } };
            viewComponent.RouteData.Values["currentCategory"] = "First";
            string viewResult = (string)(viewComponent.Invoke() as ViewViewComponentResult).ViewData["SelectedCategory"];
            Assert.Equal("First", viewResult);
        }
    }
}
