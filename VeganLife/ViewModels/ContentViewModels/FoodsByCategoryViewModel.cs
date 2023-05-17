using VeganLife.Models.FoodModel;

namespace VeganLife.ViewModels.ContentViewModels
{
    public partial class FoodsByCategoryViewModel : BaseViewModel
    {
        [ObservableProperty]
        string _titlePage;
        [ObservableProperty]
        FoodPreviewModel _currentSelectedItem;

        [ObservableProperty]
        IEnumerable<FoodPreviewModel> _foods;
        public FoodsByCategoryViewModel(INavigationService navigationService, IDataService dataService)
            : base(navigationService, dataService)
        {
        }

        public override Task OnNavigatingTo(object parameter)
        {
            var data = parameter as Dictionary<string, IEnumerable<FoodPreviewModel>>;
            if (data != null)
            {
                TitlePage = data.FirstOrDefault().Key;
                Foods = data.FirstOrDefault().Value;
            }

            return base.OnNavigatingTo(parameter);
        }

        [RelayCommand]
        async Task GoFoodDetail()
        {
            await navigation_service.NavigateToFoodDetail(CurrentSelectedItem);
            CurrentSelectedItem = null;
        }
    }
}
