// <copyright file="USDAApiService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AsyncAwaitBestPractices;
using VeganLife.Data.LocalData;
using VeganLife.Helpers;
using VeganLife.Helpers.Extensions;
using VeganLife.Models.CommunityFreeServiceModel;

namespace VeganLife.Services.CommunityFreeService
{
    public class USDAApiService
    {
        private const string BaseUrl = "https://api.nal.usda.gov/fdc/v1/";
        private const string apiKey = "***REMOVED***";

        private readonly UsdaFoodNutritionFactDataStoreService _dataStoreService;

        public USDAApiService(UsdaFoodNutritionFactDataStoreService usdaFoodDataStore)
        {
            _dataStoreService = usdaFoodDataStore;
        }

        public async Task<USDAFoodNutritionFactModel> GetFoodDetailsByIdAsync(string foodId)
        {
            var result = new USDAFoodNutritionFactModel();
            try
            {
                string url = $"{BaseUrl}food/{foodId}?api_key={apiKey}";

                // result = await _dataStoreService.GetItemAsync(foodId);
                HttpResponseMessage response = await HttpClientService.Instance.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var streamData = await response.Content.ReadAsStreamAsync();
                    var responseData = await Utf8Json.JsonSerializer.DeserializeAsync<USDAFoodNutritionFactModel>(streamData);
                    if (responseData is not null)
                    {
                        _dataStoreService.AddOrUpdateItemAsync(responseData).SafeFireAndForget();
                        result = responseData;
                    }
                }
                else
                {
                    Debug.WriteLine($"API request failed with status code: {response.StatusCode}");
                }

                return result;
            }
            catch (Exception ex)
            {
                ex.LogError();
                result = await _dataStoreService.GetItemAsync(foodId);
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
                        _ = ServicesHelper.GetService<INavigationService>().DisplayAlert("Error", "Food item not found", "OK");
                    }
                }
            }
        }
    }
}
