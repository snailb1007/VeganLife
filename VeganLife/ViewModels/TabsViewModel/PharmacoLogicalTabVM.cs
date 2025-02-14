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
        private ObservableCollection<PharmacoLogicalModel> _pharmacoLogicals;

        [ObservableProperty]
        private string _pharmacoLogicalSearchText;

        [ObservableProperty]
        private bool _isBannerClosed;

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

            await this.navigationService.NavigateToPage<DetailPharmacoLogicalPage>(paramater: param);
        }

        [RelayCommand]
        private void EnsureSearch()
        {
            if (string.IsNullOrEmpty(this.PharmacoLogicalSearchText) || string.IsNullOrWhiteSpace(this.PharmacoLogicalSearchText))
            {
                return;
            }

            var searchResult = SearchFoodByName(this._allPharmacoLogical.AsParallel(), PharmacoLogicalSearchText);
            this.PharmacoLogicals = new ObservableCollection<PharmacoLogicalModel>(searchResult);
        }

        [RelayCommand]
        private async Task OpenAIConversationAsync()
        {
            string query = string.Format(AppResources.firstQuery_vitaminPage, PharmacoLogicalSearchText);
            await Shell.Current.GoToAsync($"//chat?PassedData={query}");
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