using CarStore.Models.ViewModels;
using CarStore.POCO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CarStore.Controllers
{
    public class TestController : Controller
    {
        IConfiguration Configuration;
        ServerHealthViewModel ServerHealth;

        public TestController(IConfiguration configuration)
        {
            this.Configuration = configuration;
            ServerHealth = new ServerHealthViewModel();
            GetHealth();
        }

        private void GetHealth()
        {
            GetMainServerHealth();
            GetBackupServerHealth();
        }

        private void GetMainServerHealth()
        {
            try
            {
                var builder = new UriBuilder("http://localhost:8080/health/");
                var uri = builder.Uri;
                using (var client = new HttpClient())
                {
                    Task<HttpResponseMessage> taskResponse = Task.Run(() => client.GetAsync(uri));
                    var response = taskResponse.Result;
                    Task<string> taskResponseBody = Task.Run(() => response.Content.ReadAsStringAsync());
                    string responseBody = taskResponseBody.Result;
                    HealthResponse respond = JsonConvert.DeserializeObject<HealthResponse>(responseBody);
                    ServerHealth.MainReadiness = respond.Checks.Where(x => x.Name.Equals("Main database readiness check", StringComparison.InvariantCulture)).First().Status;
                    ServerHealth.MainLiveness = respond.Checks.Where(x => x.Name.Equals("Main database connection health check", StringComparison.InvariantCulture)).First().Status;
                }
            }
            catch (Exception e)
            {
                if (e is JsonReaderException || e is System.Net.Sockets.SocketException || e is HttpRequestException || e is AggregateException)
                {
                    ServerHealth.MainReadiness = "N/A";
                    ServerHealth.MainLiveness = "N/A";
                    return;
                }
                throw;
            }
        }

        private void GetBackupServerHealth()
        {
            try
            {
                var builder = new UriBuilder("http://localhost:9080/health/");
                var uri = builder.Uri;

                using (var client = new HttpClient())
                {
                    Task<HttpResponseMessage> taskResponse = Task.Run(() => client.GetAsync(uri));
                    var response = taskResponse.Result;
                    Task<string> taskResponseBody = Task.Run(() => response.Content.ReadAsStringAsync());
                    string responseBody = taskResponseBody.Result;
                    HealthResponse respond = JsonConvert.DeserializeObject<HealthResponse>(responseBody);
                    ServerHealth.BackupReadiness = respond.Checks.Where(x => x.Name.Equals("Backup database readiness check", StringComparison.InvariantCulture)).First().Status;
                    ServerHealth.BackupLiveness = respond.Checks.Where(x => x.Name.Equals("Backup database connection health check", StringComparison.InvariantCulture)).First().Status;
                }
            }
            catch (Exception e)
            {
                if (e is JsonReaderException || e is System.Net.Sockets.SocketException || e is HttpRequestException || e is AggregateException )
                {
                    ServerHealth.BackupReadiness = "N/A";
                    ServerHealth.BackupLiveness = "N/A";
                    return;
                }
                throw;
            }
        }

        public IActionResult Index()
        {
            return View(ServerHealth);
        }
        public IActionResult UseNoServer()
        {
            Configuration["Data:CarStoreRepo:ConnectionString"] = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Dummy;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            return View("Index", ServerHealth);
        }
        public IActionResult UseMainServer()
        {
            //Configuration["Data:CarStoreRepo:ConnectionString"] = "Server=desktop-4kjbsbv\\sqlexpress;Database=CarStore;Trusted_Connection=True;";
            //Configuration["Data:CarStoreRepo:ConnectionString"] = "Server=sqlserver://desktop-4kjbsbv\\sqlexpress;Database=CarStore;User Id=nam;Password=NAM;";
            Configuration["Data:CarStoreRepo:ConnectionString"] = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=CarStore;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

            return View("Index", ServerHealth);
        }
        public IActionResult UseBackupServer()
        {
            Configuration["Data:CarStoreRepo:ConnectionString"] = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=CarStoreBackup;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            return View("Index", ServerHealth);
        }
    }
}
