using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CarStore.Models
{
    public class EFOrderRepository : IOrderRepository
    {
        private ApplicationDbContext context;

        public EFOrderRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IQueryable<Order> Orders => context.Orders.Include(o => o.CartItems).ThenInclude(ci => ci.Car);

        public void SaveOrder(Order order)
        {
            context.AttachRange(order.CartItems.Select(ci => ci.Car));
            if (order.OrderId == 0)
                context.Orders.Add(order);
            context.SaveChanges();
        }
    }
}
