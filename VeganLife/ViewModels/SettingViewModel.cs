// <copyright file="SettingViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AsyncAwaitBestPractices;
using PropertyChanged;
using VeganLife.Helpers;
using VeganLife.Views.SettingTab;

namespace VeganLife.ViewModels
{
    /// <summary>
    /// vm for SettingPage.
    /// </summary>
    public partial class SettingViewModel : BaseViewModel
    {
        [ObservableProperty]
        private bool _isDarkMode;
        [ObservableProperty]
        private string _appVersionDisplay;

        [ObservableProperty]
        private bool _isAllowCollectLogs;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingViewModel"/> class.
        /// </summary>
        public SettingViewModel()
            : base()
        {
            this.Init();
        }

        public override async Task<Task> ViewAppearingVM()
        {
            this.AppVersionDisplay = AppInfo.VersionString;
            IsAllowCollectLogs = await UserSettingsHelper.GetBoolKey(UserSettingKey.IsAcceptedCollectLogs);
            return base.ViewAppearingVM();
        }

        private void Init()
        {
            var currentDeviceTheme = Application.Current?.UserAppTheme;
            this.IsDarkMode = currentDeviceTheme == AppTheme.Dark;
        }

        [RelayCommand]
        private async Task OpenLicensePage()
        {
            await this.navigationService.NavigateToPage<LicensePage>();
        }

        [SuppressPropertyChangedWarnings]
        partial void OnIsAllowCollectLogsChanged(bool value)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                ServicesHelper.GetService<SentryService>().IsEnabled = value;
                UserSettingsHelper.SetAsync(UserSettingKey.IsAcceptedCollectLogs, value.ToString()).SafeFireAndForget();
            });
        }
    }
}