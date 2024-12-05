namespace VeganLife.Models
{
    public partial class ChatLogsModel : ObservableObject
    {
        [PrimaryKey]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public DateTime ChatDate { get; set; }

        [ObservableProperty]
        private byte _timesLimit;

        [ObservableProperty]
        private byte _adWatchingLimit;
    }
}
