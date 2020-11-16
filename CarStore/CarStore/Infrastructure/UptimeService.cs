using System.Diagnostics;
using System.Globalization;

namespace CarStore.Infrastructure
{
    public class UptimeService
    {
        private Stopwatch timer;
        public UptimeService()
        {
            timer = Stopwatch.StartNew();
        }
        public string Uptime => timer.Elapsed.ToString(@"hh\:mm\:ss", new CultureInfo("de-DE"));
    }
}
