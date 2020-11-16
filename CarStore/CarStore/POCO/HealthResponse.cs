using System.Collections.Generic;

namespace CarStore.POCO
{
    public class HealthResponse
    {
        public string Status { get; set; }
        public List<Check> Checks { get; set; }
    }

    public class Check
    {
        public string Name { get; set; }
        public string Status { get; set; }
    }

}
