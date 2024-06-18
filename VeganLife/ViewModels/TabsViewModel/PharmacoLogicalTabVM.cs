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

            IsLoading = true;
            await this.navigationService.NavigateToPage<DetailPharmacoLogicalPage>(param)
                .ConfigureAwait(false);
            IsLoading = false;
        }

        [RelayCommand]
        private void EnsureSearch()
        {
            if (string.IsNullOrEmpty(this.PharmacoLogicalSearchText) || string.IsNullOrWhiteSpace(this.PharmacoLogicalSearchText))
            {
                return;
            }

            IsLoading = true;
            var searchResult = SearchFoodByName(this._allPharmacoLogical.AsParallel(), PharmacoLogicalSearchText);
            this.PharmacoLogicals = new ObservableCollection<PharmacoLogicalModel>(searchResult);
            IsLoading = false;
        }

        [RelayCommand]
        private async Task OpenAIConversationAsync()
        {
            IsLoading = true;
            string query = string.Format(AppResources.firstQuery_vitaminPage, PharmacoLogicalSearchText);
            await Shell.Current.GoToAsync($"//chat?PassedData={query}");
            IsLoading = false;
        }

        private ParallelQuery<PharmacoLogicalModel> SearchFoodByName(ParallelQuery<PharmacoLogicalModel> vitamins, string name)
        {
            // Normalize input name to support UTF-8 and improve search accuracy
            var normalizedNames = name.NormalizeStringAndSplit();
            return vitamins.Where(item => normalizedNames
                .Any(normalizedName => item.Id.NormalizeString().Contains(normalizedName)));
        }

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