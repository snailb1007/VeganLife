// <copyright file="SettingViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

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
        private bool isDarkMode;
        [ObservableProperty]
        private string appVersionDisplay;

        [ObservableProperty]
        private bool isAllowCollectLogs;

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
            var currentDeviceTheme = App.Current?.UserAppTheme;
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
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                using (await this.loadingService.Show(delayTime: 500))
                {
                    ServicesHelper.GetService<SentryService>().IsEnabled = value;
                    _ = UserSettingsHelper.SetAsync(UserSettingKey.IsAcceptedCollectLogs, value.ToString()).ConfigureAwait(false);
                }
            });
        }
    }
}