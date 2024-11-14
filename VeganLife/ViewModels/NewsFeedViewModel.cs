// <copyright file="NewsFeedViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using VeganLife.Helpers.AppSetting;
using VeganLife.Helpers.Extensions;
using VeganLife.Models.GoogleNewsModels;
using VeganLife.Resources.Translations;

namespace VeganLife.ViewModels
{
    /// <summary>
    /// vm for NewsFeedPage.
    /// </summary>
    public partial class NewsFeedViewModel : BaseViewModel
    {
        private TaskCompletionSource<bool> _taskLoadingMessage;
        private IEnumerable<Item> _dataFood = new List<Item>();
        private IEnumerable<Item> _dataHealthy = new List<Item>();
        private IEnumerable<Item> _dataReligion = new List<Item>();
        private IEnumerable<Item> _dataLiveStrong = new List<Item>();
        private bool _isFoodFeed = true;
        private bool _isHealthyFeed;
        private bool _isLiveStrongFeed;
        private byte _currentNumberItem;

        [ObservableProperty]
        private ObservableCollection<Item> _feeds;

        [ObservableProperty]
        private ObservableCollection<Discovery> _discoveryMenu;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewsFeedViewModel"/> class.
        /// </summary>
        public NewsFeedViewModel()
            : base()
        {
        }

        public override async Task ViewAppearingVM()
        {
            if (!isInitialized)
            {
                LoadData();
                isInitialized = true;
            }

            await base.ViewAppearingVM();
        }

        [RelayCommand]
        private void RefreshFoods()
        {
            if (this.IsLoading)
            {
                return;
            }

            this.LoadData();
        }

        [RelayCommand]
        private void SelectDiscoveryMenu(object obj)
        {
            if (this.IsLoading)
            {
                return;
            }

            this.busyManager.Increase();
            var currentItem = (Discovery)obj;
            if (currentItem == null || this.DiscoveryMenu?.Where(i => i.IsSelected)?.FirstOrDefault() == currentItem)
            {
                return;
            }

            this._currentNumberItem = 20;
            this.Feeds?.Clear();
            if (currentItem.Title.Equals(AppResources.veganFood_feedPage))
            {
                this.SetFlagDiscoverySelected(isFood: true);
                if (!this._dataFood?.Any() ?? true)
                {
                    this._dataFood = this.dataService.ReadRssFeed(ConstantHelper.RssFeedNews.GoogleNewsVeganFoods);
                }

                foreach (var item in this._dataFood?.Take(this._currentNumberItem)!)
                {
                    this.Feeds?.Add(item);
                }
            }
            else if (currentItem.Title.Equals(AppResources.healthy_feedPage))
            {
                this.SetFlagDiscoverySelected(isHealthy: true);
                if (!this._dataHealthy?.Any() ?? true)
                {
                    this._dataHealthy = dataService.ReadRssFeed(ConstantHelper.RssFeedNews.GoogleNewsVeganHealthy);
                }

                foreach (var item in this._dataHealthy?.Take(this._currentNumberItem)!)
                {
                    this.Feeds?.Add(item);
                }
            }
            else if (currentItem.Title.Equals(AppResources.religion_feedPage))
            {
                this.SetFlagDiscoverySelected();
                if (!this._dataReligion?.Any() ?? true)
                {
                    this._dataReligion = this.dataService.ReadRssFeed(ConstantHelper.RssFeedNews.GoogleNewsReligion);
                }

                MainThread.BeginInvokeOnMainThread(() => this.Feeds
                    = this._dataReligion == null ? [] : new ObservableCollection<Item>(this._dataReligion));
            }
            else if (currentItem.Title.Equals(AppResources.liveStrong_feedPage))
            {
                this.SetFlagDiscoverySelected(liveStrong: true);
                if (!this._dataLiveStrong?.Any() ?? true)
                {
                    this._dataLiveStrong = this.dataService.ReadRssFeed(ConstantHelper.RssFeedNews.GoogleNewsLiveStrong);
                }

                MainThread.BeginInvokeOnMainThread(() => this.Feeds
                    = this._dataLiveStrong is null ? [] : new ObservableCollection<Item>(this._dataLiveStrong));
            }

            foreach (var item in this.DiscoveryMenu!)
            {
                item.IsSelected = false;
            }

            currentItem.IsSelected = true;
            this.busyManager.Decrease();
        }

        [RelayCommand]
        private async Task LoadMoreItem()
        {
            if (this._taskLoadingMessage != null && !this._taskLoadingMessage.Task.IsCompleted)
            {
                await this._taskLoadingMessage.Task;
            }

            this._taskLoadingMessage = new TaskCompletionSource<bool>();
            IEnumerable<Item> listTemp;
            if (this._isFoodFeed)
            {
                listTemp = this._dataFood;
            }
            else if (this._isHealthyFeed)
            {
                listTemp = this._dataHealthy;
            }
            else
            {
                listTemp = this._isLiveStrongFeed ? this._dataLiveStrong : this._dataReligion;
            }

            var isLoadedAllData = this.Feeds?.Count > 0 && this.Feeds?.Count == listTemp?.Count();
            if (!isLoadedAllData)
            {
                for (var i = 0; i < 10 && (i + this._currentNumberItem) < listTemp?.Count(); i++)
                {
                    this.Feeds?.Add(listTemp.ElementAt(i + this._currentNumberItem));
                }

                this._currentNumberItem += 10;
            }

            this._taskLoadingMessage.TrySetResult(true);
        }

        [RelayCommand]
        private async Task SelectFeedItem(object obj)
        {
            if (this.IsLoading)
            {
                return;
            }

            var item = obj as Item;
            try
            {
                if (item == null)
                {
                    return;
                }

                Uri uri = new Uri(item.link);
                await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
            }
            catch (Exception ex)
            {
                ex.LogError(description: "No browser may be installed on the device");
            }
        }

        private void LoadData()
        {
            if (this.Feeds != null && this.Feeds.Any())
            {
                this.Feeds.Clear();
            }

            if (this.DiscoveryMenu != null && this.DiscoveryMenu.Any())
            {
                this.DiscoveryMenu.Clear();
            }

            this._dataFood = this.dataService.ReadRssFeed(ConstantHelper.RssFeedNews.GoogleNewsVeganFoods);
            this.InitMenu();
            this._currentNumberItem = 20;
            this.Feeds = new ObservableCollection<Item>(this._dataFood.Take(this._currentNumberItem));
        }

        private void InitMenu()
        {
            this.DiscoveryMenu =
            [
                new Discovery()
                {
                    ImgSource = "https://i.imgur.com/anDoRUb.jpg", Title = AppResources.veganFood_feedPage,
                    IsSelected = true
                },
                new Discovery()
                {
                    ImgSource = "https://i.imgur.com/tH9PSUe.jpg", Title = AppResources.healthy_feedPage,
                    IsSelected = false
                },
                new Discovery()
                {
                    ImgSource = "https://i.imgur.com/dlDuKhf.jpgg", Title = AppResources.religion_feedPage,
                    IsSelected = false
                },
                new Discovery()
                {
                    ImgSource = "https://i.imgur.com/sySiZVa.jpg", Title = AppResources.liveStrong_feedPage,
                    IsSelected = false
                }
            ];
        }

        private void SetFlagDiscoverySelected(bool isFood = false, bool isHealthy = false, bool liveStrong = false)
        {
            this._isFoodFeed = isFood;
            this._isHealthyFeed = isHealthy;
            this._isLiveStrongFeed = liveStrong;
        }
    }
}
