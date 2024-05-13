// <copyright file="SettingViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using VeganLife.Views.SettingTab;

namespace VeganLife.ViewModels
{
    /// <summary>
    /// vm for SettingPage.
    /// </summary>
    public partial class SettingViewModel : BaseViewModel
    {
        [ObservableProperty]
        private bool isDarkMode;
        [ObservableProperty]
        private string appVersionDisplay;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingViewModel"/> class.
        /// </summary>
        public SettingViewModel()
            : base()
        {
            this.Init();
        }

        public override Task ViewAppearingVM()
        {
            this.AppVersionDisplay = AppInfo.VersionString;
            return base.ViewAppearingVM();
        }

        private void Init()
        {
            var currentDeviceTheme = App.Current?.UserAppTheme;
            this.IsDarkMode = currentDeviceTheme == AppTheme.Dark;
        }

        [RelayCommand]
        private async Task OpenLicensePage()
        {
            await this.navigationService.NavigateToPage<LicensePage>();
        }
    }
}
