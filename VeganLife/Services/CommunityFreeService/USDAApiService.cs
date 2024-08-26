// <copyright file="USDAApiService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using FFImageLoading.Helpers;
using VeganLife.Data.LocalData;
using VeganLife.Helpers;
using VeganLife.Models.CommunityFreeServiceModel;

namespace VeganLife.Services.CommunityFreeService
{
    public class USDAApiService
    {
        private const string BaseUrl = "https://api.nal.usda.gov/fdc/v1/";
        private const string apiKey = "***REMOVED***";

        private readonly UsdaFoodDataStoreService _dataStoreService;

        public USDAApiService()
        {
            _dataStoreService = ServicesHelper.GetService<UsdaFoodDataStoreService>();
        }

        public async Task<USDAFoodNutritionFactModel> GetFoodDetailsByIdAsync(string foodId)
        {
            var result = new USDAFoodNutritionFactModel();
            try
            {
                string url = $"{BaseUrl}food/{foodId}?api_key={apiKey}";
                HttpResponseMessage response = await HttpClientService.Instance.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var streamData = await response.Content.ReadAsStreamAsync();
                    var responseData = await Utf8Json.JsonSerializer.DeserializeAsync<USDAFoodNutritionFactModel>(streamData);
                    if (responseData is not null)
                    {
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
                Debug.WriteLine($"An error occurred: {ex.Message}");
                _ = ex;
                return result;
            }
        }
    }
}
