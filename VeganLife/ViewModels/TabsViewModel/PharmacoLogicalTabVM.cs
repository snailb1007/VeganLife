using PropertyChanged;
using VeganLife.Helpers;
using VeganLife.Resources.Translations;
using VeganLife.Views.MainPageFlyout.VitaminTab;

namespace VeganLife.ViewModels.TabsViewModel
{
    public partial class PharmacoLogicalTabVM : BaseViewModel
    {
        private IEnumerable<PharmacoLogicalModel> _allPharmacoLogical;

        [ObservableProperty]
        private ObservableCollection<PharmacoLogicalModel> pharmacoLogicals;

        [ObservableProperty]
        private string pharmacoLogicalSearchText;

        [ObservableProperty]
        private bool isBannerClosed;

        public PharmacoLogicalTabVM()
            : base()
        {
        }

        public override async Task ViewAppearingVM()
        {
            if (!this.isInitialized)
            {
                this._allPharmacoLogical = await this.dataService.GetPharmacoLogical();
                this.PharmacoLogicals = new ObservableCollection<PharmacoLogicalModel>(this._allPharmacoLogical ?? []);
            }

            await base.ViewAppearingVM();
            this.isInitialized = true;
        }

        [RelayCommand]
        private async Task ItemSelectedAsync(PharmacoLogicalModel param)
        {
            if (param == null)
            {
                return;
            }

            using (await this.loadingService.Show())
            {
                await this.navigationService.NavigateToPage<DetailPharmacoLogicalPage>(param)
                .ConfigureAwait(false);
            }
        }

        [RelayCommand]
        private async Task EnsureSearch()
        {
            if (string.IsNullOrEmpty(this.PharmacoLogicalSearchText) || string.IsNullOrWhiteSpace(this.PharmacoLogicalSearchText))
            {
                return;
            }

            using (await this.loadingService.Show())
            {
                var searchResult = SearchFoodByName(this._allPharmacoLogical.AsParallel(), PharmacoLogicalSearchText);
                this.PharmacoLogicals = new ObservableCollection<PharmacoLogicalModel>(searchResult);
            }
        }

        [RelayCommand]
        private async Task OpenAIConversationAsync()
        {
            using (await this.loadingService.Show())
            {
                string query = string.Format(AppResources.firstQuery_vitaminPage, PharmacoLogicalSearchText);
                await Shell.Current.GoToAsync($"//chat?PassedData={query}");
            }
        }

        [RelayCommand]
        private void CloseBanner()
        {
            this.IsBannerClosed = true;
        }

        private ParallelQuery<PharmacoLogicalModel> SearchFoodByName(ParallelQuery<PharmacoLogicalModel> vitamins, string name)
        {
            // Normalize input name to support UTF-8 and improve search accuracy
            var normalizedNames = name.NormalizeStringAndSplit();
            return vitamins.Where(item => normalizedNames
                .Any(normalizedName => item.Id.NormalizeString().Contains(normalizedName)));
        }

        [SuppressPropertyChangedWarnings]
        partial void OnPharmacoLogicalSearchTextChanged(string value)
        {
            if (string.IsNullOrEmpty(value)
                || string.IsNullOrWhiteSpace(value))
            {
                PharmacoLogicals = new ObservableCollection<PharmacoLogicalModel>(this._allPharmacoLogical);
            }
        }
    }
}