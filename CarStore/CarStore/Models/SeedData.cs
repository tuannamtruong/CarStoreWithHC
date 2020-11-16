using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace CarStore.Models
{
    public static class SeedData
    {
        //public static void EnsurePopulated(IServiceProvider services)   
        //{
        //    ApplicationDbContext context = services.GetRequiredService<ApplicationDbContext>();
        //    //ApplicationDbContext context = app.ApplicationServices.GetRequiredService<ApplicationDbContext>();
        //    //context.Database.Migrate();
        //    if (!context.Cars.Any())
        //    {
        //        context.Cars.AddRange(
        //            new Car
        //            {
        //                Name = "Yellow Car",
        //                Category = "Color",
        //                Price = 12,
        //            }, new Car
        //            {
        //                Name = "Blue Car",
        //                Category = "Color",
        //                Price = 13
        //            }, new Car
        //            {
        //                Name = "Red Car",
        //                Category = "Color",
        //                Price = 14
        //            },
        //            new Car
        //            {
        //                Name = "Fast Car",
        //                Category = "Speed",
        //                Price = 21
        //            }, new Car
        //            {
        //                Name = "Slow Car",
        //                Category = "Speed",
        //                Price = 23
        //            }, new Car
        //            {
        //                Name = "Big Car",
        //                Category = "Size",
        //                Price = 24
        //            }, new Car
        //            {
        //                Name = "Small Car",
        //                Category = "Size",
        //                Price = 7
        //            });
        //        context.SaveChanges();
        //    }
        //}
    }
}
