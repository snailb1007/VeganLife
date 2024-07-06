// <copyright file="UsdaFoodFactDetailVM.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using VeganLife.Data.LocalData;
using VeganLife.Helpers;
using VeganLife.Helpers.AppSetting;
using VeganLife.Models.CommunityFreeServiceModel;
using VeganLife.Services.CommunityFreeService;

namespace VeganLife.ViewModels.ContentViewModels
{
    public partial class UsdaFoodFactDetailVM : BaseViewModel
    {
        private NutritionMealLogDataStoreService _foodLogService;

        [ObservableProperty]
        private USDAFoodPreviewModel currentFoodPreview;
        [ObservableProperty]
        private USDAFoodNutritionFactModel currentFoodNutritionFact;
        [ObservableProperty]
        private UndefinedMacroFoodNutriFactModel currentUndefinedMacroFoodNutriFact;

        [ObservableProperty]
        private bool isDataGridExpanded;

        // simplys
        [ObservableProperty]
        private UndefinedFoodNutrient proteinValue = null;

        [ObservableProperty]
        private UndefinedFoodNutrient carbValue;

        [ObservableProperty]
        private UndefinedFoodNutrient caloriesValue;

        public UsdaFoodFactDetailVM()
        {
            CurrentFoodNutritionFact = new USDAFoodNutritionFactModel();
        }

        public override Task OnNavigatingTo(object? parameter)
        {
            if (parameter != null)
            {
                this.CurrentFoodPreview = (USDAFoodPreviewModel)parameter;
                IsDataGridExpanded = string.IsNullOrEmpty(this.CurrentFoodPreview?.Image);
            }

            return base.OnNavigatingTo(parameter);
        }

        public async override Task ViewAppearingVM()
        {
            using (await this.loadingService.Show())
            {
                await base.ViewAppearingVM();
                _foodLogService ??= ServicesHelper.GetService<NutritionMealLogDataStoreService>();
                if (!string.IsNullOrEmpty(this.CurrentFoodPreview?.Id))
                {
                    if (this.CurrentFoodPreview.Id.Contains(ConstantHelper.TAG))
                    {
                        await ProcessUndefineFoodAsync().ConfigureAwait(false);
                    }
                    else
                    {
                        await ProcessUsdaFoodAsync().ConfigureAwait(false);
                    }
                }
            }
        }

        [RelayCommand]
        private async Task GoDetailVitaminAsync(string param)
        {
            if (GoDetailVitaminCommand.IsRunning || string.IsNullOrEmpty(param))
            {
                return;
            }

            using (await this.loadingService.Show())
            {
                await this.navigationService.PopToRootAsync();
                var rootVM = ServicesHelper.GetCurrentViewModel<NoteBookPageViewModel>();
                if (rootVM != null)
                {
                    rootVM.SelectedViewModelIndex = 1;
                    rootVM.VitaminAndMineralVM.VitaminSearchText = param;
                }
            }
        }

        [RelayCommand]
        private async Task ChangeDataGridExpandState()
        {
            using (await loadingService.Show(200))
            {
                IsDataGridExpanded = !IsDataGridExpanded;
            }
        }

        private async Task ProcessUndefineFoodAsync()
        {
            this.CurrentUndefinedMacroFoodNutriFact = await dataService.GetMacroFoodNutriFacts(this.CurrentFoodPreview.Id);
            if (this.CurrentUndefinedMacroFoodNutriFact?.foodNutrients?.Any() ?? false)
            {
                foreach (var i in this.CurrentUndefinedMacroFoodNutriFact.foodNutrients)
                {
                    if (ProteinValue == null && i.Nutrient.Name.Contains(ConstantHelper.UsdaFoodNutrition.Protein, StringComparison.OrdinalIgnoreCase))
                    {
                        ProteinValue = i;
                    }
                    else if (CarbValue == null && i.Nutrient.Name.Contains(ConstantHelper.UsdaFoodNutrition.Carbohydrate, StringComparison.OrdinalIgnoreCase))
                    {
                        CarbValue = i;
                    }
                    else if (CaloriesValue == null && i.Nutrient.Name.Contains(ConstantHelper.UsdaFoodNutrition.Energy, StringComparison.OrdinalIgnoreCase))
                    {
                        CaloriesValue = i;
                    }

                    if (ProteinValue != null
                        && CarbValue != null
                        && CaloriesValue != null)
                    {
                        break;
                    }
                }
            }
        }

        private UndefinedFoodNutrient _caloriesValue = null;
        private UndefinedFoodNutrient _proteinValue = null;
        private UndefinedFoodNutrient _carbValue = null;

        public async Task ProcessUsdaFoodAsync()
        {
            this.CurrentFoodNutritionFact = await ServicesHelper.GetService<USDAApiService>()
                    .GetFoodDetailsByIdAsync(this.CurrentFoodPreview.Id);
            if (this.CurrentFoodNutritionFact?.foodNutrients?.Any() ?? false)
            {
                foreach (var i in this.CurrentFoodNutritionFact.foodNutrients)
                {
                    SetNutrientValue(ref _proteinValue, i, [ConstantHelper.UsdaFoodNutrition.Protein]);
                    SetNutrientValue(ref _carbValue, i, [ConstantHelper.UsdaFoodNutrition.Carbohydrate, "difference"]);
                    SetNutrientValue(ref _caloriesValue, i, [ConstantHelper.UsdaFoodNutrition.Energy]);

                    if (_proteinValue != null
                        && _carbValue != null
                        && _caloriesValue != null)
                    {
                        break;
                    }
                }

                ProteinValue = _proteinValue;
                CarbValue = _carbValue;
                CaloriesValue = _caloriesValue;
                void SetNutrientValue(ref UndefinedFoodNutrient targetNutrient, FoodNutrient source, string[] searchTerms)
                {
                    var nutrientName = source.Nutrient?.Name;
                    bool isMatchesAllTerms = searchTerms.All(term => nutrientName.Contains(term, StringComparison.OrdinalIgnoreCase));
                    if (targetNutrient == null && isMatchesAllTerms)
                    {
                        targetNutrient = new UndefinedFoodNutrient()
                        {
                            Amount = source.Amount,
                            Unit = source?.Nutrient?.unitName,
                        };
                    }
                }
            }
        }
    }
}
