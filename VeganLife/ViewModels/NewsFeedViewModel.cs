using HtmlAgilityPack;
using Newtonsoft.Json;
using System.Text;
using System.Xml;
using VeganLife.Data.RssFeedsData;
using VeganLife.Helpers.AppSetting;
using VeganLife.Resources.Translations;

namespace VeganLife.ViewModels
{
    public partial class NewsFeedViewModel : BaseViewModel
    {
        DataLoader _databaseLoader;
        List<Item> _data = new List<Item>();

        bool _isFoodFeed = true;
        bool _isHealthyFeed;
        bool _isLiveStrongFeed;

        byte _currentNumberItem;

        [ObservableProperty]
        bool _isDisplayHotFeed;

        [ObservableProperty]
        ObservableCollection<HotItemModel> _hotFeeds;

        [ObservableProperty]
        ObservableCollection<Item> _feeds;

        [ObservableProperty]
        ObservableCollection<Discovery> _discoveryMenu;

        [RelayCommand]
        void ChangeDisplayStatusHotFeed()
        {
            IsDisplayHotFeed = !IsDisplayHotFeed;
        }

        [RelayCommand]
        void RefreshFoods()
        {
            if (IsLoading)
                return;
            Initialize();
        }

        [RelayCommand]
        async void SelectDiscoveryMenu(object obj)
        {
            if (IsLoading)
                return;
            var itemMenu = (Discovery)obj;
            IsLoading = true;
            if (itemMenu == null || DiscoveryMenu?.Where(i => i.IsSelected)?.FirstOrDefault() == itemMenu)
            {
                IsLoading = false;
                return;
            }

            _currentNumberItem = 20;
            if (Feeds != null)
            {
                Feeds.Clear();
                Feeds = new ObservableCollection<Item>();
            }

            if (itemMenu.Title.Equals(AppResources.veganFood_feedPage))
            {
                SetFlagDiscoverySelected(isFood: true);
                _data = _databaseLoader.LoadData(ConstantHelper.RssFeedNews.Google_News_VeganFoods);
            }
            else if (itemMenu.Title.Equals(AppResources.healthy_feedPage))
            {
                SetFlagDiscoverySelected(isHealthy: true);
                _data = _databaseLoader.LoadData(ConstantHelper.RssFeedNews.Google_News_VeganHealthy);
            }
            else if (itemMenu.Title.Equals(AppResources.religion_feedPage))
            {
                SetFlagDiscoverySelected();
                _data = _databaseLoader.LoadData(ConstantHelper.RssFeedNews.Google_News_Religion);
            }
            else if (itemMenu.Title.Equals(AppResources.liveStrong_feedPage))
            {
                SetFlagDiscoverySelected(liveStrong: true);
                _data = _databaseLoader.LoadData(ConstantHelper.RssFeedNews.Google_News_LiveStrong);
            }

            foreach (var item in DiscoveryMenu)
            {
                item.IsSelected = false;
            }

            itemMenu.IsSelected = true;
            IsLoading = false;
        }

        TaskCompletionSource<bool> _taskLoadingMessage;
        //[RelayCommand]
        //async void LoadMoreItem()
        //{
        //    if (_taskLoadingMessage != null && !_taskLoadingMessage.Task.IsCompleted)
        //        await _taskLoadingMessage.Task;
        //    _taskLoadingMessage = new TaskCompletionSource<bool>();
        //    IList<Item> listTemp;
        //    if (_isFoodFeed)
        //        listTemp = _data.GetValueOrDefault(ConstantHelper.RssFeedNews.Google_News_VeganFoods);
        //    else if (_isHealthyFeed)
        //        listTemp = _data.GetValueOrDefault(ConstantHelper.RssFeedNews.Google_News_VeganHealthy);
        //    else
        //        listTemp = _isLiveStrongFeed ? _data.GetValueOrDefault(ConstantHelper.RssFeedNews.Google_News_LiveStrong)
        //            : _data.GetValueOrDefault(ConstantHelper.RssFeedNews.Google_News_Religion);
        //    bool isLoadedAllData = Feeds?.Count > 0 && Feeds?.Count == listTemp?.Count;
        //    if (!isLoadedAllData)
        //    {
        //        for (int i = 0; i < 10 && (i + _currentNumberItem) < listTemp?.Count; i++)
        //        {
        //            Feeds.Add(listTemp[i + _currentNumberItem]);
        //        }

        //        _currentNumberItem += 10;
        //    }

        //    _taskLoadingMessage.TrySetResult(true);
        //}

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
            _databaseLoader = new DataLoader();
            _databaseLoader.DataLoaded += _database_DataLoaded;
            Initialize();
        }

        private void _database_DataLoaded(object sender, EventArgs e)
        {
            if (sender == null)
                return;
            DisplayFeeds();
            IsLoading = false;
        }

        private void Initialize()
        {
            IsLoading = true;
            IsDisplayHotFeed = true;
            if (Feeds != null && Feeds.Any())
                Feeds.Clear();
            if (DiscoveryMenu != null && DiscoveryMenu.Any())
                DiscoveryMenu.Clear();
            _data = _databaseLoader.LoadData(ConstantHelper.RssFeedNews.Google_News_VeganFoods);
            InitMenu();
        }

        void DisplayFeeds()
        {
            if (Feeds == null)
            {
                Feeds = new ObservableCollection<Item>();
            }

            _data.ForEach(i => Feeds.Add(i));
            _databaseLoader._isLoaded = false;
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

    class DataLoader
    {
        List<Item> _data = new List<Item>();

        public bool _isLoaded;

        public event EventHandler DataLoaded;

        public List<Item> LoadData(string uri)
        {
            EnsureLoad(uri);

            return _data;
        }

        async void EnsureLoad(string uri)
        {
            lock (this)
            {
                if (_isLoaded)
                    return;
                _isLoaded = true;
            }

            // actual loading
            await LoadGoogleNews(uri);
            // invoke to new feed
            DataLoaded?.Invoke(uri, EventArgs.Empty);
        }

        async Task LoadGoogleNews(string uri)
        {
            var rss = new RssFeedsHttpRequest();
            var data = await rss.GetRssData(uri);
            if (string.IsNullOrEmpty(data)) return;
            var doc = new XmlDocument();
            doc.LoadXml(data);
            var json = JsonConvert.SerializeXmlNode(doc.DocumentElement);
            if (string.IsNullOrEmpty(json))
                return;
            var baseData = JsonConvert.DeserializeObject<GoogleNewsModel>(json);
            _data = baseData.rss.channel.item;
        }
    }

    public partial class Discovery : MenuModel
    {
        [ObservableProperty]
        bool _isSelected;
    }
}
