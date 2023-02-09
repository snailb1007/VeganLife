using Android.Hardware.Usb;
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
        DataLoader _databaseFood;

        private Dictionary<string, List<Item>> _data = new Dictionary<string, List<Item>>();

        private List<Item> _allVeganFoodFeeds;
        private List<Item> _allVeganHealthyFeeds;
        private List<Item> _allReligionFeeds;
        private List<Item> _allLiveStrongFeeds;

        bool _isFoodFeed = true;
        bool _isHealthyFeed;
        bool _isLiveStrongFeed;

        byte _currentNumberItem;

        [ObservableProperty]
        bool _isDisplayHotFeed;

        [ObservableProperty]
        ObservableCollection<HotItem> _hotFeeds;

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
        void SelectDiscoveryMenu(object obj)
        {
            var watch = new Stopwatch();
            watch.Start();
            if (IsLoading)
                return;
            IsLoading = true;
            var itemSelected = obj as Discovery;
            if (itemSelected == null || DiscoveryMenu?.Where(i => i.IsSelected)?.FirstOrDefault() == itemSelected)
            {
                IsLoading = false;
                return;
            }

            _currentNumberItem = 20;
            Feeds.Clear();
            if (itemSelected.Title.Equals(AppResources.veganFood_feedPage))
            {
                SetFlagDiscoverySelected(isFood: true);
                foreach (var item in _allVeganFoodFeeds.Take(_currentNumberItem))
                    Feeds.Add(item);
            }
            else if (itemSelected.Title.Equals(AppResources.healthy_feedPage))
            {
                SetFlagDiscoverySelected(isHealthy: true);
                foreach (var item in _allVeganHealthyFeeds.Take(_currentNumberItem))
                    Feeds.Add(item);
            }
            else if (itemSelected.Title.Equals(AppResources.religion_feedPage))
            {
                SetFlagDiscoverySelected();
                foreach (var item in _allReligionFeeds.Take(_currentNumberItem))
                    Feeds.Add(item);
            }
            else if (itemSelected.Title.Equals(AppResources.liveStrong_feedPage))
            {
                SetFlagDiscoverySelected(liveStrong: true);
                foreach (var item in _allLiveStrongFeeds.Take(_currentNumberItem))
                    Feeds.Add(item);
            }

            foreach (var item in DiscoveryMenu)
            {
                item.IsSelected = false;
            }

            itemSelected.IsSelected = true;
            IsLoading = false;
            watch.Stop();
            Console.WriteLine("thien==> " + watch.ElapsedMilliseconds);
        }

        TaskCompletionSource<bool> _taskLoadingMessage;
        [RelayCommand]
        async void LoadMoreItem()
        {
            if (_taskLoadingMessage != null && !_taskLoadingMessage.Task.IsCompleted)
                await _taskLoadingMessage.Task;
            _taskLoadingMessage = new TaskCompletionSource<bool>();
            List<Item> listTemp;
            if (_isFoodFeed)
                listTemp = _allVeganFoodFeeds;
            else if (_isHealthyFeed)
                listTemp = _allVeganHealthyFeeds;
            else
                listTemp = _isLiveStrongFeed ? _allLiveStrongFeeds : _allReligionFeeds;
            bool isLoadedAllData = Feeds?.Count > 0 && Feeds?.Count == listTemp?.Count;
            if (!isLoadedAllData)
            {
                for (int i = 0; i < 10 && (i + _currentNumberItem) < listTemp?.Count; i++)
                {
                    Feeds.Add(listTemp[i + _currentNumberItem]);
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
                throw;
            }
            finally
            {
                IsLoading = false;
            }
        }

        public NewsFeedViewModel()
        {
            _databaseFood = new DataLoader();
            _databaseFood.DataLoaded += _database_DataLoaded;
            Initialize();
        }

        private void _database_DataLoaded(object sender, EventArgs e)
        {
            if (sender == null)
                return;
            foreach (var item in (string[])sender)
            {
                if (!string.IsNullOrEmpty(item) && _data.ContainsKey(item))
                {
                    if (item.Equals(ConstantHelper.RssFeedNews.Google_News_VeganFoods))
                    {
                        _allVeganFoodFeeds = _data.GetValueOrDefault(item);
                    }
                    else if (item.Equals(ConstantHelper.RssFeedNews.Google_News_VeganHealthy))
                    {
                        _allVeganHealthyFeeds = _data.GetValueOrDefault(item);
                    }
                    else if (item.Equals(ConstantHelper.RssFeedNews.Google_News_Religion))
                    {
                        _allReligionFeeds = _data.GetValueOrDefault(item);
                    }
                    else if (item.Equals(ConstantHelper.RssFeedNews.Google_News_LiveStrong))
                    {
                        _allLiveStrongFeeds = _data.GetValueOrDefault(item);
                    }

                    DisplayFeeds(false, item);
                }
            }

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
            _data = _databaseFood.LoadData(new string[]
            {
                ConstantHelper.RssFeedNews.Google_News_VeganFoods,
                ConstantHelper.RssFeedNews.Google_News_VeganHealthy,
                ConstantHelper.RssFeedNews.Google_News_Religion,
                ConstantHelper.RssFeedNews.Google_News_LiveStrong
            });
            InitMenu();
        }

        void DisplayFeeds(bool isLoadMore, string uri)
        {
            if (isLoadMore)
            {

            }
            else
            {
                if (Feeds == null)
                {
                    Feeds = new ObservableCollection<Item>();
                }
                if (HotFeeds == null)
                {
                    HotFeeds = new ObservableCollection<HotItem>();
                }

                HtmlWeb htmlWeb = new HtmlWeb() { AutoDetectEncoding = false, OverrideEncoding = Encoding.UTF8 };

                switch (uri)
                {
                    case ConstantHelper.RssFeedNews.Google_News_VeganFoods:
                        _allVeganFoodFeeds.ForEach(i => Feeds.Add(i));
                        SetHotFeeds(_allVeganFoodFeeds, htmlWeb, AppResources.veganFood_feedPage);
                        break;
                    case ConstantHelper.RssFeedNews.Google_News_VeganHealthy:
                        SetHotFeeds(_allVeganHealthyFeeds, htmlWeb, AppResources.healthy_feedPage);
                        break;

                    case ConstantHelper.RssFeedNews.Google_News_Religion:
                        SetHotFeeds(_allReligionFeeds, htmlWeb, AppResources.religion_feedPage);
                        break;

                    case ConstantHelper.RssFeedNews.Google_News_LiveStrong:
                        SetHotFeeds(_allLiveStrongFeeds, htmlWeb, AppResources.liveStrong_feedPage);
                        break;
                }
            }
        }

        void SetHotFeeds(List<Item> data, HtmlWeb htmlWeb, string topic)
        {
            HotItem itemHotFeeds = new HotItem();
            string imgLinkHotItem = string.Empty;
            foreach (var item in data)
            {
                if (item.source.url == "https://vtc.vn")
                    continue;
                imgLinkHotItem = LoadUrlPreview(htmlWeb, item?.link);
                if (!string.IsNullOrEmpty(imgLinkHotItem))
                {
                    item.ImageTitleUri = imgLinkHotItem;
                    itemHotFeeds = new HotItem(item, topic);
                    break;
                }
            }

            HotFeeds.Add(itemHotFeeds);
        }

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

        string nodeImgHead = "//meta[@property='og:image']";
        string LoadUrlPreview(HtmlWeb htmlWeb, string url)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            string result = htmlDoc.ParsedText ?? string.Empty;
            try
            {
                htmlDoc = htmlWeb.Load(url);
                // get image title
                var titleImageNode = htmlDoc.DocumentNode.SelectSingleNode(nodeImgHead);
                if (titleImageNode != null)
                {
                    result = titleImageNode?.Attributes["content"]?.Value ?? string.Empty;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }

            return result;
        }

        void SetFlagDiscoverySelected(bool isFood = false, bool isHealthy = false, bool liveStrong = false)
        {
            _isFoodFeed = isFood;
            _isHealthyFeed = isHealthy;
            _isLiveStrongFeed= liveStrong;
        }
    }

    class DataLoader
    {
        public bool _isLoaded;
        private Dictionary<string, List<Item>> _data = new Dictionary<string, List<Item>>();

        public event EventHandler DataLoaded;

        public Dictionary<string, List<Item>> LoadData(string[] uri)
        {
            EnsureLoad(uri);
            return _data;
        }

        async void EnsureLoad(string[] uri)
        {
            lock (this)
            {
                if (_isLoaded)
                    return;
                _isLoaded = true;
            }

            //List<Task> tasks = new List<Task>();
            // actual loading
            foreach (var item in uri)
            {
                await LoadGoogleNews(item);
            }

            //await Task.WhenAll(tasks);
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
            var feeds = baseData.rss.channel.item;
            _data.Add(uri, feeds);
        }
    }

    public partial class Discovery : ObservableObject
    {
        public string ImgSource { get; set; }
        public string Title { get; set; }
        [ObservableProperty]
        bool _isSelected;
        public string Opacity => IsSelected ? "1" : "0.5";
    }

    public class HotItem : Item
    {
        public HotItem() { }
        public HotItem(Item item, string topic)
        {
            title = item.title;
            link = item.link;
            guid = item.guid;
            pubDate = item.pubDate;
            description = item.description;
            source = item.source;
            ImageTitleUri = item.ImageTitleUri;
            Topic = topic;
        }

        public string Topic { get; set; }
    }
}
