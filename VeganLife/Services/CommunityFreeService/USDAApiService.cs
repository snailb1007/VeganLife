// <copyright file="USDAApiService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Text;
using AsyncAwaitBestPractices;
using Newtonsoft.Json;
using VeganLife.Data.LocalData;
using VeganLife.Helpers;
using VeganLife.Helpers.Extensions;
using VeganLife.Models.CommunityFreeServiceModel;
using VeganLife.Resources.Translations;

namespace VeganLife.Services.CommunityFreeService
{
    public class USDAApiService
    {
        private const string BaseUrl = "https://api.nal.usda.gov/fdc/v1/";
        private const string ApiKey = "8tgleoubqLXYdky38LaQFpMaQIEqvTez4eFV6obc";

        private readonly UsdaFoodNutritionFactDataStoreService _dataStoreService;

        public USDAApiService(UsdaFoodNutritionFactDataStoreService usdaFoodDataStore)
        {
            _dataStoreService = usdaFoodDataStore;
        }

        public async Task<USDAFoodNutritionFactModel> GetFoodDetailsByIdAsync(string foodId)
        {
            var localData = await _dataStoreService.GetItemAsync(foodId);
            if (!string.IsNullOrEmpty(localData?.FoodNutrientsJsonData))
            {
                UtilitiesExtension.LogError("have local data for " + foodId);
                localData.foodNutrients = JsonConvert.DeserializeObject<List<FoodNutrient>>(localData.FoodNutrientsJsonData);
                return localData;
            }

            var result = new USDAFoodNutritionFactModel();
            string url = $"{BaseUrl}food/{foodId}?api_key={ApiKey}";
            try
            {
                HttpResponseMessage response = await HttpClientService.Instance.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var streamData = await response.Content.ReadAsStringAsync();
                    var responseData = JsonConvert.DeserializeObject<USDAFoodNutritionFactModel>(streamData);
                    if (responseData is not null)
                    {
                        responseData.FoodNutrientsJsonData = JsonConvert.SerializeObject(responseData.foodNutrients);
                        _dataStoreService.AddOrUpdateItemAsync(responseData).SafeFireAndForget();
                        result = responseData;
                    }
                }
                else
                {
                    UtilitiesExtension.LogError($"API request failed with status code: {response.StatusCode}");
                }

                return result;
            }
            catch (Exception ex)
            {
                ex.LogError();
                return result;
            }
            finally
            {
                if (result.Id <= 0)
                {
                    if (!ServicesHelper.GetNetworkStatus() && ServicesHelper.GetCurrentViewModel() is BaseViewModel vm)
                    {
                        _ = vm.DisplayNoInternetAlert();
                    }
                    else
                    {
                        _ = ServicesHelper.GetService<INavigationService>()
                            .DisplayAlert(AppResources.error_common, AppResources.notFound_common, "OK");
                    }
                }
            }
        }

        public async Task<List<FoodDetailListRequest>> GetFoodsListAsync(FoodsListRequestModel request)
        {
            var url = $"{BaseUrl}foods/list?api_key={ApiKey}";

            var jsonRequest = JsonConvert.SerializeObject(request);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            var response = await HttpClientService.Instance.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var foods = JsonConvert.DeserializeObject<List<FoodDetailListRequest>>(
                    jsonResponse,
                    new JsonSerializerSettings { DateFormatHandling = DateFormatHandling.IsoDateFormat, NullValueHandling = NullValueHandling.Ignore });
                return foods;
            }
            else
            {
                // Handle error response
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"==> Error: {response.StatusCode}, Details: {errorContent}");
            }
        }
    }
}