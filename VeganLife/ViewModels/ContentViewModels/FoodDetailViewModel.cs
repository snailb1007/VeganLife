using VeganLife.Models.FoodModel;

namespace VeganLife.ViewModels.ContentViewModels
{
    public partial class FoodDetailViewModel : BaseViewModel, IQueryAttributable
    {
        [ObservableProperty]
        FoodPreviewModel _foodPreview;

        [ObservableProperty]
        FoodDetailModel _foodDetail;
        public FoodDetailViewModel(IDataService dataService) : base(dataService) { }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query == null) { return; }
            FoodPreview = query["SelectedFood"] as FoodPreviewModel;
            FoodDetail = await data_service.GetFoodDetail(FoodPreview.Id ?? string.Empty);
        }
    }
}
