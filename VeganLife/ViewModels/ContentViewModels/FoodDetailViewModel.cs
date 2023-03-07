using VeganLife.Models.FoodModel;

namespace VeganLife.ViewModels.ContentViewModels
{
    public partial class FoodDetailViewModel : BaseViewModel
    {
        [ObservableProperty]
        FoodPreviewModel _foodPreview;

        [ObservableProperty]
        FoodDetailModel _foodDetail;
        public FoodDetailViewModel(INavigationService navigationService, IDataService dataService)
            : base(navigationService, dataService) { }

        public override async Task<Task> OnNavigatingTo(object parameter)
        {
            if (parameter is not null)
            {
                FoodPreview = parameter as FoodPreviewModel;
                FoodDetail = await data_service.GetFoodDetail(FoodPreview?.Id ?? string.Empty);
            }

            return base.OnNavigatingTo(parameter);
        }
    }
}
