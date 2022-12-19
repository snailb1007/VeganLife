using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Net;
using System.Text;
using System.Xml;
using VeganLife.Data.RssFeedsData;
using VeganLife.Helpers;
using VeganLife.Helpers.AppSetting;
using VeganLife.Models;
using HtmlAgilityPack;
using System.Web;

namespace VeganLife.ViewModels
{
    public partial class NewsFeedViewModel : ObservableObject
    {
        Database _database;
        ObservableCollection<Item> _allVeganFoodFeeds;

        [ObservableProperty]
        bool _isLoading;

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
            _database = new Database();
            Initialize();
        }

        private void Initialize()
        {
            IsLoading = true;
            if (Feeds != null && Feeds.Any())
                Feeds.Clear();
            if (DiscoveryMenu != null && DiscoveryMenu.Any())
                DiscoveryMenu.Clear();
            _allVeganFoodFeeds = _database.LoadData();
            InitMenu();
            _ = DisplayFeeds(false);
            IsLoading= false;
        }

        partial void OnDiscoveryMenuChanged(List<Discovery> value)
        {
        }

        byte _currentNumberOfItem = 0;
        TaskCompletionSource<bool> _taskLoadingFeeds;
        async Task DisplayFeeds(bool isLoadMore)
        {
            if (_taskLoadingFeeds != null && !_taskLoadingFeeds.Task.IsCompleted)
                await _taskLoadingFeeds.Task;
            _taskLoadingFeeds = new TaskCompletionSource<bool>();
            if (isLoadMore)
            {

            }
            else
            {
                _currentNumberOfItem = 20;
                Feeds = new List<Item>();
                foreach (var item in _allVeganFoodFeeds.Take(20))
                {
                    if (string.IsNullOrEmpty(LoadUrlPreview(item.link)))
                        item.CanOpenInApp = false;
                    else
                        item.CanOpenInApp = true;
                    Feeds.Add(item);
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

        string LoadUrlPreview(string url)
        {
            HtmlWeb htmlWeb = new HtmlWeb() { AutoDetectEncoding = false, OverrideEncoding = Encoding.UTF8 };
            HtmlDocument htmlDoc = htmlWeb.Load(url);
            string result = htmlDoc.ParsedText ?? string.Empty;
            if (htmlDoc.ParsedText.Contains("�"))
            {
                result = HttpUtility.HtmlDecode(result);
                return string.Empty;
            }

            return result;
        }

        bool CheckEncode(string url)
        {
            return true;
        }
    }

    public partial class Discovery : ObservableObject
    {
        public string ImgSource { get; set; }
        public string Title { get; set; }

        [ObservableProperty]
        bool _isSelected;
    }

    class Database
    {
        private bool _isLoaded;
        private ObservableCollection<Item> _data = new ObservableCollection<Item>();
        public ObservableCollection<Item> LoadData()
        {
            EnsureLoad();
            return _data;
        }

        async void EnsureLoad()
        {
            lock(this)
            {
                if (_isLoaded)
                    return;
                _isLoaded = true;
            }

            // actual loading
            var resultList = await LoadGoogleNews();
            resultList.ForEach(i => _data.Add(i));
        }

        async Task<List<Item>> LoadGoogleNews()
        {
            var rss = new RssFeedsHttpRequest();
            var data = await rss.GetRssData(ConstantHelper.RssFeedNews.Google_News);
            var doc = new XmlDocument();
            doc.LoadXml(data);
            var json = JsonConvert.SerializeXmlNode(doc.DocumentElement);
            if (string.IsNullOrEmpty(json))
                return new List<Item>();
            var baseData = JsonConvert.DeserializeObject<GoogleNewsModel>(json);
            var feeds = baseData.rss.channel.item;
            return feeds;
        }
    }
}
