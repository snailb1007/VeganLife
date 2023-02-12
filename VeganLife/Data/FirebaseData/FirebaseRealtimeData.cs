using Firebase.Database;
using Newtonsoft.Json.Linq;

namespace VeganLife.Data.FireBaseData
{
    public class FirebaseRealtimeData
    {
        const string firebase_client_link = "https://vegan-life-d1c9b-default-rtdb.firebaseio.com/";
        private FirebaseClient _firebaseDatabase;

        public FirebaseRealtimeData()
        {
            _firebaseDatabase = new FirebaseClient(firebase_client_link);
        }

        public async Task<string> GetBackgroundImage(string goal)
        {
            var data = await _firebaseDatabase.Child($"Backgrounds/Themes/{goal}").OnceAsync<string>();
            return data.Select(item => item.Object.ToString()).FirstOrDefault();
        }

        public async Task<List<FoodModel>> GetFoods()
        {
            try
            {
                var data = await _firebaseDatabase.Child("Foods/SummaryFoods").OnceAsync<JArray>();
                return data.Select(item => new FoodModel
                {
                    Name = string.Empty,
                    Content = string.Empty,
                    Image = string.Empty
                }).ToList();
            }
            catch (FirebaseException e)
            {
                Console.WriteLine(e.Message);
                return new List<FoodModel>();
            }
        }

        public async Task<IEnumerable<VitaminModel>> GetVitamins()
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
