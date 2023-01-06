using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System.Text;
using System.Xml;
using VeganLife.Data.RssFeedsData;
using VeganLife.Helpers.AppSetting;
using VeganLife.Models;
using VeganLife.Resources.Translations;

namespace VeganLife.ViewModels
{
    public partial class NewsFeedViewModel : ObservableObject
    {
        DataLoader _databaseFood;
        // byte _currentNumberOfItem = 0;

        private Dictionary<string, List<Item>> _data = new Dictionary<string, List<Item>>();

        private List<Item> _allVeganFoodFeeds;
        private List<Item> _allVeganHealthyFeeds;
        private List<Item> _allReligionFeeds;
        private List<Item> _allLiveStrongFeeds;

        [ObservableProperty]
        bool _isLoading;

        [ObservableProperty]
        List<HotItem> _hotFeeds;
        [ObservableProperty]
        List<Item> _feeds;

        [ObservableProperty]
        List<Discovery> _discoveryMenu;

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
            if (IsLoading)
                return;
            IsLoading = true;
            var itemSelected = obj as Discovery;
            if (itemSelected == null || DiscoveryMenu?.Where(i => i.IsSelected)?.FirstOrDefault() == itemSelected)
            {
                IsLoading = false;
                return;
            }

            if (itemSelected.Title.Equals(AppResources.veganFood_feedPage))
            {
                Feeds.Clear();
                _allVeganFoodFeeds.ForEach(i => Feeds.Add(i));
            }
            else if (itemSelected.Title.Equals(AppResources.healthy_feedPage))
            {
                Feeds = _allVeganHealthyFeeds;
            }
            else if (itemSelected.Title.Equals(AppResources.religion_feedPage))
            {
                Feeds = _allReligionFeeds;
            }
            else if (itemSelected.Title.Equals(AppResources.liveStrong_feedPage))
            {
                Feeds = _allLiveStrongFeeds;
            }

            foreach (var item in DiscoveryMenu)
            {
                item.IsSelected = false;
            }

            itemSelected.IsSelected = true;
            IsLoading = false;
        }

        bool _isLoadingMoreItem = false;
        [RelayCommand]
        void LoadMoreItem()
        {
            if (_isLoadingMoreItem) return;
            _isLoadingMoreItem = true;

            _isLoadingMoreItem = false;
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

        partial void OnDiscoveryMenuChanged(List<Discovery> value)
        {
        }

        // TaskCompletionSource<bool> _taskLoadingFeeds;
        void DisplayFeeds(bool isLoadMore, string uri)
        {
            if (isLoadMore)
            {

            }
            else
            {
                if (Feeds == null)
                {
                    Feeds = new List<Item>();
                }
                if (HotFeeds == null)
                {
                    HotFeeds = new List<HotItem>();
                }

                HtmlWeb htmlWeb = new HtmlWeb() { AutoDetectEncoding = false, OverrideEncoding = Encoding.UTF8 };
                HotItem itemHotFeeds = new HotItem();
                string imgLinkHotItem = string.Empty;

                switch (uri)
                {
                    case ConstantHelper.RssFeedNews.Google_News_VeganFoods:
                        Feeds.AddRange(_allVeganFoodFeeds);
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
            DiscoveryMenu = new List<Discovery>()
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
                // Console.WriteLine(e.Message); ;
            }

            return result;
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
        public bool IsSelected { get; set; } = false;
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
