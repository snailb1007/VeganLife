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

        public override async Task<Task> ViewAppearingVM()
        {
            this.Vitamins = await this.dataService.GetVitamins().ConfigureAwait(false);
            return base.ViewAppearingVM();
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
