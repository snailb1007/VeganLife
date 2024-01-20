using VeganLife.Models.CommunityFreeServiceModel;

namespace VeganLife.Services.CommunityFreeService
{
    public class USDAApiService
    {
        private const string BaseUrl = "https://api.nal.usda.gov/fdc/v1/";
        private const string apiKey = "8tgleoubqLXYdky38LaQFpMaQIEqvTez4eFV6obc";

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
#if DEBUG
                    Console.WriteLine($"API request failed with status code: {response.StatusCode}");
#endif
                }

                return result;
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine($"An error occurred: {ex.Message}");
#endif
                _ = ex;
                return result;
            }
        }
    }
}
