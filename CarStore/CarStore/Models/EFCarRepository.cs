using System.Linq;

namespace CarStore.Models
{
    public class EFCarRepository : ICarRepository
    {
        private ApplicationDbContext context;

        public EFCarRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IQueryable<Car> Cars => context.Cars;

        public void AddOrUpdate(Car car)
        {
            if (car.CarId == 0)
                context.Add(car);
            else
            {
                Car existedCar = context.Cars.FirstOrDefault(c => c.CarId == car.CarId);
                if (existedCar != null)
                {
                    existedCar.Name = car.Name;
                    existedCar.Price = car.Price;
                    existedCar.Category = car.Category;
                }
            }
            context.SaveChanges();
        }

        public Car Delete(int carId)
        {
            Car car = Cars.Where(x => x.CarId == carId).FirstOrDefault();
            if (car != null)
            {
                context.Cars.Remove(car);
                context.SaveChanges();
            }
            return car;
        }
    }
}
