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
            new MenuModel() { Title = "Bữa sáng" },
            new MenuModel() { Title = "Đồ uống" },
            new MenuModel() { Title = "Bữa tối" },
            new MenuModel() { Title = "Món tráng miệng" },
        };

        public MainViewModel(INavigationService navigationService, IDataService dataService)
            : base(navigationService, dataService)
        {
            Init();
        }

        async void Init()
        {
            Foods = await data_service.GetFoods();
            var menu = await data_service.GetFoodMenu();
        }

        [RelayCommand]
        async void GoFoodDetail(object obj)
        {
            //await Shell.Current.GoToAsync(nameof(FoodDetailPage), new Dictionary<string, object> { { "SelectedFood", obj } });
            await navigation_service.NavigateToFoodDetail(obj);
        }
    }
}
