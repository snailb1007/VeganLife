using VeganLife.Helpers;
using VeganLife.Views.MainPageFlyout.VitaminTab;

namespace VeganLife.ViewModels.TabsViewModel
{
    public partial class AthleticNutritionTabVM : BaseViewModel
    {
        private IEnumerable<AthleticNutritionModel> _allAthleticNutritions;

        [ObservableProperty]
        private ObservableCollection<AthleticNutritionModel> athleticNutritions;

        [ObservableProperty]
        private string athleticNutritionSearchText;

        public AthleticNutritionTabVM()
            : base()
        {
        }

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

            IsLoading = true;
            await this.navigationService.NavigateToPage<DetailAthleticNutritionPage>(param)
                .ConfigureAwait(false);
            IsLoading = false;
        }

        [RelayCommand]
        private void EnsureSearch()
        {
            if (string.IsNullOrEmpty(this.AthleticNutritionSearchText) || string.IsNullOrWhiteSpace(this.AthleticNutritionSearchText))
            {
                return;
            }

            IsLoading = true;
            var searchResult = SearchFoodByName(this._allAthleticNutritions.AsParallel(), AthleticNutritionSearchText);
            this.AthleticNutritions = new ObservableCollection<AthleticNutritionModel>(searchResult);
            IsLoading = false;
        }

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