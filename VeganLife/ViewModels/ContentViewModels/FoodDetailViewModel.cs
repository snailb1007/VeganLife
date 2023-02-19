using VeganLife.Data.FireBaseData;
using VeganLife.Models.FoodModel;

namespace VeganLife.ViewModels.ContentViewModels
{
    public partial class FoodDetailViewModel : BaseViewModel, IQueryAttributable
    {
        [ObservableProperty]
        FoodPreviewModel _foodPreview;

        [ObservableProperty]
        FoodDetailModel _foodDetail;
        public FoodDetailViewModel() { }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query == null) { return; }
            FoodPreview = query["SelectedFood"] as FoodPreviewModel;
            FoodDetail = await FirebaseRealtimeData.GetFoodDetail(FoodPreview.Id ?? string.Empty);
        }
    }
}
