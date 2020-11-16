using System.Linq;

namespace CarStore.Models
{
    public interface ICarRepository
    {
        IQueryable<Car> Cars { get; }
        void AddOrUpdate(Car car);
        Car Delete(int carId);
    }
}