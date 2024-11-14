using PropertyChanged;
using VeganLife.Helpers;
using VeganLife.Views.MainPageFlyout.VitaminTab;

namespace VeganLife.ViewModels.TabsViewModel
{
    public partial class AthleticNutritionTabVM : BaseViewModel
    {
        private IEnumerable<AthleticNutritionModel> _allAthleticNutritions;

        [ObservableProperty]
        private bool _isBannerClosed;

        [ObservableProperty]
        private ObservableCollection<AthleticNutritionModel> _athleticNutritions;

        [ObservableProperty]
        private string _athleticNutritionSearchText;

        public override async Task ViewAppearingVM()
        {
            if (!this.isInitialized)
            {
                this._allAthleticNutritions = await this.dataService.GetAthleticNutritions();
                this.AthleticNutritions = new ObservableCollection<AthleticNutritionModel>(this._allAthleticNutritions ?? []);
            }

            await base.ViewAppearingVM();
            this.isInitialized = true;
        }

        [RelayCommand]
        private async Task ItemSelectedAsync(AthleticNutritionModel param)
        {
            if (param == null)
            {
                return;
            }

            await this.navigationService.NavigateToPage<DetailAthleticNutritionPage>(param);
        }

        [RelayCommand]
        private void EnsureSearch()
        {
            if (string.IsNullOrEmpty(this.AthleticNutritionSearchText) || string.IsNullOrWhiteSpace(this.AthleticNutritionSearchText))
            {
                return;
            }

            var searchResult = SearchFoodByName(this._allAthleticNutritions.AsParallel(), AthleticNutritionSearchText);
            this.AthleticNutritions = new ObservableCollection<AthleticNutritionModel>(searchResult);
        }

        [RelayCommand]
        private void CloseAdBanner()
        {
            IsBannerClosed = true;
        }

        [SuppressPropertyChangedWarnings]
        partial void OnAthleticNutritionSearchTextChanged(string value)
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
            {
                this.AthleticNutritions = new ObservableCollection<AthleticNutritionModel>(this._allAthleticNutritions ?? []);
            }
        }

        private ParallelQuery<AthleticNutritionModel> SearchFoodByName(ParallelQuery<AthleticNutritionModel> vitamins, string name)
        {
            // Normalize input name to support UTF-8 and improve search accuracy
            var normalizedNames = name.NormalizeStringAndSplit();
            return vitamins.Where(item => normalizedNames
                .Any(normalizedName => item.Id.NormalizeString().Contains(normalizedName)));
        }
    }
}