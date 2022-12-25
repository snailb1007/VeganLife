using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Text;
using System.Xml;
using VeganLife.Data.RssFeedsData;
using VeganLife.Helpers.AppSetting;
using VeganLife.Models;
using Item = VeganLife.Models.Item;

namespace VeganLife.ViewModels
{
    public partial class NewsFeedViewModel : ObservableObject
    {
        DataLoader _databaseFood;
        byte _currentNumberOfItem = 0;

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
            if (itemSelected != null)
            {
                DiscoveryMenu.ForEach(i => i.IsSelected = false);
                itemSelected.IsSelected = true;
            }

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

        private async void _database_DataLoaded(object sender, EventArgs e)
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

                    await DisplayFeeds(false, item);
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

        TaskCompletionSource<bool> _taskLoadingFeeds;
        async Task DisplayFeeds(bool isLoadMore, string uri)
        {
            //if (_taskLoadingFeeds != null && !_taskLoadingFeeds.Task.IsCompleted)
            //    await _taskLoadingFeeds.Task;
            //_taskLoadingFeeds = new TaskCompletionSource<bool>();
            if (isLoadMore)
            {

            }
            else
            {
                _currentNumberOfItem = 20;
                Feeds = new List<Item>();
                if (HotFeeds == null)
                {
                    HotFeeds = new List<HotItem>();
                }

                HtmlWeb htmlWeb = new HtmlWeb() { AutoDetectEncoding = false, OverrideEncoding = Encoding.UTF8 };

                switch(uri)
                {
                    case ConstantHelper.RssFeedNews.Google_News_VeganFoods:
                        byte loop = 0;
                        foreach (var item in _allVeganFoodFeeds.Take(10))
                        {
                            item.ImageTitleUri = LoadUrlPreview(htmlWeb, item.link);
                            Feeds.Add(item);
                            if (loop == 0)
                            {
                                HotFeeds.Add(new HotItem(item, topic: "food"));
                            }

                            loop++;
                        }
                        break;
                    case ConstantHelper.RssFeedNews.Google_News_VeganHealthy:
                        var itemHealthy = _allVeganHealthyFeeds.FirstOrDefault();
                        itemHealthy.ImageTitleUri = LoadUrlPreview(htmlWeb, itemHealthy.link);
                        HotFeeds.Add(new HotItem(itemHealthy, topic: "healthy"));
                        break;

                    case ConstantHelper.RssFeedNews.Google_News_Religion:
                        var itemReligion = _allReligionFeeds.FirstOrDefault();
                        itemReligion.ImageTitleUri = LoadUrlPreview(htmlWeb, itemReligion.link);
                        HotFeeds.Add(new HotItem(itemReligion, topic: "religion"));
                        break;

                    case ConstantHelper.RssFeedNews.Google_News_LiveStrong:
                        var itemLiveStrong = _allLiveStrongFeeds.FirstOrDefault();
                        itemLiveStrong.ImageTitleUri = LoadUrlPreview(htmlWeb, itemLiveStrong.link);
                        HotFeeds.Add(new HotItem(itemLiveStrong, topic: "strong"));
                        break;
                }
            }
        }

        private void InitMenu()
        {
            DiscoveryMenu = new List<Discovery>()
            {
                new Discovery(){ ImgSource =  "https://i.imgur.com/anDoRUb.jpg", Title= "Món chay"},
                new Discovery(){ ImgSource =  "https://i.imgur.com/tH9PSUe.jpg", Title= "Sức khỏe"},
                new Discovery(){ ImgSource =  "https://i.imgur.com/dlDuKhf.jpgg", Title= "Tín ngưỡng"},
                new Discovery(){ ImgSource =  "https://i.imgur.com/sySiZVa.jpg", Title= "Sống khỏe"}
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
            foreach(var item in uri)
            {
                await LoadGoogleNews(item);
            }

            //await Task.WhenAll(tasks);
            DataLoaded?.Invoke(uri, EventArgs.Empty);
        }

        TaskCompletionSource<bool> _taskCompletion;
        async Task LoadGoogleNews(string uri)
        {
            //if (_taskCompletion != null && !_taskCompletion.Task.IsCompleted)
            //    await _taskCompletion.Task;
            //_taskCompletion = new TaskCompletionSource<bool>();
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
            //_taskCompletion.TrySetResult(true);
            //_taskCompletion = null;
        }
    }

    public partial class Discovery : ObservableObject
    {
        public string ImgSource { get; set; }
        public string Title { get; set; }

        [ObservableProperty]
        bool _isSelected;
    }

    public class HotItem : Item
    {
        public HotItem(Item item, string topic)
        {
            title = item.title;
            link= item.link;
            guid= item.guid;
            pubDate= item.pubDate;
            description= item.description;
            source= item.source;
            ImageTitleUri = item.ImageTitleUri;
            Topic = topic;
        }

        public string Topic { get; set; }
    }
}
