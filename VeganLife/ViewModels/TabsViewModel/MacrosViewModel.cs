// <copyright file="MacrosViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using PropertyChanged;
using System.Text;
using VeganLife.Data;
using VeganLife.Data.LocalData;
using VeganLife.Handlers;
using VeganLife.Helpers;
using VeganLife.Models.CommunityFreeServiceModel;
using VeganLife.Resources.Translations;
using VeganLife.Services.CommunityFreeService;
using VeganLife.Views.ContentViews.Tabs;
using VeganLife.Views.MainPageFlyout;
using VeganLife.Views.MainPageFlyout.PortionTab;

namespace VeganLife.ViewModels.TabsViewModel
{
    public partial class MacrosViewModel : BaseViewModel
    {
        private readonly BaseDataStore<USDAFoodNutritionFactModel> _usdaFoodNutritionFactDataStoreService;
        private readonly BaseDataStore<UndefinedMacroFoodNutriFactModel> _undefinedMacroFoodNutriFactDataStoreService;
        private readonly BaseDataStore<NutritionMealLogModel> _nutritionMealLogDataStoreService;
        private readonly USDAApiService _uSDAApiService;

        private List<USDAFoodPreviewModel> _allUSDAFoodPreview;

        [ObservableProperty]
        private bool _isFilterOpened;

        [ObservableProperty]
        private bool _isVeganSelected;

        [ObservableProperty]
        private bool _isUnVeganSelected;

        [ObservableProperty]
        private bool _isBannerClosed;

        [ObservableProperty]
        private bool _isScrolling;

        [ObservableProperty]
        private ObservableCollection<USDAFoodPreviewModel> _usdaFoodPreviews;

        [ObservableProperty]
        private string _textSearch;

        [ObservableProperty]
        private USDAFoodPreviewModel _usdaFoodPreviewCurrent;

        public MacrosViewModel(LocalDataStoreFactory localDataStoreFactory)
            : base()
        {
            this._usdaFoodNutritionFactDataStoreService = localDataStoreFactory.GetDataStore<USDAFoodNutritionFactModel>();
            this._undefinedMacroFoodNutriFactDataStoreService = localDataStoreFactory.GetDataStore<UndefinedMacroFoodNutriFactModel>();
            this._nutritionMealLogDataStoreService = localDataStoreFactory.GetDataStore<NutritionMealLogModel>();
            this._uSDAApiService = FFImageLoading.Helpers.ServiceHelper.GetService<USDAApiService>();
        }

        public override async Task<Task> ViewAppearingVM()
        {
            if (isInitialized)
            {
                return base.ViewAppearingVM();
            }

            if (!_allUSDAFoodPreview?.Any() ?? true)
            {
                if (!_allUSDAFoodPreview?.Any() ?? true)
                {
                    var foodData = await this.dataService.GetFoodsUSDA();
                    this._allUSDAFoodPreview = [..foodData];
                }

                Application.Current?.Windows[0]?.Page?.Dispatcher?
                    .Dispatch(() => UsdaFoodPreviews = new ObservableCollection<USDAFoodPreviewModel>(_allUSDAFoodPreview ?? []));
            }

            isInitialized = true;
            return base.ViewAppearingVM();
        }

        [RelayCommand]
        private async Task ItemSelectedChanged(USDAFoodPreviewModel param)
        {
            if (ItemSelectedChangedCommand.IsRunning
                || IsLoading
                || param is null)
            {
                return;
            }

            if (param.IsShowingEdit)
            {
                param.IsShowingEdit = false;
                return;
            }

            try
            {
                this.busyManager.Increase();
                await navigationService.NavigateToPage<UsdaFoodFactDetailPage>(paramater: param);
            }
            finally
            {
                UsdaFoodPreviewCurrent = null!;
                this.busyManager.Decrease();
            }
        }

        [RelayCommand]
        private void EnsureSearch()
        {
            this.UsdaFoodPreviews = new ObservableCollection<USDAFoodPreviewModel>(GetFoodsFilter());
        }

        [RelayCommand]
        private void OnFilter()
        {
            IsFilterOpened = !IsFilterOpened;
        }

        [RelayCommand]
        private async Task OnSupportRequest()
        {
            var templateTask = ResourceReader.ReadTextFileAsync("VeganLife.Resources.Raw.mail_template.txt");
            var userTask = FFImageLoading.Helpers.ServiceHelper.GetService<LocalDataStoreFactory>().GetDataStore<UserInfo>().GetFirstOrDefaultItem();
            await Task.WhenAll(templateTask, userTask);
            var content = templateTask.Result.Replace("@@@username@@@", userTask?.Result?.Name);
            content = content.Replace("@@@content@@@", TextSearch);
            await FFImageLoading.Helpers.ServiceHelper.GetService<IDeviceService>().SendEmailAsync("Support Request", content, new List<string> { "cskhveganlife@gmail.com" });
        }

        [RelayCommand]
        private async Task OpenAIConversation()
        {
            string query = AppResources.nutritionFact_foodDetail + " " + TextSearch;
            await Shell.Current.GoToAsync($"//chat?PassedData={query}");
        }

        [RelayCommand]
        private void CloseAdBanner()
        {
            IsBannerClosed = true;
        }

        [RelayCommand]
        private async Task OnUSDABannerClicked()
        {
            if (this.USDABannerClickedCommand.IsRunning)
            {
                return;
            }

            await navigationService.NavigateToPage<USDAFoodListPage>();
        }

        bool _isProcessing;
        [RelayCommand]
        private void OnItemEditedTap(USDAFoodPreviewModel param)
        {
            if (IsLoading || _isProcessing)
            {
                return;
            }

            _isProcessing = true;
            param.IsShowingEdit = !param.IsShowingEdit;
            Task.Delay(100).ContinueWith(t => _isProcessing = false);
        }

        [RelayCommand]
        private async Task OnAddMealLogsClickedAsync(USDAFoodPreviewModel param)
        {
            busyManager.Increase();
            var nutritionMealLogModel = new NutritionMealLogModel
            {
                EatingDay = DateTime.Now.Date,
                Amount = param.Amount,
                Name = param.Name
            };
            var mealLogs = await _nutritionMealLogDataStoreService.GetItemsAsync();
            NutritionMealLogModel mealTargetItem;

            var (itemExists, targetItem) = await GetFoodDetailsAsync(param.IsUSDAFood, param.Id);

            nutritionMealLogModel.CaloriesAmount = targetItem.CaloriesAmount;
            nutritionMealLogModel.ProteinAmount = targetItem.ProteinAmount;
            nutritionMealLogModel.CarbohydrateAmount = targetItem.CarbohydrateAmount;
            nutritionMealLogModel.FatAmount = targetItem.FatAmount;

            if (param.IsUSDAFood)
            {
                nutritionMealLogModel.UsdaFoodId = targetItem.Id;
            }
            else
            {
                nutritionMealLogModel.UndefinedFoodId = targetItem.Id;
            }

            mealTargetItem = mealLogs.FirstOrDefault(x => x.EatingDay == nutritionMealLogModel.EatingDay &&
                     ((param.IsUSDAFood && x.UsdaFoodId == nutritionMealLogModel.UsdaFoodId) ||
                     (!param.IsUSDAFood && x.UndefinedFoodId == nutritionMealLogModel.UndefinedFoodId)));

            if (mealTargetItem != null)
            {
                mealTargetItem.Amount += nutritionMealLogModel.Amount;
                await _nutritionMealLogDataStoreService.AddOrUpdateItemAsync(mealTargetItem, isUpdate: true);
            }
            else
            {
                await _nutritionMealLogDataStoreService.AddOrUpdateItemAsync(nutritionMealLogModel);
            }
#if ANDROID
            (AppShell.Current.Handler as ShellHandler).ChangeBageInfo(1);
#endif
            busyManager.Decrease();

            async Task<(bool, dynamic)> GetFoodDetailsAsync(bool isUSDAFood, string id)
            {
                if (isUSDAFood)
                {
                    var result = await _usdaFoodNutritionFactDataStoreService.IsExistingItem(id);
                    return result.isExised
                        ? (true, result.result)
                        : (false, await _uSDAApiService.GetFoodDetailsByIdAsync(id));
                }
                else
                {
                    var result = await _undefinedMacroFoodNutriFactDataStoreService.IsExistingItem(id);
                    return result.isExised
                        ? (true, result.result)
                        : (false, await dataService.GetMacroFoodNutriFacts(id));
                }
            }
        }

        [SuppressPropertyChangedWarnings]
        partial void OnIsVeganSelectedChanged(bool value)
        {
            if (value)
            {
                IsUnVeganSelected = false;
            }

            this.UsdaFoodPreviews = new ObservableCollection<USDAFoodPreviewModel>(GetFoodsFilter());
        }

        [SuppressPropertyChangedWarnings]
        partial void OnIsUnVeganSelectedChanged(bool value)
        {
            if (value)
            {
                IsVeganSelected = false;
            }

            this.UsdaFoodPreviews = new ObservableCollection<USDAFoodPreviewModel>(GetFoodsFilter());
        }

        [SuppressPropertyChangedWarnings]
        partial void OnTextSearchChanged(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                UsdaFoodPreviews = new ObservableCollection<USDAFoodPreviewModel>(GetFoodsFilter());
                return;
            }

            Task.Delay(200).ContinueWith(t => EnsureSearch());
        }

        internal void ScrollToTop()
        {
            var currentShoTab = Shell.Current.CurrentPage.FindByName("Tab1");
            var collection = (currentShoTab as Sharpnado.Tabs.DelayedView<MacrosTab>)?.Content?.FindByName("FoodsPreviewCollection")!;
            if (collection != null)
            {
                (collection as CollectionView)?.ScrollTo(UsdaFoodPreviews?.FirstOrDefault(), animate: false);
            }
        }

        private IEnumerable<USDAFoodPreviewModel> GetFoodsFilter(string[] categories = null!)
        {
            // Start with all food previews
            var filteredFoods = _allUSDAFoodPreview.AsParallel();

            // Filter by vegan status
            if (IsVeganSelected || IsUnVeganSelected)
            {
                bool veganStatus = IsVeganSelected;  // True if vegan, false if unvegan
                filteredFoods = filteredFoods.Where(food => food.IsPlantOrigin == veganStatus);
            }

            // Filter by search text if not empty
            if (!string.IsNullOrEmpty(TextSearch) && !string.IsNullOrWhiteSpace(TextSearch))
            {
                filteredFoods = SearchFoodByName(filteredFoods, TextSearch);
            }

            // Filter by categories if specified
            if (categories is { Length: > 0 })
            {
                // Convert categories to a hash set for efficient lookup
                HashSet<string> categorySet = new HashSet<string>(categories);
                filteredFoods = filteredFoods.Where(food => categorySet.Contains(food.Category));
            }

            return filteredFoods;
        }

        private ParallelQuery<USDAFoodPreviewModel> SearchFoodByName(ParallelQuery<USDAFoodPreviewModel> uSDAFoods, string name)
        {
            // Normalize input name to support UTF-8 and improve search accuracy
            var normalizedName = NormalizeString(name);
            return uSDAFoods.Where(item => NormalizeString(item.Name).Contains(normalizedName));
        }

        private string NormalizeString(string input)
        {
            return input.Normalize(NormalizationForm.FormKD).ToLower().Trim();
        }
    }
}
