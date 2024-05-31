// <copyright file="VitaminAndMineralViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using VeganLife.Views.MainPageFlyout.VitaminTab;

namespace VeganLife.ViewModels.ContentViewModels
{
    public partial class VitaminAndMineralViewModel : BaseViewModel
    {
        [ObservableProperty]
        private IEnumerable<VitaminModel> vitamins;

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
    }
}
