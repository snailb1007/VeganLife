// <copyright file="App.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife
{
    using Microsoft.Maui;
    using VeganLife.Helpers;
    using VeganLife.Helpers.AppSetting;
    using VeganLife.Resources.Translations;

    /// <summary>
    /// auto-generated.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="App"/> class.
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.SetupTheme();
            this.SetupLanguage();

            if (UserSettingsHelper.IsFirstTime)
            {
                this.MainPage = new NavigationPage(ServicesHelper.GetService<TutorialPage>());
            }
            else
            {
                this.MainPage = new AppShell();
            }
        }

        protected override Window CreateWindow(IActivationState activationState)
        {
            return base.CreateWindow(activationState);
        }

        private void SetupLanguage()
        {
            ConstantHelper.AppSetting.IsVietnameseLang = true;
            var culture = new CultureInfo(ConstantHelper.LanguageVietnam);
            CultureInfo.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
            AppResources.Culture = culture;
        }

        private void SetupTheme()
        {
            if (string.IsNullOrEmpty(UserSettingsHelper.Get(UserSettingKey.SelectedTheme)))
            {
                AppThemeHelper.SetTheme(AppTheme.Light);
                UserSettingsHelper.Set(UserSettingKey.SelectedTheme, AppTheme.Light.ToString());
            }
            else
            {
                var currentThemeUser = UserSettingsHelper.Get(UserSettingKey.SelectedTheme);
                if (currentThemeUser != null)
                {
                    var goalTheme = currentThemeUser == AppTheme.Dark.ToString() ? AppTheme.Dark : AppTheme.Light;
                    AppThemeHelper.SetTheme(goalTheme);
                }
            }
        }
    }
}