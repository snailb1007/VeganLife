using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using VeganLife.Data.FireBaseData;
using VeganLife.Models;

namespace VeganLife.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        IEnumerable<FoodModel> _foods;

        [RelayCommand]
        void RefreshFoods()
        {
            LoadData();
        }

        public MainViewModel()
        {
            LoadData();
        }

        public async void LoadData()
        {
            //if (ConstantHelper.FirebaseData.FirebaseRealtimeData == null)
            //{
            //    ConstantHelper.FirebaseData.FirebaseRealtimeData = new FirebaseRealtimeData();
            //}

            //var x = new FirebaseRealtimeData();

            //Foods = await x.GetFoods();
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://vnexpress.net/rss/suc-khoe.rss"),
                //Headers =
                //{
                //    { "X-RapidAPI-Key", "SIGN-UP-FOR-KEY" },
                //    { "X-RapidAPI-Host", "bloomberg-market-and-financial-news.p.rapidapi.com" },
                //},
            };

            try
            {
                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(body);
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
