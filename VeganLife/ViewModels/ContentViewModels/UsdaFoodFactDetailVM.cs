// <copyright file="UsdaFoodFactDetailVM.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using VeganLife.Data;
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
        private readonly BaseDataStore<USDAFoodNutritionFactModel> _usdaDataStoreService;

        [ObservableProperty]
        private USDAFoodPreviewModel _currentFoodPreview;

        [ObservableProperty]
        private USDAFoodNutritionFactModel _currentFoodNutritionFact;

        [ObservableProperty]
        private UndefinedMacroFoodNutriFactModel _currentUndefinedMacroFoodNutriFact;

        [ObservableProperty]
        private bool _isDataGridExpanded;

        [ObservableProperty]
        private List<AffiliationModel> _affiliations;

        // simplys
        [ObservableProperty]
        private UndefinedFoodNutrient proteinValue = null!;

        [ObservableProperty]
        private UndefinedFoodNutrient carbValue;

        [ObservableProperty]
        private UndefinedFoodNutrient caloriesValue;

        public UsdaFoodFactDetailVM(USDAApiService uSDAApiService, LocalDataStoreFactory localDataStoreFactory)
        {
            CurrentFoodNutritionFact = new USDAFoodNutritionFactModel();
            _usdaApiService = uSDAApiService;
            _usdaDataStoreService = localDataStoreFactory.GetDataStore<USDAFoodNutritionFactModel>();
        }

        public override Task OnNavigatingTo(object parameter)
        {
            if (parameter == null)
            {
                return base.OnNavigatingTo(null);
            }

            this.CurrentFoodPreview = (USDAFoodPreviewModel)parameter;
            IsDataGridExpanded = string.IsNullOrEmpty(this.CurrentFoodPreview?.Image);
            var nameNormal = this.CurrentFoodPreview?.Name?.RemoveNestedParentheses() ?? string.Empty;
            Affiliations = (StaticHelper.Affiliation.Affiliations
                                .Where(i => i.NutrientName == nameNormal || nameNormal.ToLower().Contains(i.NutrientName.ToLower()))
                            ?? []).ToList();

            return base.OnNavigatingTo(parameter);
        }

        public override async Task ViewAppearingVM()
        {
            this.busyManager.Increase();
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

            this.busyManager.Decrease();
        }

        [RelayCommand]
        private async Task GoDetailVitaminAsync(string param)
        {
            if (GoDetailVitaminCommand.IsRunning || string.IsNullOrEmpty(param))
            {
                return;
            }

            await this.navigationService.PopToRootAsync();
            if (ServicesHelper.GetCurrentViewModel() is NoteBookPageViewModel rootVm)
            {
                rootVm.SelectedViewModelIndex = 1;
                rootVm.VitaminAndMineralVM.VitaminSearchText = param;
            }
        }

        [RelayCommand]
        private void ChangeDataGridExpandState()
        {
            this.busyManager.Increase();
            IsDataGridExpanded = !IsDataGridExpanded;
            _ = Task.Delay(200).ContinueWith(t => this.busyManager.Decrease());
        }

        private async Task ProcessUndefineFoodAsync()
        {
            this.CurrentUndefinedMacroFoodNutriFact = await dataService.GetMacroFoodNutriFacts(this.CurrentFoodPreview.Id);
            bool? any = (this.CurrentUndefinedMacroFoodNutriFact?.foodNutrients!).Any();

            if (any ?? false)
            {
                foreach (var i in this.CurrentUndefinedMacroFoodNutriFact?.foodNutrients!)
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

        private async Task ProcessUsdaFoodAsync()
        {
            this.CurrentFoodNutritionFact = await _usdaApiService.GetFoodDetailsByIdAsync(this.CurrentFoodPreview.Id);
            //bool? any = false;
            var foodNutrients = this.CurrentFoodNutritionFact?.foodNutrients;
            if (foodNutrients != null)
            {
                if (!foodNutrients.Any())
                {
                    return;
                }

                var x = new Stopwatch();
                x.Start();

                // try summarize usda food nutrients
                foreach (var i in foodNutrients)
                {
                    NutritionFactsHelper.SetNutrientValue(ref _proteinValue, i, [ConstantHelper.UsdaFoodNutrition.Protein]);
                    NutritionFactsHelper.SetNutrientValue(ref _carbValue, i,
                        [ConstantHelper.UsdaFoodNutrition.Carbohydrate, "difference"]);
                    NutritionFactsHelper.SetNutrientValue(ref _caloriesValue, i, [ConstantHelper.UsdaFoodNutrition.Energy]);

                    if (_proteinValue != null
                        && _carbValue != null
                        && _caloriesValue != null)
                    {
                        break;
                    }
                }

                x.Stop();
                Console.WriteLine("==> time run: " + x.ElapsedMilliseconds + " ms.");

                bool isNeedUpdate = false;
                if (_proteinValue != null)
                {
                    ProteinValue = _proteinValue;
                    this.CurrentFoodNutritionFact.Protein = _proteinValue?.Amount.Value ?? -1;
                    isNeedUpdate = true;
                }

                if (_carbValue != null)
                {
                    CarbValue = _carbValue;
                    this.CurrentFoodNutritionFact.Carbohydrate = _carbValue?.Amount.Value ?? -1;
                    isNeedUpdate = true;
                }

                if (_caloriesValue != null)
                {
                    CaloriesValue = _caloriesValue;
                    this.CurrentFoodNutritionFact.Calories = _caloriesValue?.Amount.Value ?? -1;
                    isNeedUpdate = true;
                }

                if (isNeedUpdate)
                {
                    await _usdaDataStoreService.AddOrUpdateItemAsync(this.CurrentFoodNutritionFact, isUpdate: true);
                }
            }
        }
    }
}
