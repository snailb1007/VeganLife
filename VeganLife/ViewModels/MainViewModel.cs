using VeganLife.Data.FireBaseData;
using VeganLife.Models.FoodModel;

namespace VeganLife.ViewModels
{
    public partial class MainViewModel : BaseViewModel
    {
        [ObservableProperty]
        IEnumerable<FoodPreviewModel> _foods;

        public MainViewModel()
        {
            Init();
        }

        async void Init()
        {
            Foods = await FirebaseRealtimeData.GetFoods();
        }
    }
}
