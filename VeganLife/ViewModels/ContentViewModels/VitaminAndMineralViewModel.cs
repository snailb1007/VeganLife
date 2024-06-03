// <copyright file="VitaminAndMineralViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Text;
using VeganLife.Models.CommunityFreeServiceModel;
using VeganLife.Resources.Translations;
using VeganLife.Views.MainPageFlyout.VitaminTab;

namespace VeganLife.ViewModels.ContentViewModels
{
    [QueryProperty(nameof(PassedData), nameof(PassedData))]
    public partial class VitaminAndMineralViewModel : BaseViewModel
    {
        [ObservableProperty]
        private ObservableCollection<VitaminModel> vitamins;

        [ObservableProperty]
        private string passedData;

        [ObservableProperty]
        private string searchText;

        private IEnumerable<VitaminModel> _allVitamins;

        public VitaminAndMineralViewModel()
            : base()
        {
        }

        public override async Task ViewAppearingVM()
        {
            if (!this.isInitialized)
            {
                this._allVitamins = await this.dataService.GetVitamins().ConfigureAwait(false);
                this.Vitamins = new ObservableCollection<VitaminModel>(this._allVitamins ?? []);
            }

            await base.ViewAppearingVM();
            this.isInitialized = true;
        }

        [RelayCommand]
        private async Task ItemSelectedAsync(VitaminModel param)
        {
            if (param == null)
            {
                return;
            }

            IsLoading = true;
            await this.navigationService.NavigateToPage<DetailVitaminAndMineralPage>(param)
                .ConfigureAwait(false);
            IsLoading = false;
        }

        [RelayCommand]
        private void EnsureSearch()
        {
            if (string.IsNullOrEmpty(this.SearchText) || string.IsNullOrWhiteSpace(this.SearchText))
            {
                return;
            }

            IsLoading = true;
            var searchResult = SearchFoodByName(this.Vitamins.AsParallel(), SearchText);
            this.Vitamins = new ObservableCollection<VitaminModel>(searchResult);
            IsLoading = false;
        }

        [RelayCommand]
        private async Task OpenAIConversationAsync()
        {
            IsLoading = true;
            string query = string.Format(AppResources.firstQuery_vitaminPage, SearchText);
            await Shell.Current.GoToAsync($"//chat?PassedData={query}");
            IsLoading = false;
        }

        partial void OnPassedDataChanged(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            _ = navigationService.PopToRootAsync();
            SearchText = value;
            PassedData = string.Empty;
        }

        partial void OnSearchTextChanged(string value)
        {
            if (string.IsNullOrEmpty(value)
                || string.IsNullOrWhiteSpace(value))
            {
                Vitamins = new ObservableCollection<VitaminModel>(this._allVitamins);
            }
        }

        private ParallelQuery<VitaminModel> SearchFoodByName(ParallelQuery<VitaminModel> vitamins, string name)
        {
            // Normalize input name to support UTF-8 and improve search accuracy
            var normalizedName = NormalizeString(name);
            return vitamins.Where(item => NormalizeString(item.Id).Contains(normalizedName)
            || NormalizeString(item.VietnameseName).Contains(normalizedName));

            string NormalizeString(string input)
            {
                return input.Normalize(NormalizationForm.FormKD).ToLower().Trim();
            }
        }
    }
}
