using VeganLife.Helpers;
using VeganLife.Models.CommunityFreeServiceModel;
using VeganLife.Services.CommunityFreeService;

namespace VeganLife.ViewModels.ContentViewModels
{
    public partial class UsdaFoodFactDetailVM : BaseViewModel
    {
        [ObservableProperty]
        private USDAFoodPreviewModel currentFoodPreview;
        [ObservableProperty]
        private USDAFoodNutritionFactModel currentFoodNutritionFact;

        public override Task OnNavigatingTo(object parameter)
        {
            if (parameter != null)
            {
                this.CurrentFoodPreview = (USDAFoodPreviewModel)parameter;
            }

            return base.OnNavigatingTo(parameter);
        }

        public async override Task<Task> ViewAppearingVM()
        {
            if (!string.IsNullOrEmpty(this.CurrentFoodPreview?.Id))
            {
                this.CurrentFoodNutritionFact = await ServicesHelper.GetService<USDAApiService>()
                    .GetFoodDetailsByIdAsync(this.CurrentFoodPreview.Id);
            }

            return base.ViewAppearingVM();
        }
    }
}
