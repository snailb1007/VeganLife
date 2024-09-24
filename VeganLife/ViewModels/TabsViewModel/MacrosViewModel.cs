// <copyright file="MacrosViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using PropertyChanged;
using System.Text;
using VeganLife.Data.LocalData;
using VeganLife.Helpers;
using VeganLife.Models.CommunityFreeServiceModel;
using VeganLife.Resources.Translations;
using VeganLife.Services.CommunityFreeService;
using VeganLife.Views.ContentViews.Tabs;
using VeganLife.Views.MainPageFlyout;
using VeganLife.Views.PortionTab;

namespace VeganLife.ViewModels.TabsViewModel
{
    public partial class MacrosViewModel : BaseViewModel
    {
        [ObservableProperty]
        private bool _isFilterOpened;
        [ObservableProperty]
        private bool _isVeganSelected;
        [ObservableProperty]
        private bool _isUnVeganSelected;
        [ObservableProperty]
        private bool _isBannerClosed;

        [ObservableProperty]
        private ObservableCollection<USDAFoodPreviewModel> usdaFoodPreviews;
        [ObservableProperty]
        private string textSearch;
        [ObservableProperty]
        private USDAFoodPreviewModel usdaFoodPreviewCurrent;

        private List<USDAFoodPreviewModel> _allUSDAFoodPreview;

        public MacrosViewModel()
            : base()
        {
        }

        public override async Task<Task> ViewAppearingVM()
        {
            if (isInitialized)
            {
                return base.ViewAppearingVM();
            }

            var target = new FoodsListRequestModel
            {
                DataType = new List<string> { "Survey (FNDDS)" },
                PageSize = 10,
                PageNumber = 1,
                SortBy = "dataType.keyword",
                SortOrder = "desc",
            };
            var x = await ServicesHelper.GetService<USDAApiService>().GetFoodsListAsync(target);

            if (!_allUSDAFoodPreview?.Any() ?? true)
            {
                if (!_allUSDAFoodPreview?.Any() ?? true)
                {
                    var foodData = await this.dataService.GetFoodsUSDA();
                    this._allUSDAFoodPreview = new List<USDAFoodPreviewModel>(foodData);
                }

                App.Current?.MainPage?.Dispatcher?
                    .Dispatch(() => UsdaFoodPreviews = new ObservableCollection<USDAFoodPreviewModel>(_allUSDAFoodPreview ?? []));
            }

            isInitialized = true;
            return base.ViewAppearingVM();
        }

        [RelayCommand]
        private async Task ItemSelectedChanged(USDAFoodPreviewModel param)
        {
            try
            {
                if (ItemSelectedChangedCommand.IsRunning || IsLoading || param is null)
                {
                    return;
                }

                using (await this.loadingService.Show())
                {
                    await navigationService.NavigateToPage<UsdaFoodFactDetailPage>(param);
                }
            }
            finally
            {
                UsdaFoodPreviewCurrent = null!;
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
            var userTask = ServicesHelper.GetService<UserInfoDataStoreServie>().GetFirstOrDefaultItem();
            await Task.WhenAll(templateTask, userTask);
            var content = templateTask.Result.Replace("@@@username@@@", userTask?.Result?.Name);
            content = content.Replace("@@@content@@@", TextSearch);
            await ServicesHelper.GetService<IDeviceService>().SendEmailAsync("Support Request", content, new List<string> { "cskhveganlife@gmail.com" });
        }

        [RelayCommand]
        private async Task OpenAIConversation()
        {
            using (await this.loadingService.Show())
            {
                string query = AppResources.nutritionFact_foodDetail + " " + TextSearch;
                await Shell.Current.GoToAsync($"//chat?PassedData={query}");
            }
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

            using (await this.loadingService.Show())
            {
                await navigationService.NavigateToPage<USDAFoodListPage>();
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
            }
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
            if (categories != null && categories.Length > 0)
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
