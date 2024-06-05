// <copyright file="DataService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Firebase.Database;
using Firebase.Database.Query;
using HtmlAgilityPack;
using System.Runtime.ConstrainedExecution;
using System.ServiceModel.Syndication;
using System.Xml;
using VeganLife.Data.LocalData;
using VeganLife.Helpers;
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

        // update master
        private const string UpdateMasterVitamin = "/App/updateMaster/vitamin";

        private const string FoodDetailAddress = "Foods/detail";
        private const string MenuFoodAddress = "App/img/menu_food";
        private const string FoodListAddress = "Foods/list";
        private const string VitaminListAddress = "Vitamins";
        private const string FoodNutriFacts = "Foods/nutritionFact";
        private const string MacrosFoodNutriFactDetail = "USDA/food_data_central/details";

        private readonly UpdateMasterDataStoreService _updateMasterDataStoreService;
        private IList<UpdateMasterModel> _updateMasters;

        public DataService()
        {
            _updateMasterDataStoreService = ServicesHelper.GetService<UpdateMasterDataStoreService>();
            _ = _updateMasterDataStoreService.GetItemsAsync()
                .ContinueWith(t =>
                {
                    _updateMasters = new List<UpdateMasterModel>(t.Result);
                });
        }

        private async Task<UpdateMasterStruct> CheckUpdateMaster(string key)
        {
            int ver = 0;
            switch (key)
            {
                case nameof(VitaminModel):
                    ver = await GetUpdateMasterVitamin();
                    break;
            }

            var updateMaster = _updateMasters.FirstOrDefault(i => i.Id == key);
            if (updateMaster is null)
            {
                return new(true, ver, false);
            }

            return new(ver > updateMaster.Version, ver);
        }

        // Method for fetching a single item
        public async Task<T> GetSingleDataFromFirebaseAsync<T>(string path, T defaultValue = default)
        {
            if (string.IsNullOrEmpty(path))
            {
                return defaultValue;
            }

            try
            {
                T result = await this.firebaseDatabase.Child(path).OnceSingleAsync<T>().ConfigureAwait(false);
                return result ?? defaultValue;
            }
            catch (FirebaseException e)
            {
                Debug.WriteLine($"Firebase error in GetSingleDataFromFirebaseAsync: {e.Message}");
                return defaultValue;
            }
        }

        // Method for fetching a collection
        private async Task<IReadOnlyCollection<FirebaseObject<T>>> GetCollectionFromFirebaseAsync<T>(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }

            try
            {
                var result = await this.firebaseDatabase.Child(path).OnceAsync<T>().ConfigureAwait(false);
                return result;
            }
            catch (FirebaseException e)
            {
                Debug.WriteLine($"Firebase error in GetCollectionFromFirebaseAsync: {e.Message}");
                return null;
            }
        }

        public async Task<FoodDetailModel> GetFoodDetail(string id)
        {
            var data = await this.GetSingleDataFromFirebaseAsync($"{FoodDetailAddress}/{id}", new FoodDetailModel());
            data.Id = id;
            return data;
        }

        public async Task<IEnumerable<FoodMenuCategoryModel>> GetFoodMenu()
        {
            var data = await this.GetCollectionFromFirebaseAsync<FoodMenuCategoryModel>(MenuFoodAddress);
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
            {
                return new UndefinedMacroFoodNutriFactModel();
            }

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
            var localVitaminStore = ServicesHelper.GetService<VitaminsDataStoreService>();
            var masterData = await CheckUpdateMaster(nameof(VitaminModel));
            if (masterData.HasUpdate)
            {
                var data = await this.GetCollectionFromFirebaseAsync<VitaminModel>(VitaminListAddress);
                if (data is null)
                {
                    return Enumerable.Empty<VitaminModel>();
                }

                var res = data.Select(i => new VitaminModel
                {
                    Id = i.Key,
                    Content = i.Object?.Content ?? string.Empty,
                    Date = i.Object?.Date ?? string.Empty,
                });
                if (masterData.IsExistMasterTable)
                {
                    await localVitaminStore.DeleteAllItems();
                }

                // Update local data
                await localVitaminStore.SaveItems(res)
                    .ContinueWith(t => _updateMasterDataStoreService
                        .AddOrUpdateItemAsync(
                            new UpdateMasterModel()
                            {
                                Id = nameof(VitaminModel),
                                Version = masterData.Ver,
                            },
                            isUpdate: masterData.IsExistMasterTable))
                    .ConfigureAwait(false);
                return res;
            }
            else
            {
                return await localVitaminStore.GetItemsAsync();
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
                    IsPlantOrigin = item.Object.IsPlantOrigin,
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

        private async Task<int> GetUpdateMasterVitamin()
        {
            try
            {
                var data = await this.firebaseDatabase.Child(UpdateMasterVitamin).OnceSingleAsync<int>();
                return data;
            }
            catch (FirebaseException e)
            {
                _ = e;
                return -1;
            }
        }
    }

    internal record UpdateMasterStruct(bool HasUpdate, int Ver, bool IsExistMasterTable = true);
}
