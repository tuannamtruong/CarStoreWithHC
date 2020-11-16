using System.Collections.Generic;
using System.Linq;

namespace CarStore.Models
{
    public class Cart
    {
        private List<CartItem> PickedItems { get; set; } = new List<CartItem>();

        public virtual void Add(Car car, int amount = 1)
        {
            CartItem item = PickedItems.Where(x => car.CarId == x.Car.CarId).FirstOrDefault();
            if (item == null)
            {
                item = new CartItem
                {
                    Car = car,
                    Amount = amount
                };
                PickedItems.Add(item);
            }
            else
                item.Amount += amount;
        }
        public virtual void Remove(Car car) => PickedItems.RemoveAll(x => x.Car.CarId == car.CarId);
        public virtual void Clear() => PickedItems.Clear();
        public virtual IEnumerable<CartItem> GetPickedItems => PickedItems;
        public virtual decimal GetTotalPrice() => PickedItems.Sum(i => i.Amount * i.Car.Price);
    }

    public class CartItem
    {
        public int CartItemId { get; set; }
        public Car Car { get; set; }
        public int Amount { get; set; }
    }
}