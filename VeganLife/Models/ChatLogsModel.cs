using SQLite;

namespace VeganLife.Models
{
    public partial class ChatLogsModel : ObservableObject
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public DateTime ChatDate { get; set; }
        [ObservableProperty]
        private byte _timesLimit;
        [ObservableProperty]
        private byte _adWatchingLimit;
    }
}
