using VeganLife.Models.FoodModel;
using VeganLife.Services.LocalDataServices;

namespace VeganLife.ViewModels
{
    public partial class MainViewModel : BaseViewModel
    {
        FoodPreviewDataStoreService _dataStoreService;
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
            _dataStoreService = new FoodPreviewDataStoreService();
            Init();
        }

        async void Init()
        {
            if (AccessType == NetworkAccess.Internet)
                Foods = await data_service.GetFoods();
            if (Foods == null || !Foods.Any())
            {
                Foods = await _dataStoreService.GetItemsAsync();
            }
            else
            {
                if (_dataStoreService != null)
                {
                    foreach (var item in Foods)
                    {
                        await _dataStoreService.AddOrUpdateItemAsync(item);
                    }
                }
            }
            //var menu = await data_service.GetFoodMenu();
        }

        [RelayCommand]
        async void GoFoodDetail(object obj)
        {
            //await Shell.Current.GoToAsync(nameof(FoodDetailPage), new Dictionary<string, object> { { "SelectedFood", obj } });
            await navigation_service.NavigateToFoodDetail(obj);
        }
    }
}
