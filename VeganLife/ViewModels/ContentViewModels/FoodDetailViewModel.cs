using VeganLife.Data.LocalData;
using VeganLife.Helpers;
using VeganLife.Models.FoodModel;
using VeganLife.Services.LocalDataServices;

namespace VeganLife.ViewModels.ContentViewModels
{
    public partial class FoodDetailViewModel : BaseViewModel
    {
        [ObservableProperty]
        bool _isDecorateExpaned;

        [ObservableProperty]
        bool _isIngredientExpaned = true;

        [ObservableProperty]
        bool _isMakingExpaned;

        [ObservableProperty]
        bool _isSauceExpaned;

        FoodDetailDataStoreService _foodDetailDataStoreService;
        [ObservableProperty]
        FoodPreviewModel _foodPreview;

        [ObservableProperty]
        FoodDetailModel _foodDetail;
        public FoodDetailViewModel(INavigationService navigationService, IDataService dataService)
            : base(navigationService, dataService)
        {
            var database = ServicesHelper.GetService<ISQLite>();
            if (database != null)
                _foodDetailDataStoreService = new FoodDetailDataStoreService(database);
        }

        public override async Task<Task> OnNavigatingTo(object parameter)
        {
            if (parameter is not null)
            {
                FoodPreview = parameter as FoodPreviewModel;
                FoodPreview.IsRead = true;

                if (IsNetworkConnected)
                    FoodDetail = await data_service.GetFoodDetail(FoodPreview?.Id ?? string.Empty);
                if (FoodDetail == null)
                    FoodDetail = (await _foodDetailDataStoreService.GetItemsAsync()).FirstOrDefault();
                else
                    await _foodDetailDataStoreService.AddOrUpdateItemAsync(FoodDetail);
            }

            return base.OnNavigatingTo(parameter);
        }


        [RelayCommand]
        void OnBookmarkClicked()
        {
            if (FoodPreview == null) return;
            FoodPreview.IsBookmarked = !FoodPreview.IsBookmarked;
        }

        [RelayCommand]
        void ExpandClicked(string param)
        {
            switch(param)
            {
                case "1":
                    IsIngredientExpaned = !IsIngredientExpaned;
                    break;
                case "2":
                    break;
                case "3":
                    break;
                case "4":
                    break;
            };
        }
    }
}
