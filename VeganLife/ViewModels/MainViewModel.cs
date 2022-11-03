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

            var x = new FirebaseRealtimeData();

            Foods = await x.GetFoods();
        }
    }
}
