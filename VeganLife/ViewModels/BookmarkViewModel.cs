using VeganLife.Data.LocalData;
using VeganLife.Helpers;
using VeganLife.Models.FoodModel;
using VeganLife.Services.LocalDataServices;

namespace VeganLife.ViewModels
{
    public partial class BookmarkViewModel : BaseViewModel
    {
        FoodPreviewDataStoreService _dataStoreService;

        [ObservableProperty]
        IList<FoodPreviewModel> _foods;
        [ObservableProperty]
        FoodPreviewModel _foodSelected;
        public BookmarkViewModel(INavigationService navigationService, IDataService dataService)
            : base(navigationService, dataService)
        {
            var database = ServicesHelper.GetService<ISQLite>();
            if (database != null)
                _dataStoreService = new FoodPreviewDataStoreService(database);
            Init();
        }

        async void Init()
        {
            Foods = (await _dataStoreService.GetItemsAsync()).Where(i => i.IsBookmarked).ToList();
        }

        [RelayCommand]
        async Task GoFoodDetail(object obj)
        {
            await _dataStoreService.AddOrUpdateItemAsync(FoodSelected, true);
            await navigation_service.NavigateToFoodDetail(obj);
        }

        public override Task OnNavigatedFrom(bool isForwardNavigation)
        {
            Console.WriteLine("==>OnNavigatedFrom");
            FoodSelected = null;
            return base.OnNavigatedFrom(isForwardNavigation);
        }

        public override Task OnNavigatedTo()
        {
            Console.WriteLine("==>OnNavigatedTo");
            return base.OnNavigatedTo();
        }
    }
}
