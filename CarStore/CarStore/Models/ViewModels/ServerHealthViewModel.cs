namespace CarStore.Models.ViewModels
{
    /// <summary>
    /// ViewModel to show the health of main and backup server
    /// </summary>
    public class ServerHealthViewModel
    {
        public string MainLiveness { get; set; }
        public string MainReadiness { get; set; }
        public string BackupLiveness { get; set; }
        public string BackupReadiness { get; set; }
    }
}
