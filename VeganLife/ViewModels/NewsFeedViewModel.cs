// <copyright file="NewsFeedViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.ViewModels
{
    using System.ServiceModel.Syndication;
    using VeganLife.Helpers.AppSetting;
    using VeganLife.Models.GoogleNewsModels;
    using VeganLife.Resources.Translations;

    /// <summary>
    /// vm for NewsFeedPage.
    /// </summary>
    public partial class NewsFeedViewModel : BaseViewModel
    {
        private TaskCompletionSource<bool> taskLoadingMessage;
        private IEnumerable<Item> dataFood = new List<Item>();
        private IEnumerable<Item> dataHealthy = new List<Item>();
        private IEnumerable<Item> dataReligion = new List<Item>();
        private IEnumerable<Item> dataLiveStrong = new List<Item>();
        private bool isFoodFeed = true;
        private bool isHealthyFeed;
        private bool isLiveStrongFeed;
        private byte currentNumberItem;

        [ObservableProperty]
        private ObservableCollection<Item> feeds;

        [ObservableProperty]
        private ObservableCollection<Discovery> discoveryMenu;


        /// <summary>
        /// Initializes a new instance of the <see cref="NewsFeedViewModel"/> class.
        /// </summary>
        public NewsFeedViewModel()
            : base()
        {
        }

        public override Task ViewAppearingVM()
        {
            if (!isInitialized)
            {
                LoadData();
                isInitialized = true;
            }

            return base.ViewAppearingVM();
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

            this.IsLoading = true;
            var currentItem = (Discovery)obj;
            if (currentItem == null || this.DiscoveryMenu?.Where(i => i.IsSelected)?.FirstOrDefault() == currentItem)
            {
                this.IsLoading = false;
                return;
            }

            this.currentNumberItem = 20;
            this.Feeds?.Clear();
            if (currentItem.Title.Equals(AppResources.veganFood_feedPage))
            {
                this.SetFlagDiscoverySelected(isFood: true);
                if (!this.dataFood?.Any() ?? true)
                {
                    this.dataFood = this.dataService.ReadRssFeed(ConstantHelper.RssFeedNews.GoogleNewsVeganFoods);
                }

                foreach (var item in this.dataFood?.Take(this.currentNumberItem)!)
                {
                    this.Feeds?.Add(item);
                }
            }
            else if (currentItem.Title.Equals(AppResources.healthy_feedPage))
            {
                this.SetFlagDiscoverySelected(isHealthy: true);
                if (!this.dataHealthy?.Any() ?? true)
                {
                    this.dataHealthy = dataService.ReadRssFeed(ConstantHelper.RssFeedNews.GoogleNewsVeganHealthy);
                }

                foreach (var item in this.dataHealthy?.Take(this.currentNumberItem)!)
                {
                    this.Feeds?.Add(item);
                }
            }
            else if (currentItem.Title.Equals(AppResources.religion_feedPage))
            {
                this.SetFlagDiscoverySelected();
                if (!this.dataReligion?.Any() ?? true)
                {
                    this.dataReligion = this.dataService.ReadRssFeed(ConstantHelper.RssFeedNews.GoogleNewsReligion);
                }

                MainThread.BeginInvokeOnMainThread(() => this.Feeds = new ObservableCollection<Item>(this.dataReligion));
            }
            else if (currentItem.Title.Equals(AppResources.liveStrong_feedPage))
            {
                this.SetFlagDiscoverySelected(liveStrong: true);
                if (!this.dataLiveStrong?.Any() ?? true)
                {
                    this.dataLiveStrong = this.dataService.ReadRssFeed(ConstantHelper.RssFeedNews.GoogleNewsLiveStrong);
                }

                MainThread.BeginInvokeOnMainThread(() => this.Feeds = new ObservableCollection<Item>(this.dataLiveStrong));
            }

            foreach (var item in this.DiscoveryMenu!)
            {
                item.IsSelected = false;
            }

            currentItem.IsSelected = true;
            this.IsLoading = false;
        }

        [RelayCommand]
        private async Task LoadMoreItem()
        {
            if (this.taskLoadingMessage != null && !this.taskLoadingMessage.Task.IsCompleted)
            {
                await this.taskLoadingMessage.Task;
            }

            this.taskLoadingMessage = new TaskCompletionSource<bool>();
            IEnumerable<Item> listTemp;
            if (this.isFoodFeed)
            {
                listTemp = this.dataFood;
            }
            else if (this.isHealthyFeed)
            {
                listTemp = this.dataHealthy;
            }
            else
            {
                listTemp = this.isLiveStrongFeed ? this.dataLiveStrong : this.dataReligion;
            }

            bool isLoadedAllData = this.Feeds?.Count > 0 && this.Feeds?.Count == listTemp?.Count();
            if (!isLoadedAllData)
            {
                for (int i = 0; i < 10 && (i + this.currentNumberItem) < listTemp?.Count(); i++)
                {
                    this.Feeds?.Add(listTemp.ElementAt(i + this.currentNumberItem));
                }

                this.currentNumberItem += 10;
            }

            this.taskLoadingMessage.TrySetResult(true);
        }

        [RelayCommand]
        private async Task SelectFeedItem(object obj)
        {
            if (this.IsLoading)
            {
                return;
            }

            this.IsLoading = true;
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
                _ = ex;
#if DEBUG
                // An unexpected error occured. No browser may be installed on the device.
                await Console.Out.WriteLineAsync("No browser may be installed on the device\n" + ex.Message);
#endif
            }
            finally
            {
                this.IsLoading = false;
            }
        }

        private void LoadData()
        {
            this.IsLoading = true;
            if (this.Feeds != null && this.Feeds.Any())
            {
                this.Feeds.Clear();
            }

            if (this.DiscoveryMenu != null && this.DiscoveryMenu.Any())
            {
                this.DiscoveryMenu.Clear();
            }

            this.dataFood = this.dataService.ReadRssFeed(ConstantHelper.RssFeedNews.GoogleNewsVeganFoods);
            this.InitMenu();
            this.currentNumberItem = 20;
            this.Feeds = new ObservableCollection<Item>(this.dataFood.Take(this.currentNumberItem));
            this.IsLoading = false;
        }

        private void InitMenu()
        {
            this.DiscoveryMenu = new ObservableCollection<Discovery>()
            {
                new Discovery() { ImgSource = "https://i.imgur.com/anDoRUb.jpg", Title = AppResources.veganFood_feedPage, IsSelected = true },
                new Discovery() { ImgSource = "https://i.imgur.com/tH9PSUe.jpg", Title = AppResources.healthy_feedPage, IsSelected = false },
                new Discovery() { ImgSource = "https://i.imgur.com/dlDuKhf.jpgg", Title = AppResources.religion_feedPage, IsSelected = false },
                new Discovery() { ImgSource = "https://i.imgur.com/sySiZVa.jpg", Title = AppResources.liveStrong_feedPage, IsSelected = false },
            };
        }

        private void SetFlagDiscoverySelected(bool isFood = false, bool isHealthy = false, bool liveStrong = false)
        {
            this.isFoodFeed = isFood;
            this.isHealthyFeed = isHealthy;
            this.isLiveStrongFeed = liveStrong;
        }
    }
}
