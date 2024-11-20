// <copyright file="BookmarkViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AsyncAwaitBestPractices;
using CommunityToolkit.Mvvm.Messaging;
using VeganLife.Data.LocalData;
using VeganLife.Helpers;
using VeganLife.Messages;
using VeganLife.Models.FoodModel;
using VeganLife.Services.LocalDataServices;
using VeganLife.Views.MainPageFlyout.FoodTab;

namespace VeganLife.ViewModels
{
    public partial class BookmarkViewModel : BaseViewModel, IRecipient<BookmarkFoodChangedMessage>
    {
        private readonly FoodPreviewDataStoreService _dataStoreService;

        [ObservableProperty]
        private ObservableCollection<FoodPreviewModel> _foods;
        [ObservableProperty]
        private FoodPreviewModel _foodSelected;

        public BookmarkViewModel()
            : base()
        {
            var database = FFImageLoading.Helpers.ServiceHelper.GetService<ISQLite>();
            if (database != null)
            {
                this._dataStoreService = new FoodPreviewDataStoreService(database);
            }

            this.Init();
            WeakReferenceMessenger.Default.Register<BookmarkFoodChangedMessage>(this);
        }

        /// <inheritdoc/>
        public override Task OnNavigatedFrom(bool isForwardNavigation)
        {
            this.FoodSelected = null;
            return base.OnNavigatedFrom(isForwardNavigation);
        }

        public void Receive(BookmarkFoodChangedMessage message)
        {
            if (message == null)
            {
                return;
            }

            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (message.Value.IsBookmarked)
                {
                    this.Foods.Add(message.Value);
                }
                else
                {
                    this.Foods.ToList().ForEach(i =>
                    {
                        if (i.Id.Equals(message.Value.Id))
                        {
                            this.Foods.Remove(i);
                        }
                    });
                }
            });
        }

        private void Init()
        {
            this.Foods = new ObservableCollection<FoodPreviewModel>();
            this.LoadDataAsync().SafeFireAndForget();
        }

        private async Task LoadDataAsync()
        {
            (await this._dataStoreService.GetItemsAsync())
                .Where(i => i.IsBookmarked)
                .ToList().ForEach(i => this.Foods.Add(i));
        }

        [RelayCommand]
        private async Task GoFoodDetail(object obj)
        {
            await this._dataStoreService.AddOrUpdateItemAsync(this.FoodSelected, true);
            await this.navigationService.NavigateToPage<FoodDetailPage>(paramater: obj);
            FoodSelected = null;
        }

        [RelayCommand]
        private async Task GoListPageAsync()
        {
            await Shell.Current.GoToAsync("//home/recipe");
        }

        // partial void OnFoodsChanged(ObservableCollection<FoodPreviewModel> value)
        // {
        //    Foods.ToList().ForEach(i =>
        //    {
        //        if (!i.IsBookmarked)
        //            Foods.Remove(i);
        //    });
        // }
    }
}
