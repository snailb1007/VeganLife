// <copyright file="VitaminAndMineralViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using VeganLife.Helpers;
using VeganLife.Resources.Translations;
using VeganLife.Views.MainPageFlyout.VitaminTab;

namespace VeganLife.ViewModels
{
    public partial class VitaminAndMineralViewModel : BaseViewModel
    {
        private IEnumerable<VitaminModel> _allVitamins;

        [ObservableProperty]
        private ObservableCollection<VitaminModel> vitamins;

        [ObservableProperty]
        private string vitaminSearchText;

        [ObservableProperty]
        private bool isBannerClosed = false;

        public VitaminAndMineralViewModel()
            : base()
        {
        }

        public override async Task ViewAppearingVM()
        {
            if (!this.isInitialized)
            {
                this._allVitamins = await this.dataService.GetVitamins();
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
            if (string.IsNullOrEmpty(this.VitaminSearchText) || string.IsNullOrWhiteSpace(this.VitaminSearchText))
            {
                return;
            }

            IsLoading = true;
            var searchResult = SearchFoodByName(this._allVitamins.AsParallel(), VitaminSearchText);
            this.Vitamins = new ObservableCollection<VitaminModel>(searchResult);
            IsLoading = false;
        }

        [RelayCommand]
        private async Task OpenAIConversationAsync()
        {
            IsLoading = true;
            string query = string.Format(AppResources.firstQuery_vitaminPage, VitaminSearchText);
            await Shell.Current.GoToAsync($"//chat?PassedData={query}");
            IsLoading = false;
        }

        [RelayCommand]
        private void CloseBanner()
        {
            this.IsBannerClosed = true;
        }

        partial void OnVitaminSearchTextChanged(string value)
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
            var normalizedNames = name.NormalizeStringAndSplit();
            return vitamins.Where(item => normalizedNames
                .Any(normalizedName => item.Id.NormalizeString().Contains(normalizedName)
                    || item.VietnameseName.NormalizeString().Contains(normalizedName)));
        }
    }
}