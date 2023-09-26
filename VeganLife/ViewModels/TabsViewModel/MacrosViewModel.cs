using VeganLife.Models.CommunityFreeServiceModel;

namespace VeganLife.ViewModels.TabsViewModel
{
    public partial class MacrosViewModel : BaseViewModel
    {
        [ObservableProperty]
        private ObservableCollection<USDAFoodPreviewModel> usdaFoodPreviews;
        public MacrosViewModel()
            : base()
        {
        }

        [RelayCommand]
        async Task ClickedThisTab()
        {
            await this.dataService.GetFoodsUSDA()
                .ContinueWith(t => UsdaFoodPreviews = new ObservableCollection<USDAFoodPreviewModel>(t.Result));
        }
    }
}
