using System.Collections.Generic;

namespace CarStore.Models.ViewModels
{
    /// <summary>
    /// ViewModel to show list car from database with pagination.
    /// </summary>
    public class CarListViewModel
    {
        public IEnumerable<Car> Cars { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string CurrentCategory { get; set; }
    }
}
