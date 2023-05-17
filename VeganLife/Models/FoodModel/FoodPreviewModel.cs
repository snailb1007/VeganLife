using CommunityToolkit.Mvvm.Messaging;
using Newtonsoft.Json;
using PropertyChanged;
using SQLite;
using VeganLife.Messages;

namespace VeganLife.Models.FoodModel
{
    public partial class FoodPreviewModel
    {
        [JsonIgnore, PrimaryKey]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("time")]
        public string Time { get; set; }
        [JsonProperty("category")]
        public string Category { get; set; }
    }

    [AddINotifyPropertyChangedInterface]
    public partial class FoodPreviewModel
    {
        string[] timeArr => Time?.Split('-');

        public bool IsBookmarked { get; set; }
        public bool IsRead { get; set; }
        public byte PrepTime
            => (timeArr != null && byte.TryParse(timeArr[0], out var i)) ? i : (byte)0;
        public byte CookTime
            => (timeArr != null && byte.TryParse(timeArr[1], out var i)) ? i : (byte)0;
        [RelayCommand]
        void BookmarkClicked()
        {
            WeakReferenceMessenger.Default.Send(new BookmarkFoodModelMessage(this));
        }
    }
}
