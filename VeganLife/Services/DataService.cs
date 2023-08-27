// <copyright file="DataService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.Services
{
    using System.Xml;
    using Firebase.Database;
    using Firebase.Database.Query;
    using Newtonsoft.Json;
    using VeganLife.Data.RssFeedsData;
    using VeganLife.Models.FoodModel;
    using VeganLife.Models.GoogleNewsModels;

    public class DataService : IDataService
    {
        protected readonly FirebaseClient firebaseDatabase = new FirebaseClient(FirebaseClientLink);
        private const string FirebaseClientLink = "https://vegan-life-d1c9b-default-rtdb.firebaseio.com/";
        private const string FoodDetailAddress = "Foods/detail";
        private const string MenuFoodAddress = "App/img/menu_food";
        private const string FoodListAddress = "Foods/list";
        private const string VitaminListAddress = "Vitamins/list";
        private const string FoodNutriFacts = "Foods/nutritionFact";

        #region food
        public async Task<FoodDetailModel> GetFoodDetail(string id)
        {
            try
            {
                var data = await this.firebaseDatabase.Child(FoodDetailAddress).Child(id).OnceSingleAsync<FoodDetailModel>().ConfigureAwait(false);
                data.Id = id;
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
                var data = await this.firebaseDatabase.Child(MenuFoodAddress).OnceAsync<MenuModel>().ConfigureAwait(false);
                return data.Select(item => new FoodMenuCategoryModel
                {
                    ImgSource = item.Object.ImgSource,
                    Title = item.Key,
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
                var data = await this.firebaseDatabase.Child(FoodListAddress).OnceAsync<FoodPreviewModel>().ConfigureAwait(false);
                return data.Select(item => new FoodPreviewModel
                {
                    Id = item.Key,
                    Name = item.Object.Name,
                    Image = item.Object.Image,
                    Time = item.Object.Time,
                    Category = item.Object.Category,
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

        public async Task<FoodNutriFacts> GetFoodNutriFacts(string id)
        {
            if (string.IsNullOrEmpty(id))
                return new FoodNutriFacts();
            try
            {
                var data = await this.firebaseDatabase.Child(FoodNutriFacts).Child(id).OnceSingleAsync<FoodNutriFacts>().ConfigureAwait(false);
                data ??= new FoodNutriFacts();
                data.Id = id;
                return data;
            }
            catch (FirebaseException e)
            {
#if DEBUG
                Console.WriteLine(e.StackTrace);
#endif
                return new FoodNutriFacts();
            }
        }
        #endregion

        public async Task<IEnumerable<VitaminModel>> GetVitamins()
        {
            try
            {
                var dataTask = await this.firebaseDatabase.Child(VitaminListAddress).OnceAsync<VitaminModel>().ConfigureAwait(false);
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
            {
                return Enumerable.Empty<Item>();
            }

            var doc = new XmlDocument();
            doc.LoadXml(data);
            var json = JsonConvert.SerializeXmlNode(doc.DocumentElement);
            if (string.IsNullOrEmpty(json))
            {
                return Enumerable.Empty<Item>();
            }

            var baseData = JsonConvert.DeserializeObject<GoogleNewsModel>(json);
            return baseData.rss.channel.item;
        }
    }
}
