using VeganLife.Data.LocalData;
using VeganLife.Helpers;
using VeganLife.Helpers.AppSetting;
using VeganLife.Models.CommunityFreeServiceModel;
using VeganLife.Services.CommunityFreeService;

namespace VeganLife.ViewModels.ContentViewModels
{
    public partial class UsdaFoodFactDetailVM : BaseViewModel
    {
        const string TAG = "undefined";
        [ObservableProperty]
        private USDAFoodPreviewModel currentFoodPreview;
        [ObservableProperty]
        private USDAFoodNutritionFactModel currentFoodNutritionFact;
        [ObservableProperty]
        private UndefinedMacroFoodNutriFactModel currentUndefinedMacroFoodNutriFact;

        [ObservableProperty]
        private FoodNutrient proteinValue;
        [ObservableProperty]
        private FoodNutrient carbValue;
        [ObservableProperty]
        private FoodNutrient caloriesValue;
        [ObservableProperty]
        private FoodNutrient fatValue;

        [ObservableProperty]
        private bool isBottomSheetPresented;

        private NutritionMealLogDataStoreService foodLogService;

        public UsdaFoodFactDetailVM()
        {
            CurrentFoodNutritionFact = new USDAFoodNutritionFactModel();
        }

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
            this.IsLoading = true;
            foodLogService ??= ServicesHelper.GetService<NutritionMealLogDataStoreService>();
            if (!string.IsNullOrEmpty(this.CurrentFoodPreview?.Id))
            {
                if (this.CurrentFoodPreview.Id.Contains(TAG))
                {
                    this.CurrentUndefinedMacroFoodNutriFact = await dataService.GetMacroFoodNutriFacts(this.CurrentFoodPreview.Id);
                }
                else
                {
                    this.CurrentFoodNutritionFact = await ServicesHelper.GetService<USDAApiService>()
                    .GetFoodDetailsByIdAsync(this.CurrentFoodPreview.Id);
                    if (this.CurrentFoodNutritionFact?.foodNutrients?.Any() ?? false)
                    {
                        foreach (var i in this.CurrentFoodNutritionFact.foodNutrients)
                        {
                            if (this.CaloriesValue == null && i.Nutrient.Name.Contains(ConstantHelper.UsdaFoodNutrition.Energy))
                            {
                                this.CaloriesValue = i;
                            }
                            else if (this.ProteinValue == null && i.Nutrient.Name.Contains(ConstantHelper.UsdaFoodNutrition.Protein))
                            {
                                this.ProteinValue = i;
                            }
                            else if (CarbValue == null && i.Nutrient.Name.Contains(ConstantHelper.UsdaFoodNutrition.Carbohydrate)
                                && i.Nutrient.Name.Contains("difference"))
                            {
                                this.CarbValue = i;
                            }
                            else if (FatValue == null && i.Nutrient.Name.Contains(ConstantHelper.UsdaFoodNutrition.fat))
                            {
                                this.FatValue = i;
                            }

                            if (CarbValue != null
                                && this.CaloriesValue != null
                                && FatValue != null
                                && ProteinValue != null)
                            {
                                break;
                            }
                        }
                    }
                }
            }

            this.IsLoading = false;
            return base.ViewAppearingVM();
        }

        public override Task ViewDisappearingVM()
        {
            if (this.IsBottomSheetPresented)
            {
                IsBottomSheetPresented = false;
            }

            return base.ViewDisappearingVM();
        }

        //[RelayCommand]
        //private async Task MoreClicked()
        //{
        //}

        //[RelayCommand]
        //private void EatClicked()
        //{
        //    var today = DateTime.Now;
        //    var id = today.ToString("yyyyMMddHH");
        //    Console.WriteLine("==> eat clicked " + id);
        //}
    }
}
