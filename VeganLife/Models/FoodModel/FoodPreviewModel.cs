using CommunityToolkit.Mvvm.Messaging;
using Newtonsoft.Json;
using SQLite;
using VeganLife.Helpers;
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

    public partial class FoodPreviewModel : ObservableObject
    {
        [ObservableProperty]
        byte _countCorrectWordOnSearch;
        string[] timeArr => Time?.Split('-');

        [ObservableProperty]
        bool _isBookmarked;
        [ObservableProperty]
        bool _isRead;
        public byte PrepTime
            => (timeArr != null && byte.TryParse(timeArr[0], out var i)) ? i : (byte)0;
        public byte CookTime
            => (timeArr != null && byte.TryParse(timeArr[1], out var i)) ? i : (byte)0;
        [RelayCommand]
        async Task BookmarkClicked()
        {
            IsBookmarked = !IsBookmarked;
            if (!await MainViewModel.DataStoreService.AddOrUpdateItemAsync(this, true))
                await ServicesHelper.GetService<INavigationService>().DisplayAlert("Error", "Oh, lỗi rồi!", "ok");
            WeakReferenceMessenger.Default.Send(new BookmarkFoodModelMessage(this));
        }
    }
}
