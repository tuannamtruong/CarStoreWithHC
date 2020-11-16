using CarStore.Models;
using System.Linq;
using Xunit;

namespace CarStore.Test
{
    public class CartTests
    {
        [Fact]
        public void Add_AddNewItem_SucceedAddItemToCart()
        {
            Car car1 = new Car { CarId = 1, Name = "FCar", Category = "One" };
            Car car2 = new Car { CarId = 2, Name = "SCar", Category = "Two" };
            Cart cart = new Cart();
            cart.Add(car1, 1);
            cart.Add(car2, 2);
            var result = cart.GetPickedItems.ToList();
            Assert.Equal(2, result.Count());
            Assert.Equal(car1, result[0].Car);
            Assert.Equal(car2, result[1].Car);
        }
        [Fact]
        public void Add_AddDuplicatedItem_SucceedIncreseAmountOfItem()
        {
            Car car1 = new Car { CarId = 1, Name = "FCar", Category = "One" };
            Car car2 = new Car { CarId = 2, Name = "SCar", Category = "Two" };
            Cart cart = new Cart();
            cart.Add(car1, 1);
            cart.Add(car2, 2);
            cart.Add(car2, 3);
            var result = cart.GetPickedItems.ToList();
            Assert.Equal(2, result.Count());
            Assert.Equal(1, result[0].Amount);
            Assert.Equal(5, result[1].Amount);
        }
        [Fact]
        public void GetTotalPrice_CartHasMultipleItems_SucceedCalculate()
        {
            Car car1 = new Car { CarId = 1, Price = 5 };
            Car car2 = new Car { CarId = 2, Price = 8.9M };
            Cart cart = new Cart();
            cart.Add(car1, 1);
            cart.Add(car2, 2);
            cart.Add(car2, 3);
            var result = cart.GetTotalPrice();
            Assert.Equal(5 + 8.9M * 5, result);
        }
        [Fact]
        public void Remove_RemoveExistedItem_SucceedRemove()
        {
            Car car1 = new Car { CarId = 1, Price = 5 };
            Car car2 = new Car { CarId = 2, Price = 8.9M };
            Cart cart = new Cart();
            cart.Add(car1, 1);
            cart.Add(car2, 2);
            cart.Add(car2, 3);
            cart.Remove(car2);
            Assert.Single(cart.GetPickedItems);
            Assert.Empty(cart.GetPickedItems.Where(x => x.Car.CarId == 2));
        }
        [Fact]
        public void Clear_ClearExistedItems_SucceedClear()
        {
            Car car1 = new Car { CarId = 1, Price = 5 };
            Car car2 = new Car { CarId = 2, Price = 8.9M };
            Cart cart = new Cart();
            cart.Add(car1, 1);
            cart.Add(car2, 2);
            cart.Add(car2, 3);
            cart.Clear();
            Assert.Empty(cart.GetPickedItems);
        }
    }
}
