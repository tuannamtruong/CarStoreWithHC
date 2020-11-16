using CarStore.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace CarStore.Components
{
    public class UptimeViewComponent : ViewComponent
    {
        private UptimeService uptime;

        public UptimeViewComponent(UptimeService uptime)
        {
            this.uptime = uptime;
        }

        public IViewComponentResult Invoke()
        {
            return View(uptime);
        }
    }
}
