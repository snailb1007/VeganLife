using Firebase.Database;
using Firebase.Database.Query;
using VeganLife.Models.FoodModel;

namespace VeganLife.Data.FireBaseData
{
    public static class FirebaseRealtimeData
    {
        const string firebase_client_link = "https://vegan-life-d1c9b-default-rtdb.firebaseio.com/";
        private static FirebaseClient _firebaseDatabase = new FirebaseClient(firebase_client_link);

        public async static Task<IEnumerable<FoodPreviewModel>> GetFoods()
        {
            try
            {
                var data = await _firebaseDatabase.Child("Foods/list").OnceAsync<FoodPreviewModel>().ConfigureAwait(false);
                return data.Select(item => new FoodPreviewModel
                {
                    Id = item.Key,
                    Name = item.Object.Name,
                    Image = item.Object.Image
                });
            }
            catch (FirebaseException e)
            {
                Console.WriteLine(e.StackTrace);
                return Enumerable.Empty<FoodPreviewModel>();
            }
        }

        public static async Task<FoodDetailModel> GetFoodDetail(string id)
        {
            try
            {
                var data = await _firebaseDatabase.Child("Foods/detail").Child(id).OnceSingleAsync<FoodDetailModel>().ConfigureAwait(false);
                return data;
            }
            catch (FirebaseException e)
            {
                Console.WriteLine(e.StackTrace);
                return new FoodDetailModel();
            }
        }

        public async static Task<IEnumerable<VitaminModel>> GetVitamins()
        {
            try
            {
                var dataTask = await _firebaseDatabase.Child("Vitamins/list").OnceAsync<VitaminModel>().ConfigureAwait(false);
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
    }
}
