using System;

namespace CarStore.Models.ViewModels
{
    public class PagingInfo
    {
        public int AmountItem { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }
        public int AmountPages => (int)Math.Ceiling((decimal)AmountItem / ItemsPerPage);
    }
}
