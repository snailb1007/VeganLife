using VeganLife.Helpers.AppSetting;
using VeganLife.Resources.Translations;

namespace VeganLife.ViewModels
{
    public partial class NewsFeedViewModel : BaseViewModel
    {
        IEnumerable<Item> _dataFood = new List<Item>();
        IEnumerable<Item> _dataHealthy = new List<Item>();
        IEnumerable<Item> _dataReligion = new List<Item>();
        IEnumerable<Item> _dataLiveStrong = new List<Item>();

        bool _isFoodFeed = true;
        bool _isHealthyFeed;
        bool _isLiveStrongFeed;

        byte _currentNumberItem;

        [ObservableProperty]
        ObservableCollection<Item> _feeds;

        [ObservableProperty]
        ObservableCollection<Discovery> _discoveryMenu;

        [RelayCommand]
        void RefreshFoods()
        {
            if (IsLoading)
                return;
            Initialize();
        }

        [RelayCommand]
        async Task SelectDiscoveryMenu(object obj)
        {
            if (IsLoading)
                return;
            IsLoading = true;
            var currentItem = (Discovery)obj;
            if (currentItem == null || DiscoveryMenu?.Where(i => i.IsSelected)?.FirstOrDefault() == currentItem)
            {
                IsLoading = false;
                return;
            }

            _currentNumberItem = 20;
            Feeds?.Clear();
            if (currentItem.Title.Equals(AppResources.veganFood_feedPage))
            {
                SetFlagDiscoverySelected(isFood: true);
                if (!_dataFood?.Any() ?? true)
                    _dataFood = await data_service.LoadGoogleNews(ConstantHelper.RssFeedNews.Google_News_VeganFoods);
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    foreach (var item in _dataFood.Take(_currentNumberItem))
                    {
                        Feeds.Add(item);
                    }
                }
                );
            }
            else if (currentItem.Title.Equals(AppResources.healthy_feedPage))
            {
                SetFlagDiscoverySelected(isHealthy: true);
                if (!_dataHealthy?.Any() ?? true)
                    _dataHealthy = await data_service.LoadGoogleNews(ConstantHelper.RssFeedNews.Google_News_VeganHealthy);
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    foreach (var item in _dataHealthy.Take(_currentNumberItem))
                    {
                        Feeds.Add(item);
                    }
                });
            }
            else if (currentItem.Title.Equals(AppResources.religion_feedPage))
            {
                SetFlagDiscoverySelected();
                if (!_dataReligion?.Any() ?? true)
                    _dataReligion = await data_service.LoadGoogleNews(ConstantHelper.RssFeedNews.Google_News_Religion);
                MainThread.BeginInvokeOnMainThread(() => Feeds = new ObservableCollection<Item>(_dataReligion));
            }
            else if (currentItem.Title.Equals(AppResources.liveStrong_feedPage))
            {
                SetFlagDiscoverySelected(liveStrong: true);
                if (!_dataLiveStrong?.Any() ?? true)
                    _dataLiveStrong = await data_service.LoadGoogleNews(ConstantHelper.RssFeedNews.Google_News_LiveStrong);
                MainThread.BeginInvokeOnMainThread(() => Feeds = new ObservableCollection<Item>(_dataLiveStrong));
            }

            foreach (var item in DiscoveryMenu)
            {
                item.IsSelected = false;
            }

            currentItem.IsSelected = true;
            IsLoading = false;
        }

        TaskCompletionSource<bool> _taskLoadingMessage;
        [RelayCommand]
        async void LoadMoreItem()
        {
            if (_taskLoadingMessage != null && !_taskLoadingMessage.Task.IsCompleted)
                await _taskLoadingMessage.Task;
            _taskLoadingMessage = new TaskCompletionSource<bool>();
            IEnumerable<Item> listTemp;
            if (_isFoodFeed)
                listTemp = _dataFood;
            else if (_isHealthyFeed)
                listTemp = _dataHealthy;
            else
                listTemp = _isLiveStrongFeed ? _dataLiveStrong : _dataReligion;
            bool isLoadedAllData = Feeds?.Count > 0 && Feeds?.Count == listTemp?.Count();
            if (!isLoadedAllData)
            {
                for (int i = 0; i < 10 && (i + _currentNumberItem) < listTemp?.Count(); i++)
                {
                    Feeds.Add(listTemp.ElementAt(i + _currentNumberItem));
                }

                _currentNumberItem += 10;
            }

            _taskLoadingMessage.TrySetResult(true);
        }

        [RelayCommand]
        async Task SelectFeedItem(object obj)
        {
            if (IsLoading)
                return;
            IsLoading = true;
            var item = obj as Item;
            try
            {
                if (item == null)
                    return;
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
                IsLoading = false;
            }
        }

        public NewsFeedViewModel(INavigationService navigationService, IDataService dataService) : base(navigationService, dataService)
        {
            Initialize();
        }

        private async void Initialize()
        {
            IsLoading = true;
            if (Feeds != null && Feeds.Any())
                Feeds.Clear();
            if (DiscoveryMenu != null && DiscoveryMenu.Any())
                DiscoveryMenu.Clear();
            _dataFood = await data_service.LoadGoogleNews(ConstantHelper.RssFeedNews.Google_News_VeganFoods);
            InitMenu();
            _currentNumberItem = 20;
            Feeds = new ObservableCollection<Item>(_dataFood.Take(_currentNumberItem));
            IsLoading = false;
        }

        //void SetHotFeeds(List<Item> data, HtmlWeb htmlWeb, string topic)
        //{
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

        //    HotFeeds.Add(itemHotFeeds);
        //}

        private void InitMenu()
        {
            DiscoveryMenu = new ObservableCollection<Discovery>()
            {
                new Discovery(){ ImgSource = "https://i.imgur.com/anDoRUb.jpg", Title= AppResources.veganFood_feedPage, IsSelected = true},
                new Discovery(){ ImgSource = "https://i.imgur.com/tH9PSUe.jpg", Title= AppResources.healthy_feedPage, IsSelected = false},
                new Discovery(){ ImgSource = "https://i.imgur.com/dlDuKhf.jpgg", Title= AppResources.religion_feedPage, IsSelected = false},
                new Discovery(){ ImgSource = "https://i.imgur.com/sySiZVa.jpg", Title= AppResources.liveStrong_feedPage, IsSelected = false}
            };
        }

        //        string nodeImgHead = "//meta[@property='og:image']";
        //        string nodeWeb = "//a";
        //        string LoadUrlPreview(HtmlWeb htmlWeb, string url)
        //        {

        //            HtmlDocument htmlDoc = new HtmlDocument();
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
        //#if DEBUG
        //                    Console.WriteLine(result);
        //#endif
        //                }
        //            }
        //            catch (Exception e)
        //            {
        //                Console.WriteLine(e.StackTrace);
        //            }

        //            return result;
        //        }

        void SetFlagDiscoverySelected(bool isFood = false, bool isHealthy = false, bool liveStrong = false)
        {
            _isFoodFeed = isFood;
            _isHealthyFeed = isHealthy;
            _isLiveStrongFeed = liveStrong;
        }
    }

    public partial class Discovery : MenuModel
    {
        [ObservableProperty]
        bool _isSelected;
    }
}
