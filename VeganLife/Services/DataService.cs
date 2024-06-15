// <copyright file="DataService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Firebase.Database;
using Firebase.Database.Query;
using HtmlAgilityPack;
using System.Runtime.ConstrainedExecution;
using System.ServiceModel.Syndication;
using System.Xml;
using VeganLife.Data;
using VeganLife.Data.LocalData;
using VeganLife.Helpers;
using VeganLife.Models.CommunityFreeServiceModel;
using VeganLife.Models.FirebaseDataModel;
using VeganLife.Models.FoodModel;
using VeganLife.Models.GoogleNewsModels;
using VeganLife.Resources.Translations;

// Ignore Spelling: Firebase Nutri
namespace VeganLife.Services
{
    public class DataService : IDataService
    {
        protected readonly FirebaseClient firebaseDatabase = new FirebaseClient(FirebaseClientLink);
        private const string FirebaseClientLink = "https://vegan-life-d1c9b-default-rtdb.firebaseio.com/";

        // update master
        private const string UpdateMasterVitaminAddress = "/App/updateMaster/vitamin";
        private const string UpdateMasterUsdaFoodsAddress = "/App/updateMaster/usdaFoods";

        private const string FoodDetailAddress = "Foods/detail";
        private const string MenuFoodAddress = "App/img/menu_food";
        private const string FoodListAddress = "Foods/list";
        private const string VitaminListAddress = "Vitamins";
        private const string AthleticNutritionsAddress = "/AthleticNutritions";

        private const string FoodNutriFacts = "Foods/nutritionFact";
        private const string MacrosFoodNutriFactDetail = "USDA/food_data_central/details";
        private const string UsdaFoodPreviewsAddress = "/USDA/food_data_central/list";

        private readonly UpdateMasterDataStoreService _updateMasterDataStoreService;
        private IList<UpdateMasterModel> _updateMasters;
        private readonly IDictionary<string, object> _dataStore;

        public DataService()
        {
            _updateMasterDataStoreService = ServicesHelper.GetService<UpdateMasterDataStoreService>();
            _dataStore = new Dictionary<string, object>
            {
                { nameof(VitaminModel), ServicesHelper.GetService<VitaminsDataStoreService>() },
                { nameof(USDAFoodPreviewModel), ServicesHelper.GetService<UsdaFoodPreviewsDataStore>() },
                { nameof(AthleticNutritionModel), ServicesHelper.GetService<AthleticNutritionDataStore>() },
            };
            _ = _updateMasterDataStoreService.GetItemsAsync()
                .ContinueWith(t =>
                {
                    _updateMasters = new List<UpdateMasterModel>(t.Result);
                });
        }

        public async Task<bool> GetMaintenanceStatusAsync()
        {
            try
            {
                var data = await this.firebaseDatabase.Child("/App/isServerInMaintenance").OnceSingleAsync<bool>();
                if (data)
                {
                    await App.Current?.MainPage?.DisplayAlert(
                        title: AppResources.Infor_common,
                        message: AppResources.ServiecStop_common,
                        "OK")!;
                }

                return data;
            }
            catch (FirebaseException e)
            {
                _ = e;
                Debug.WriteLine(e.StackTrace);
                return false;
            }
        }

        private async Task<UpdateMasterStruct> CheckUpdateMaster(string key)
        {
            int ver = 0;
            switch (key)
            {
                case nameof(VitaminModel):
                    ver = await GetUpdateMasterVitamin();
                    break;
                case nameof(USDAFoodPreviewModel):
                    ver = await GetUpdateMasterUsdaFoods();
                    break;
                case nameof(AthleticNutritionModel):
                    ver = await GetUpdateMasterAthleticNutritions();
                    break;
            }

            var updateMaster = _updateMasters.FirstOrDefault(i => i.Id == key);
            if (updateMaster is null)
            {
                return new(true, ver, false);
            }

            return new(ver == 0 || ver > updateMaster.Version, ver);
        }

        // Method for fetching a single item
        private async Task<T> GetSingleDataFromFirebaseAsync<T>(string path, T defaultValue = default)
        {
            if (string.IsNullOrEmpty(path) || await GetMaintenanceStatusAsync())
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
            if (string.IsNullOrEmpty(path) || await GetMaintenanceStatusAsync())
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

        private async Task<IEnumerable<TModel>> GetDatasAsync<TModel>(string modelName, string dataAddress)
            where TModel : new()
        {
            var localData = _dataStore[modelName] as IDataStoreService<TModel>;
            var masterData = await CheckUpdateMaster(modelName);
            if (masterData.HasUpdate)
            {
                var data = await this.GetCollectionFromFirebaseAsync<TModel>(dataAddress);
                if (data == null)
                {
                    return Enumerable.Empty<TModel>();
                }

                var result = data.Select(MapToModel).ToList();

                if (localData is null)
                {
                    return result;
                }

                if (masterData.IsExistMasterTable)
                {
                    await localData.DeleteAllItems();
                }

                await Task.WhenAll(localData.SaveItems(result), UpdateMasterDataAsync(modelName, masterData))
                    .ConfigureAwait(false);
                return result;
            }
            else
            {
                if (localData == null)
                {
                    return Enumerable.Empty<TModel>();
                }

                return await localData.GetItemsAsync();
            }
        }

        private TModel MapToModel<TModel>(FirebaseObject<TModel> firebaseObject)
            where TModel : new()
        {
            if (typeof(TModel) == typeof(VitaminModel) && firebaseObject.Object is VitaminModel data)
            {
                var item = firebaseObject.Object as VitaminModel;
                return (TModel)((object)new VitaminModel
                {
                    Id = firebaseObject.Key,
                    Content = data.Content ?? string.Empty,
                    Date = data.Date ?? string.Empty,
                });
            }
            else if (typeof(TModel) == typeof(USDAFoodPreviewModel))
            {
                var item = firebaseObject.Object as USDAFoodPreviewModel;
                return (TModel)(object)new USDAFoodPreviewModel
                {
                    Id = firebaseObject.Key,
                    Image = item?.Image ?? string.Empty,
                    Name = item?.Name ?? string.Empty,
                    Category = item?.Category ?? string.Empty,
                    IsPlantOrigin = item?.IsPlantOrigin ?? false,
                };
            }
            else if (typeof(TModel) == typeof(AthleticNutritionModel) && firebaseObject.Object is AthleticNutritionModel thleticNutritionModelData)
            {
                var item = firebaseObject.Object as AthleticNutritionModel;
                return (TModel)((object)new AthleticNutritionModel
                {
                    Id = firebaseObject.Key,
                    Content = thleticNutritionModelData.Content ?? string.Empty,
                    Date = thleticNutritionModelData.Date ?? string.Empty,
                });
            }
            else
            {
                throw new InvalidOperationException("Unsupported model type");
            }
        }

        private async Task UpdateMasterDataAsync(string modelName, UpdateMasterStruct masterData)
        {
            await _updateMasterDataStoreService.AddOrUpdateItemAsync(
                new UpdateMasterModel()
                {
                    Id = modelName,
                    Version = masterData.Ver,
                },
                isUpdate: masterData.IsExistMasterTable);
        }

        public async Task<IEnumerable<VitaminModel>> GetVitamins() =>
            await GetDatasAsync<VitaminModel>(nameof(VitaminModel), VitaminListAddress);

        public async Task<IEnumerable<USDAFoodPreviewModel>> GetFoodsUSDA() =>
            await GetDatasAsync<USDAFoodPreviewModel>(nameof(USDAFoodPreviewModel), UsdaFoodPreviewsAddress);

        public async Task<IEnumerable<AthleticNutritionModel>> GetAthleticNutritions() =>
            await GetDatasAsync<AthleticNutritionModel>(nameof(AthleticNutritionModel), AthleticNutritionsAddress);

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
                ImgSource = item?.Object?.ImgSource ?? string.Empty,
                Title = item?.Key ?? string.Empty,
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
                Debug.WriteLine(ex.Message);
                return Enumerable.Empty<Item>();
            }
        }

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
                        Debug.WriteLine($"Failed to fetch content from {url}. Status code: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    _ = ex;
                    Debug.WriteLine($"An error occurred: {ex.Message}");
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
                Debug.WriteLine(e.StackTrace);
                return null;
            }
        }

        private async Task<int> GetUpdateMasterVitamin()
        {
            try
            {
                var data = await this.firebaseDatabase.Child(UpdateMasterVitaminAddress).OnceSingleAsync<int>();
                return data;
            }
            catch (FirebaseException e)
            {
                _ = e;
                return -1;
            }
        }

        // get update master for usda foods
        private async Task<int> GetUpdateMasterUsdaFoods()
        {
            try
            {
                var data = await this.firebaseDatabase.Child(UpdateMasterUsdaFoodsAddress).OnceSingleAsync<int>();
                return data;
            }
            catch (FirebaseException e)
            {
                _ = e;
                return -1;
            }
        }

        // get update master for AthleticNutritions
        private async Task<int> GetUpdateMasterAthleticNutritions()
        {
            try
            {
                var data = await this.firebaseDatabase.Child("/App/updateMaster/athleticNutrition").OnceSingleAsync<int>();
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
