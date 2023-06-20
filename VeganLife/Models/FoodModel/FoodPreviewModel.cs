// <copyright file="FoodPreviewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.Models.FoodModel
{
    using CommunityToolkit.Mvvm.Messaging;
    using Newtonsoft.Json;
    using SQLite;
    using VeganLife.Helpers;
    using VeganLife.Messages;

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
        private byte countCorrectWordOnSearch;

        private string[] TimeArr => this.Time?.Split('-');

        [ObservableProperty]
        private bool isBookmarked;

        [ObservableProperty]
        private bool isRead;

        public byte PrepTime
            => (this.TimeArr != null && byte.TryParse(this.TimeArr[0], out var i)) ? i : (byte)0;

        public byte CookTime
            => (this.TimeArr != null && byte.TryParse(this.TimeArr[1], out var i)) ? i : (byte)0;

        [RelayCommand]
        private async Task BookmarkClicked()
        {
            this.IsBookmarked = !this.IsBookmarked;
            if (!await MainViewModel.DataStoreService.AddOrUpdateItemAsync(this, true))
            {
                await ServicesHelper.GetService<INavigationService>().DisplayAlert("Error", "Oh, lỗi rồi!", "ok");
            }

            WeakReferenceMessenger.Default.Send(new BookmarkFoodModelMessage(this));
        }
    }
}
