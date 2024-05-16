// <copyright file="App.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using VeganLife.Helpers;
using VeganLife.Helpers.AppSetting;
using VeganLife.Resources.Translations;

// Ignore Spelling: App
namespace VeganLife
{
    /// <summary>
    /// auto-generated.
    /// </summary>
    public partial class App : Application
    {
        public static double MainSize
        {
            get => DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="App"/> class.
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            _ = this.SetupThemeAsync();
            this.SetupLanguage();
            this.MainPage = new AppShell();
        }

        private void SetupLanguage()
        {
            StaticHelper.AppSetting.IsVietnameseLang = true;
            var culture = new CultureInfo(ConstantHelper.LanguageVietnam);
            CultureInfo.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
            AppResources.Culture = culture;
        }

        private async Task SetupThemeAsync()
        {
            var isCollectAccepted = await UserSettingsHelper.GetBoolKey(UserSettingKey.IsAcceptedCollectLogs);
            ServicesHelper.GetService<SentryService>().IsEnabled = isCollectAccepted;
            if (string.IsNullOrEmpty(await UserSettingsHelper.GetAsync(UserSettingKey.SelectedTheme)))
            {
                AppThemeHelper.SetTheme(AppTheme.Light);
                await UserSettingsHelper.SetAsync(UserSettingKey.SelectedTheme, AppTheme.Light.ToString());
            }
            else
            {
                var currentThemeUser = await UserSettingsHelper.GetAsync(UserSettingKey.SelectedTheme);
                if (currentThemeUser != null)
                {
                    var goalTheme = currentThemeUser == AppTheme.Dark.ToString() ? AppTheme.Dark : AppTheme.Light;
                    AppThemeHelper.SetTheme(goalTheme);
                }
            }

#if ANDROID
            AndroidX.AppCompat.App.AppCompatDelegate.DefaultNightMode = Current?.UserAppTheme switch
            {
                AppTheme.Light => AndroidX.AppCompat.App.AppCompatDelegate.ModeNightNo,
                AppTheme.Dark => AndroidX.AppCompat.App.AppCompatDelegate.ModeNightYes,
                _ => AndroidX.AppCompat.App.AppCompatDelegate.ModeNightFollowSystem,
            };
#endif
        }
    }
}