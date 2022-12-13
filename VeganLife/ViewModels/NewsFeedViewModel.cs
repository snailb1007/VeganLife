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

namespace VeganLife.ViewModels
{
    public partial class NewsFeedViewModel : ObservableObject
    {
        Database _database;

        [ObservableProperty]
        bool _isLoading;

        [ObservableProperty]
        ObservableCollection<Item> _feeds;

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
            Feeds = _database.LoadData();
            InitMenu();
            IsLoading= false;
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

        partial void OnDiscoveryMenuChanged(List<Discovery> value)
        {
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
            LoadUrlPreview(feeds.FirstOrDefault().link);
            //feeds.ForEach(item => item.description.cdatasection = StringProcessHelper.ExtractImgSrc(item.description?.cdatasection) ?? string.Empty);
            return feeds;
        }


        async void LoadUrlPreview(string url)
        {
            HtmlWeb htmlWeb = new HtmlWeb() { AutoDetectEncoding = false, OverrideEncoding = Encoding.UTF8 };
            HtmlDocument htmlDoc = htmlWeb.Load(url);
            Console.WriteLine("thien==>" + htmlDoc.ParsedText);
            //using (var response = await (new HttpClient()).GetAsync(url))
            //using (var content = response.Content)
            //{
            //    if (content != null && response.IsSuccessStatusCode)
            //    {
            //        var buffer = await response.Content.ReadAsStringAsync();
            //    }
            //}
            //var threadItems = htmlDoc.DocumentNode.SelectNodes("//ul[@id='threads']/li").ToList();

            //var items = new List<object>();
            //foreach (var item in threadItems)
            //{
            //    //Extract các giá trị từ các tag con của tag li
            //    var linkNode = item.SelectSingleNode(".//a[contains(@class,'title')]");
            //    var link = linkNode.Attributes["href"].Value;
            //    var text = linkNode.InnerText;
            //    var readCount = item.SelectSingleNode(".//div[@class='folTypPost']/ul/li/b").InnerText;

            //    items.Add(new { text, readCount, link });
            //}
        }
    }
}
