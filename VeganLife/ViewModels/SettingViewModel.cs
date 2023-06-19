// <copyright file="SettingViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.ViewModels
{
    using VeganLife.Helpers;
    using VeganLife.Helpers.AppSetting;
    using VeganLife.Views.SettingTab;

    /// <summary>
    /// vm for SettingPage.
    /// </summary>
    public partial class SettingViewModel : BaseViewModel
    {
        [ObservableProperty]
        private bool isDarkMode;

        [ObservableProperty]
        private string imgBackground = string.Empty;

        [ObservableProperty]
        private int indexPickerOption;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingViewModel"/> class.
        /// </summary>
        public SettingViewModel()
            : base()
        {
            this.Init();
        }

        private void Init()
        {
            this.IndexPickerOption = 0;
            this.ImgBackground = ConstantHelper.ThemeInfo.ImgBackground;
            var currentDeviceTheme = App.Current.UserAppTheme;
            this.IsDarkMode = currentDeviceTheme == AppTheme.Dark;
        }

        [RelayCommand]
        private void SwitchTheme(Microsoft.Maui.Controls.Switch parameter)
        {
            if (parameter == null)
            {
                return;
            }

            this.IsDarkMode = parameter.IsToggled;
            var goalTheme = this.IsDarkMode ? AppTheme.Dark : AppTheme.Light;
            AppThemeHelper.SetTheme(goalTheme);
            UserSettingsHelper.Set(UserSettingKey.SelectedTheme, goalTheme.ToString());
        }

        [RelayCommand]
        private async Task OpenLicensePage()
        {
            await this.navigationService.NavigataToPage<LicensePage>();
        }
    }
}
