// <copyright file="VitaminAndMineralViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using VeganLife.Views.MainPageFlyout.VitaminTab;

namespace VeganLife.ViewModels.ContentViewModels
{
    [QueryProperty(nameof(PassedData), nameof(PassedData))]
    public partial class VitaminAndMineralViewModel : BaseViewModel
    {
        [ObservableProperty]
        private IEnumerable<VitaminModel> vitamins;

        [ObservableProperty]
        private string passedData;

        public VitaminAndMineralViewModel()
            : base()
        {
        }

        public override async Task ViewAppearingVM()
        {
            if (!this.isInitialized)
            {
                this.Vitamins = await this.dataService.GetVitamins().ConfigureAwait(false);
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

        partial void OnPassedDataChanged(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            var target = this.Vitamins?.FirstOrDefault(v => v.Id == value);
            ItemSelectedCommand.Execute(target);
            PassedData = string.Empty;
        }
    }
}
