// <copyright file="VitaminAndMineralViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.ViewModels.ContentViewModels
{
    public partial class VitaminAndMineralViewModel : BaseViewModel
    {
        [ObservableProperty]
        private IEnumerable<VitaminModel> vitamins;

        public VitaminAndMineralViewModel(INavigationService navigationService, IDataService dataService)
            : base(navigationService, dataService)
        {
            this.Init();
            this.LoadDataAsync().ConfigureAwait(false);
        }

        private void Init()
        {
        }

        private async Task LoadDataAsync()
        {
            this.Vitamins = await this.dataService.GetVitamins();
        }

        [RelayCommand]
        private async Task ItemSelectedAsync()
        {
            await Task.Delay(1);
        }
    }
}
