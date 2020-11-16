namespace CarStore.Models.ViewModels
{
    /// <summary>
    /// ViewModel for buying items Cart of user
    /// </summary>
    public class CartIndexViewModel
    {
        public Cart Cart { get; set; }
        public string ReturnUrl { get; set; }
    }
}
