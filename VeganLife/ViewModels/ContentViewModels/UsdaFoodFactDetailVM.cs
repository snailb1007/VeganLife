// <copyright file="UsdaFoodFactDetailVM.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AsyncAwaitBestPractices;
using VeganLife.Data.LocalData;
using VeganLife.Helpers;
using VeganLife.Helpers.AppSetting;
using VeganLife.Models.CommunityFreeServiceModel;
using VeganLife.Services.CommunityFreeService;

namespace VeganLife.ViewModels.ContentViewModels
{
    public partial class UsdaFoodFactDetailVM : BaseViewModel
    {
        private readonly USDAApiService _usdaApiService;
        private readonly NutritionMealLogDataStoreService _foodLogService;

        [ObservableProperty]
        private USDAFoodPreviewModel currentFoodPreview;

        [ObservableProperty]
        private USDAFoodNutritionFactModel currentFoodNutritionFact;

        [ObservableProperty]
        private UndefinedMacroFoodNutriFactModel currentUndefinedMacroFoodNutriFact;

        [ObservableProperty]
        private bool isDataGridExpanded;

        [ObservableProperty]
        private List<AffiliationModel> affiliations;

        // simplys
        [ObservableProperty]
        private UndefinedFoodNutrient proteinValue = null;

        [ObservableProperty]
        private UndefinedFoodNutrient carbValue;

        [ObservableProperty]
        private UndefinedFoodNutrient caloriesValue;

        public UsdaFoodFactDetailVM(USDAApiService uSDAApiService, NutritionMealLogDataStoreService nutritionMealLogDataStoreService)
        {
            CurrentFoodNutritionFact = new USDAFoodNutritionFactModel();
            _foodLogService = nutritionMealLogDataStoreService;
            _usdaApiService = uSDAApiService;
        }

        public override Task OnNavigatingTo(object? parameter)
        {
            if (parameter != null)
            {
                this.CurrentFoodPreview = (USDAFoodPreviewModel)parameter;
                IsDataGridExpanded = string.IsNullOrEmpty(this.CurrentFoodPreview?.Image);
                var nameNormal = this.CurrentFoodPreview?.Name?.RemoveNestedParentheses() ?? string.Empty;
                Affiliations = (StaticHelper.Affiliation.Affiliations
                    .Where(i => i.NutrientName == nameNormal || nameNormal.ToLower().Contains(i.NutrientName.ToLower()))
                        ?? Enumerable.Empty<AffiliationModel>()).ToList();
            }

            return base.OnNavigatingTo(parameter);
        }

        public async override Task ViewAppearingVM()
        {
            using (await this.loadingService.Show())
            {
                await base.ViewAppearingVM();
                if (!string.IsNullOrEmpty(this.CurrentFoodPreview?.Id))
                {
                    if (this.CurrentFoodPreview.Id.Contains(ConstantHelper.TAG))
                    {
                        await ProcessUndefineFoodAsync();
                    }
                    else
                    {
                        await ProcessUsdaFoodAsync();
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
                var rootVM = ServicesHelper.GetCurrentViewModel() as NoteBookPageViewModel;
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

        private UndefinedFoodNutrient? _caloriesValue = null;
        private UndefinedFoodNutrient? _proteinValue = null;
        private UndefinedFoodNutrient? _carbValue = null;

        private async Task ProcessUsdaFoodAsync()
        {
            this.CurrentFoodNutritionFact = await _usdaApiService.GetFoodDetailsByIdAsync(this.CurrentFoodPreview.Id);
            if (this.CurrentFoodNutritionFact?.foodNutrients?.Any() ?? false)
            {
                // try summarize usda food nutrients
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
                void SetNutrientValue(ref UndefinedFoodNutrient? targetNutrient, FoodNutrient source, string[] searchTerms)
                {
                    var nutrientName = source.Nutrient?.Name;
                    bool isMatchesAllTerms = searchTerms
                        .All(term => !string.IsNullOrEmpty(nutrientName) && nutrientName.Contains(term, StringComparison.OrdinalIgnoreCase));
                    if (targetNutrient == null && isMatchesAllTerms)
                    {
                        targetNutrient = new UndefinedFoodNutrient()
                        {
                            Amount = source.Amount,
                            Unit = source?.Nutrient?.UnitName ?? string.Empty,
                        };
                    }
                }
            }
        }
    }
}
