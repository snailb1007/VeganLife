using Android.Webkit;
using System.Text;
using System.Text.RegularExpressions;
using VeganLife.Data.LocalData;
using VeganLife.Helpers;
using VeganLife.Models.CommunityFreeServiceModel;
using VeganLife.Views.ContentViews.Tabs;
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
        private ObservableCollection<USDAFoodPreviewModel> usdaFoodPreviews;
        [ObservableProperty]
        private string textSearch;
        [ObservableProperty]
        private USDAFoodPreviewModel usdaFoodPreviewCurrent;

        private List<USDAFoodPreviewModel> allUSDAFoodPreview;
        //private IEnumerable<USDAFoodPreviewModel> passedFoodFilter;
        public MacrosViewModel()
            : base()
        {
        }

        public override async Task<Task> ViewAppearingVM()
        {
            if (!allUSDAFoodPreview?.Any() ?? true)
            {
                if (!allUSDAFoodPreview?.Any() ?? true)
                {
                    var foodData = await this.dataService.GetFoodsUSDA();
                    this.allUSDAFoodPreview = new List<USDAFoodPreviewModel>(foodData);
                }

                App.Current?.MainPage?.Dispatcher?
                    .Dispatch(() => UsdaFoodPreviews = new ObservableCollection<USDAFoodPreviewModel>
                    (allUSDAFoodPreview ?? new List<USDAFoodPreviewModel>()));
            }

            return base.ViewAppearingVM();
        }

        [RelayCommand]
        private async Task ItemSelectedChanged(USDAFoodPreviewModel data)
        {
            if (ItemSelectedChangedCommand.IsRunning || IsLoading || data is null)
                return;
            IsLoading = true;
            await navigationService.NavigateToPage<UsdaFoodFactDetailPage>(data);
            IsLoading = false;
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
            var userTask = ServicesHelper.GetService<UserInfoDataStoreServie>().GetItemAsync();
            await Task.WhenAll(templateTask, userTask);
            var content = templateTask.Result.Replace("@@@username@@@", userTask?.Result?.Name);
            content = content.Replace("@@@content@@@", TextSearch);
            await ServicesHelper.GetService<IDeviceService>().SendEmailAsync("Support Request", content, new List<string> { "cskhveganlife@gmail.com" });
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

        partial void OnIsVeganSelectedChanged(bool value)
        {
            if (value)
            {
                IsUnVeganSelected = false;
            }

            this.UsdaFoodPreviews = new ObservableCollection<USDAFoodPreviewModel>(GetFoodsFilter());
        }

        partial void OnIsUnVeganSelectedChanged(bool value)
        {
            if (value)
            {
                IsVeganSelected = false;
            }

            this.UsdaFoodPreviews = new ObservableCollection<USDAFoodPreviewModel>(GetFoodsFilter());
        }

        private IEnumerable<USDAFoodPreviewModel> GetFoodsFilter(string[] categories = null)
        {
            // Start with all food previews
            var filteredFoods = allUSDAFoodPreview.AsParallel();

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
