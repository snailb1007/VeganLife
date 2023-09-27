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

        public override async Task<Task> ViewAppearingVM()
        {
            if (!UsdaFoodPreviews?.Any() ?? true)
            {
                await this.dataService.GetFoodsUSDA()
                .ContinueWith(t => UsdaFoodPreviews = new ObservableCollection<USDAFoodPreviewModel>(t.Result));
            }

            return base.ViewAppearingVM();
        }
    }
}
