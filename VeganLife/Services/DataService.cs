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
        const string food_detail_address = "Foods/detail";
        const string menu_food_address = "App/img/menu_food";
        const string food_list_address = "Foods/list";
        const string vitamin_list_address = "Vitamins/list";

        protected readonly FirebaseClient firebaseDatabase = new FirebaseClient(firebase_client_link);

        public async Task<FoodDetailModel> GetFoodDetail(string id)
        {
            try
            {
                var data = await firebaseDatabase.Child(food_detail_address).Child(id).OnceSingleAsync<FoodDetailModel>().ConfigureAwait(false);
                return data;
            }
            catch (FirebaseException e)
            {
#if DEBUG
                Console.WriteLine(e.StackTrace);
#endif
                return new FoodDetailModel();
            }
        }

        public async Task<IEnumerable<FoodMenuCategoryModel>> GetFoodMenu()
        {
            try
            {
                var data = await firebaseDatabase.Child(menu_food_address).OnceAsync<MenuModel>().ConfigureAwait(false);
                return data.Select(item => new FoodMenuCategoryModel
                {
                    ImgSource = item.Object.ImgSource,
                    Title = item.Key
                });
            }
            catch (FirebaseException e)
            {
#if DEBUG
                Console.WriteLine(e.StackTrace);
#endif
                return Enumerable.Empty<FoodMenuCategoryModel>();
            }
        }

        public async Task<IEnumerable<FoodPreviewModel>> GetFoods()
        {
            try
            {
                var data = await firebaseDatabase.Child(food_list_address).OnceAsync<FoodPreviewModel>().ConfigureAwait(false);
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
#if DEBUG
                Console.WriteLine(e.StackTrace);
#endif
                return Enumerable.Empty<FoodPreviewModel>();
            }
        }

        public async Task<IEnumerable<VitaminModel>> GetVitamins()
        {
            try
            {
                var dataTask = await firebaseDatabase.Child(vitamin_list_address).OnceAsync<VitaminModel>().ConfigureAwait(false);
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
#if DEBUG
                Console.WriteLine(e.StackTrace);
#endif
                return Enumerable.Empty<VitaminModel>();
            }
        }

        public async Task<IEnumerable<Item>> LoadGoogleNews(string uri)
        {
            var rss = new RssFeedsHttpRequest();
            var data = await rss.GetRssData(uri);
            if (string.IsNullOrEmpty(data))
                return Enumerable.Empty<Item>();
            var doc = new XmlDocument();
            doc.LoadXml(data);
            var json = JsonConvert.SerializeXmlNode(doc.DocumentElement);
            if (string.IsNullOrEmpty(json))
                return Enumerable.Empty<Item>();
            var baseData = JsonConvert.DeserializeObject<GoogleNewsModel>(json);
            return baseData.rss.channel.item;
        }
    }
}
