// <copyright file="DataService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Firebase.Database;
using Firebase.Database.Query;
using HtmlAgilityPack;
using System.ServiceModel.Syndication;
using System.Xml;
using VeganLife.Models.CommunityFreeServiceModel;
using VeganLife.Models.FirebaseDataModel;
using VeganLife.Models.FoodModel;
using VeganLife.Models.GoogleNewsModels;

// Ignore Spelling: Firebase Nutri
namespace VeganLife.Services
{
    public class DataService : IDataService
    {
        protected readonly FirebaseClient firebaseDatabase = new FirebaseClient(FirebaseClientLink);
        private const string FirebaseClientLink = "https://vegan-life-d1c9b-default-rtdb.firebaseio.com/";
        private const string FoodDetailAddress = "Foods/detail";
        private const string MenuFoodAddress = "App/img/menu_food";
        private const string FoodListAddress = "Foods/list";
        private const string VitaminListAddress = "Vitamins/list";
        private const string FoodNutriFacts = "Foods/nutritionFact";
        private const string MacrosFoodNutriFactDetail = "USDA/food_data_central/details";

        public DataService()
        {
        }

        public async Task<FoodDetailModel> GetFoodDetail(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new FoodDetailModel();
            }

            try
            {
                var data = await this.firebaseDatabase.Child(FoodDetailAddress).Child(id).OnceSingleAsync<FoodDetailModel>();
                data ??= new FoodDetailModel();
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
                if (data is null)
                {
                    return Enumerable.Empty<FoodMenuCategoryModel>();
                }

                return data.Select(item => new FoodMenuCategoryModel
                {
                    ImgSource = item?.Object?.ImgSource,
                    Title = item?.Key,
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
                    Star = item.Object.Star,
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

        public async Task<FoodNutrientFacts> GetFoodNutriFacts(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new FoodNutrientFacts();
            }

            try
            {
                var data = await this.firebaseDatabase.Child(FoodNutriFacts).Child(id).OnceSingleAsync<FoodNutrientFacts>().ConfigureAwait(false);
                data ??= new FoodNutrientFacts();
                data.Id = id;
                return data;
            }
            catch (FirebaseException e)
            {
#if DEBUG
                Console.WriteLine(e.StackTrace);
#endif
                return new FoodNutrientFacts();
            }
        }

        public async Task<UndefinedMacroFoodNutriFactModel> GetMacroFoodNutriFacts(string id)
        {
            if (string.IsNullOrEmpty(id))
                return new UndefinedMacroFoodNutriFactModel();
            try
            {
                var data = await this.firebaseDatabase.Child(MacrosFoodNutriFactDetail).Child(id)
                    .OnceSingleAsync<UndefinedMacroFoodNutriFactModel>();
                data ??= new UndefinedMacroFoodNutriFactModel();
                data.FdcId = id;
                return data;
            }
            catch (FirebaseException e)
            {
#if DEBUG
                Console.WriteLine(e.StackTrace);
#endif
                return new UndefinedMacroFoodNutriFactModel();
            }
        }

        public async Task<IEnumerable<VitaminModel>> GetVitamins()
        {
            try
            {
                var dataTask = await this.firebaseDatabase.Child(VitaminListAddress).OnceAsync<VitaminModel>().ConfigureAwait(false);
                return dataTask.Select(i => new VitaminModel
                {
                    Id = i.Key,
                    Name = i.Object?.Name,
                    Image = i.Object?.Image,
                    Summary = i.Object?.Summary,
                    WebView = i.Object?.WebView,
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

        #region USDA
        public async Task<IEnumerable<USDAFoodPreviewModel>> GetFoodsUSDA()
        {
            try
            {
                var data = await this.firebaseDatabase.Child("/USDA/food_data_central/list").OnceAsync<USDAFoodPreviewModel>();
                return data.Select(item => new USDAFoodPreviewModel
                {
                    Id = item.Key,
                    Image = item.Object.Image,
                    Name = item.Object.Name,
                    Category = item.Object.Category,
                    IsPlantOrigin = item.Object.IsPlantOrigin
                });
            }
            catch (FirebaseException e)
            {
                _ = e;
#if DEBUG
                Console.WriteLine(e.StackTrace);
#endif
            }

            return new[] { new USDAFoodPreviewModel { } };
        }

        #endregion

        #region Google news feed
        //public async Task<IEnumerable<Item>> LoadGoogleNews(string uri)
        //{
        //    var data = await _rssFeedsHttpRequest.GetRssData(uri);
        //    if (string.IsNullOrEmpty(data))
        //    {
        //        return Enumerable.Empty<Item>();
        //    }

        //    var doc = new XmlDocument();
        //    doc.LoadXml(data);
        //    var json = JsonConvert.SerializeXmlNode(doc.DocumentElement);
        //    if (string.IsNullOrEmpty(json))
        //    {
        //        return Enumerable.Empty<Item>();
        //    }

        //    var baseData = JsonConvert.DeserializeObject<GoogleNewsModel>(json);
        //    return baseData.rss.channel.item;
        //}

        public IEnumerable<Item> ReadRssFeed(string url)
        {
            try
            {
                var results = new List<Item>();
                using (XmlReader reader = XmlReader.Create(url))
                {
                    SyndicationFeed feed = SyndicationFeed.Load(reader);
                    foreach (var i in feed.Items)
                    {
                        results.Add(new Item
                        {
                            title = i.Title.Text,
                            LocalTimePosted = i.PublishDate.DateTime,
                            link = i.Links.FirstOrDefault()?.Uri.ToString() ?? string.Empty,
                        });
                    }

                    return results;
                }
            }
            catch (Exception ex)
            {
                _ = ex;
#if DEBUG
                Console.WriteLine(ex.Message);
#endif
                return Enumerable.Empty<Item>();
            }
        }
        #endregion

        public async Task<List<string>> GetImageLinksAsync(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return new List<string>();
            }

            List<string> imageLinks = new List<string>();

            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await httpClient.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        string htmlContent = await response.Content.ReadAsStringAsync();

                        HtmlDocument htmlDocument = new HtmlDocument();
                        htmlDocument.LoadHtml(htmlContent);

                        // Select all image elements with an "src" attribute
                        HtmlNodeCollection imgNodes = htmlDocument.DocumentNode.SelectNodes("//img[@src]");

                        if (imgNodes != null)
                        {
                            foreach (HtmlNode imgNode in imgNodes)
                            {
                                string imgSrc = imgNode.GetAttributeValue("src", string.Empty);
                                if (!string.IsNullOrEmpty(imgSrc))
                                {
                                    imageLinks.Add(imgSrc);
                                }
                            }
                        }
                    }
                    else
                    {
#if DEBUG
                        Console.WriteLine($"Failed to fetch content from {url}. Status code: {response.StatusCode}");
#endif
                    }
                }
                catch (Exception ex)
                {
                    _ = ex;
#if DEBUG
                    Console.WriteLine($"An error occurred: {ex.Message}");
#endif
                }
            }

            return imageLinks;
        }

        public async Task<string> GetFireBaseValue(string nodePath)
        {
            if (string.IsNullOrEmpty(nodePath))
            {
                return string.Empty;
            }
            else
            {
                try
                {
                    var data = await this.firebaseDatabase.Child(nodePath).OnceSingleAsync<string>();
                    return data;
                }
                catch (FirebaseException firebaseE)
                {
#if DEBUG
                    await Console.Out.WriteLineAsync(firebaseE.Message);
#endif
                    return string.Empty;
                }
            }
        }

        public async Task<BmiModel> GetHealthDiagnosisFirebaseDataModel()
        {
            try
            {
                var data = await this.firebaseDatabase.Child("/HealthDiagonosis/BMI").OnceSingleAsync<BmiModel>();
                return data;
            }
            catch (FirebaseException e)
            {
                _ = e;
#if DEBUG
                Console.WriteLine(e.StackTrace);
#endif
                return null;
            }
        }
    }
}
