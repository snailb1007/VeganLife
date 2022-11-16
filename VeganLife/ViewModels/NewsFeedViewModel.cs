using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using VeganLife.Data.RssFeedsData;
using VeganLife.Helpers.AppSetting;
using VeganLife.Models;

namespace VeganLife.ViewModels
{
    public partial class NewsFeedViewModel : ObservableObject
    {
        [ObservableProperty]
        IEnumerable<Item> _feeds;

        [RelayCommand]
        void RefreshFoods()
        {
        }

        public NewsFeedViewModel()
        {
            LoadData();
        }

        public async void LoadData()
        {
            var rss = new RssFeedsHttpRequest();
            var data = await rss.GetRssData(ConstantHelper.RssFeedNews.Google_News);
            var doc = new XmlDocument();
            doc.LoadXml(data);
            var json = JsonConvert.SerializeXmlNode(doc.DocumentElement);
            var baseData = JsonConvert.DeserializeObject<GoogleNewsModel>(json);
            Feeds = baseData.rss.channel.item.ToList();
        }
    }
}
