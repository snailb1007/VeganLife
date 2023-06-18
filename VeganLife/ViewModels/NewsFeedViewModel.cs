// <copyright file="NewsFeedViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.ViewModels
{
    using VeganLife.Helpers.AppSetting;
    using VeganLife.Resources.Translations;

    public partial class NewsFeedViewModel : BaseViewModel
    {
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

        [RelayCommand]
        private void RefreshFoods()
        {
            if (this.IsLoading)
            {
                return;
            }

            this.Initialize();
        }

        [RelayCommand]
        private async Task SelectDiscoveryMenu(object obj)
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
                    this.dataFood = await this.dataService.LoadGoogleNews(ConstantHelper.RssFeedNews.GoogleNewsVeganFoods);
                }

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    foreach (var item in this.dataFood.Take(this.currentNumberItem))
                    {
                        this.Feeds.Add(item);
                    }
                });
            }
            else if (currentItem.Title.Equals(AppResources.healthy_feedPage))
            {
                this.SetFlagDiscoverySelected(isHealthy: true);
                if (!this.dataHealthy?.Any() ?? true)
                {
                    this.dataHealthy = await this.dataService.LoadGoogleNews(ConstantHelper.RssFeedNews.GoogleNewsVeganHealthy);
                }

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    foreach (var item in this.dataHealthy.Take(this.currentNumberItem))
                    {
                        this.Feeds.Add(item);
                    }
                });
            }
            else if (currentItem.Title.Equals(AppResources.religion_feedPage))
            {
                this.SetFlagDiscoverySelected();
                if (!this.dataReligion?.Any() ?? true)
                    {
                    this.dataReligion = await this.dataService.LoadGoogleNews(ConstantHelper.RssFeedNews.GoogleNewsReligion);
                }

                MainThread.BeginInvokeOnMainThread(() => this.Feeds = new ObservableCollection<Item>(this.dataReligion));
            }
            else if (currentItem.Title.Equals(AppResources.liveStrong_feedPage))
            {
                this.SetFlagDiscoverySelected(liveStrong: true);
                if (!this.dataLiveStrong?.Any() ?? true)
                    {
                    this.dataLiveStrong = await this.dataService.LoadGoogleNews(ConstantHelper.RssFeedNews.GoogleNewsLiveStrong);
                }

                MainThread.BeginInvokeOnMainThread(() => this.Feeds = new ObservableCollection<Item>(this.dataLiveStrong));
            }

            foreach (var item in this.DiscoveryMenu)
            {
                item.IsSelected = false;
            }

            currentItem.IsSelected = true;
            this.IsLoading = false;
        }

        private TaskCompletionSource<bool> taskLoadingMessage;

        [RelayCommand]
        private async void LoadMoreItem()
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
                    this.Feeds.Add(listTemp.ElementAt(i + this.currentNumberItem));
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
                // An unexpected error occured. No browser may be installed on the device.
                await Console.Out.WriteLineAsync("No browser may be installed on the device\n" + ex.Message);
                throw;
            }
            finally
            {
                this.IsLoading = false;
            }
        }

        public NewsFeedViewModel(INavigationService navigationService, IDataService dataService) : base(navigationService, dataService)
        {
            this.Initialize();
        }

        private async void Initialize()
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

            this.dataFood = await this.dataService.LoadGoogleNews(ConstantHelper.RssFeedNews.GoogleNewsVeganFoods);
            this.InitMenu();
            this.currentNumberItem = 20;
            this.Feeds = new ObservableCollection<Item>(this.dataFood.Take(this.currentNumberItem));
            this.IsLoading = false;
        }

        // void SetHotFeeds(List<Item> data, HtmlWeb htmlWeb, string topic)
        // {
        //    HotItemModel itemHotFeeds = new HotItemModel();
        //    string imgLinkHotItem = string.Empty;
        //    foreach (var item in data)
        //    {
        //        if (item.source.url == "https://vtc.vn")
        //            continue;
        //        imgLinkHotItem = LoadUrlPreview(htmlWeb, item?.link);
        //        if (!string.IsNullOrEmpty(imgLinkHotItem))
        //        {
        //            item.ImageTitleUri = imgLinkHotItem;
        //            itemHotFeeds = new HotItemModel(item, topic);
        //            break;
        //        }
        //    }

        // HotFeeds.Add(itemHotFeeds);
        // }
        private void InitMenu()
        {
            this.DiscoveryMenu = new ObservableCollection<Discovery>()
            {
                new Discovery() { ImgSource = "https://i.imgur.com/anDoRUb.jpg", Title= AppResources.veganFood_feedPage, IsSelected = true},
                new Discovery() { ImgSource = "https://i.imgur.com/tH9PSUe.jpg", Title= AppResources.healthy_feedPage, IsSelected = false},
                new Discovery() { ImgSource = "https://i.imgur.com/dlDuKhf.jpgg", Title= AppResources.religion_feedPage, IsSelected = false},
                new Discovery() { ImgSource = "https://i.imgur.com/sySiZVa.jpg", Title= AppResources.liveStrong_feedPage, IsSelected = false},
            };
        }

        // string nodeImgHead = "//meta[@property='og:image']";
        //        string nodeWeb = "//a";
        //        string LoadUrlPreview(HtmlWeb htmlWeb, string url)
        //        {

        // HtmlDocument htmlDoc = new HtmlDocument();
        //            string result = htmlDoc.ParsedText ?? string.Empty;
        //            try
        //            {
        //                htmlDoc = htmlWeb.Load(url.Remove(url.IndexOf("?")));
        //                //get web
        //                var webNode = htmlDoc.DocumentNode.SelectSingleNode(nodeWeb);
        //                if (webNode != null)
        //                {
        //                    result = webNode?.Attributes["href"]?.Value ?? string.Empty;
        //                }
        //                // get image title
        //                if (string.IsNullOrEmpty(result))
        //                    return result;
        //                htmlDoc = htmlWeb.Load(result);
        //                var titleImageNode = htmlDoc.DocumentNode.SelectSingleNode(nodeImgHead);
        //                if (titleImageNode != null)
        //                {
        //                    result = titleImageNode?.Attributes["Content"]?.Value ?? string.Empty;
        // #if DEBUG
        //                    Console.WriteLine(result);
        // #endif
        //                }
        //            }
        //            catch (Exception e)
        //            {
        //                Console.WriteLine(e.StackTrace);
        //            }

        // return result;
        //        }
        private void SetFlagDiscoverySelected(bool isFood = false, bool isHealthy = false, bool liveStrong = false)
        {
            this.isFoodFeed = isFood;
            this.isHealthyFeed = isHealthy;
            this.isLiveStrongFeed = liveStrong;
        }
    }

    public partial class Discovery : MenuModel
    {
        [ObservableProperty]
        private bool isSelected;
    }
}
