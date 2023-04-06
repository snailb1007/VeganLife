using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json;
using System.Xml;
using VeganLife.Data.RssFeedsData;
using VeganLife.Models.FoodModel;

namespace VeganLife.Services
{
    public class DataService : IDataService
    {
        const string firebase_client_link = "https://vegan-life-d1c9b-default-rtdb.firebaseio.com/";
        protected readonly FirebaseClient firebaseDatabase = new FirebaseClient(firebase_client_link);

        public async Task<FoodDetailModel> GetFoodDetail(string id)
        {
            try
            {
                var data = await firebaseDatabase.Child("Foods/detail").Child(id).OnceSingleAsync<FoodDetailModel>().ConfigureAwait(false);
                return data;
            }
            catch (FirebaseException e)
            {
                Console.WriteLine(e.StackTrace);
                return new FoodDetailModel();
            }
        }

        public async Task<IEnumerable<FoodMenuCategoryModel>> GetFoodMenu()
        {
            try
            {
                var data = await firebaseDatabase.Child("App/img/menu_food").OnceAsync<MenuModel>().ConfigureAwait(false);
                return data.Select(item => new FoodMenuCategoryModel
                {
                    ImgSource = item.Object.ImgSource,
                    Title = item.Key
                });
            }
            catch (FirebaseException e)
            {
                Console.WriteLine(e.StackTrace);
                return Enumerable.Empty<FoodMenuCategoryModel>();
            }
        }

        public async Task<IEnumerable<FoodPreviewModel>> GetFoods()
        {
            try
            {
                var data = await firebaseDatabase.Child("Foods/list").OnceAsync<FoodPreviewModel>().ConfigureAwait(false);
                return data.Select(item => new FoodPreviewModel
                {
                    Id = item.Key,
                    Name = item.Object.Name,
                    Image = item.Object.Image,
                    Time = item.Object.Time,
                    Category = item.Object.Category
                });
            }
            catch (FirebaseException e)
            {
                Console.WriteLine(e.StackTrace);
                return Enumerable.Empty<FoodPreviewModel>();
            }
        }

        public async Task<IEnumerable<VitaminModel>> GetVitamins()
        {
            try
            {
                var dataTask = await firebaseDatabase.Child("Vitamins/list").OnceAsync<VitaminModel>().ConfigureAwait(false);
                return dataTask.Select(i => new VitaminModel
                {
                    Id = i.Key,
                    Name = i.Object.Name,
                    Image = i.Object.Image,
                    Summary = i.Object.Summary,
                    WebView = i.Object.WebView,
                });
            }
            catch (FirebaseException e)
            {
                Console.WriteLine(e.StackTrace);
                return Enumerable.Empty<VitaminModel>();
            }
        }

        public async Task<List<Item>> LoadGoogleNews(string uri)
        {
            var rss = new RssFeedsHttpRequest();
            var data = await rss.GetRssData(uri);
            if (string.IsNullOrEmpty(data))
                return new List<Item>();
            var doc = new XmlDocument();
            doc.LoadXml(data);
            var json = JsonConvert.SerializeXmlNode(doc.DocumentElement);
            if (string.IsNullOrEmpty(json))
                return new List<Item>();
            var baseData = JsonConvert.DeserializeObject<GoogleNewsModel>(json);
            return baseData.rss.channel.item;
        }
    }
}
