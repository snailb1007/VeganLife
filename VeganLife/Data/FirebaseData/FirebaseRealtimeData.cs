using Firebase.Database;
using Newtonsoft.Json.Linq;
using VeganLife.Models;

namespace VeganLife.Data.FireBaseData
{
    public class FirebaseRealtimeData
    {
        const string _linkOfFirebaseClient = "https://vegan-life-d1c9b-default-rtdb.firebaseio.com/";
        public FirebaseClient FirebaseDatabase { get; private set; }

        public FirebaseRealtimeData()
        {
            FirebaseDatabase = new FirebaseClient(_linkOfFirebaseClient);
        }

        public async Task<string> GetBackgroundImage(string goal)
        {
            var data = await FirebaseDatabase.Child($"Backgrounds/Themes/{goal}").OnceAsync<string>();
            return data.Select(item => item.Object.ToString()).FirstOrDefault();
        }

        public async Task<List<FoodModel>> GetFoods()
        {
            try
            {
                var data = await FirebaseDatabase.Child("Foods/SummaryFoods").OnceAsync<JArray>();
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
    }
}
