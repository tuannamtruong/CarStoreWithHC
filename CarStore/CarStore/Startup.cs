using CarStore.Infrastructure;
using CarStore.Infrastructure.Middleware;
using CarStore.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CarStore
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration) => Configuration = configuration;

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<UptimeService>();
            services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlServer(Configuration["Data:CarStoreIdentity:ConnectionString"]));
            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AppIdentityDbContext>().AddDefaultTokenProviders();
            services.AddTransient<ICarRepository, EFCarRepository>();
            services.AddTransient<IOrderRepository, EFOrderRepository>();
            services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));                // When an object with datatype Cart is asked, the session will provide an session cart object.
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration["Data:CarStoreRepo:ConnectionString"]));
            services.AddMvc();
            services.AddMemoryCache();
            services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseMiddleware<TestMiddleware>();
                app.UseDeveloperExceptionPage();
                app.UseStatusCodePages();
            }
            else
            {
                app.UseExceptionHandler("/Error/Index");
            }
            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseSession();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: default,
                    template: "{controller}/{action}",
                    defaults: new { Controller = "Test", action = "Index" });
                routes.MapRoute(
                    name: null,
                    template: "{currentCategory}/Page{currentPage}",
                    defaults: new { Controller = "Car", action = "List" });
                routes.MapRoute(
                    name: null,
                    template: "Page{currentPage}",
                    defaults: new { Controller = "Car", action = "List", currentPage = 1 });
                routes.MapRoute(
                    name: null,
                    template: "{currentCategory}",
                    defaults: new { Controller = "Car", action = "List", currentPage = 1 });
                routes.MapRoute(
                    name: null,
                    template: "",
                    defaults: new { Controller = "Car", action = "List", currentPage = 1 });
                routes.MapRoute(name: null, template: "{controller}/{action}/{id?}");
            });
        }
    }
}
