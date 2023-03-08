using Firebase.Database;
using Firebase.Database.Query;
using VeganLife.Models.FoodModel;

namespace VeganLife.Services
{
    public class DataService : IDataService
    {
        const string firebase_client_link = "https://vegan-life-d1c9b-default-rtdb.firebaseio.com/";
        protected readonly FirebaseClient firebaseDatabase = new FirebaseClient(firebase_client_link);

        public async Task<FoodDetailModel> GetFoodDetail(string id)
        {
            try
            {
                var data = await firebaseDatabase.Child("Foods/detail").Child(id).OnceSingleAsync<FoodDetailModel>().ConfigureAwait(false);
                return data;
            }
            catch (FirebaseException e)
            {
                Console.WriteLine(e.StackTrace);
                return new FoodDetailModel();
            }
        }

        public async Task<IEnumerable<MenuModel>> GetFoodMenu()
        {
            try
            {
                var data = await firebaseDatabase.Child("App/img/menu_food").OnceAsync<MenuModel>().ConfigureAwait(false);
                return data.Select(item => new MenuModel
                {
                    ImgSource = item.Object.ImgSource
                });
            }
            catch (FirebaseException e)
            {
                Console.WriteLine(e.StackTrace);
                return Enumerable.Empty<MenuModel>();
            }
        }

        public async Task<IEnumerable<FoodPreviewModel>> GetFoods()
        {
            try
            {
                var data = await firebaseDatabase.Child("Foods/list").OnceAsync<FoodPreviewModel>().ConfigureAwait(false);
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

        public async Task<IEnumerable<VitaminModel>> GetVitamins()
        {
            try
            {
                var dataTask = await firebaseDatabase.Child("Vitamins/list").OnceAsync<VitaminModel>().ConfigureAwait(false);
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
