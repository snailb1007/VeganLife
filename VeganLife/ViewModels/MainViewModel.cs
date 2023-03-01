using VeganLife.Data.FireBaseData;
using VeganLife.Models.FoodModel;
using VeganLife.Views.FoodTab;

namespace VeganLife.ViewModels
{
    public partial class MainViewModel : BaseViewModel
    {
        [ObservableProperty]
        IEnumerable<FoodPreviewModel> _foods;

        public IList<MenuModel> Categorys = new List<MenuModel>()
        {
            new MenuModel() { ImgSource = "", Title = "Bữa sáng" },
            new MenuModel() { ImgSource = "", Title = "Đồ uống" },
            new MenuModel() { ImgSource = "", Title = "Bữa tối" },
            new MenuModel() { ImgSource = "", Title = "Món tráng miệng" },
        };

        public MainViewModel()
        {
            Init();
        }

        async void Init()
        {
            Foods = await FirebaseRealtimeData.GetFoods();
        }

        [RelayCommand]
        async void GoFoodDetail(object obj)
        {
            await Shell.Current.GoToAsync(nameof(FoodDetailPage), new Dictionary<string, object> { { "SelectedFood", obj } });
        }
    }
}
