using VeganLife.Helpers;
using VeganLife.Helpers.AppSetting;
using VeganLife.Resources.Translations;

namespace VeganLife;

public partial class App : Application
{
    public App(INavigationService service)
    {
        InitializeComponent();

        SetupTheme();
        SetupLanguage();

        MainPage = new NavigationPage(new LoginPage(new LoginViewModel(service)));
    }

    private void SetupLanguage()
    {
        ConstantHelper.AppSetting.IsVietnameseLang = true;
        var culture = new CultureInfo(ConstantHelper.Language_Vietnam);
        CultureInfo.CurrentCulture = culture;
        Thread.CurrentThread.CurrentUICulture = culture;
        AppResources.Culture = culture;
    }

    private void SetupTheme()
    {
        var themeMode = UserSettingsHelper.Get(UserSettingKey.ThemeMode);
        if (string.IsNullOrEmpty(themeMode))
        {
            AppThemeHelper.SetTheme(App.Current.PlatformAppTheme);

            UserSettingsHelper.Set(UserSettingKey.ThemeMode, ConstantHelper.Theme_Mode_Auto);
            UserSettingsHelper.Set(UserSettingKey.SelectedTheme, AppTheme.Unspecified.ToString());
        }
        else if (themeMode == ConstantHelper.Theme_Mode_Auto)
        {
            AppThemeHelper.SetTheme(App.Current.PlatformAppTheme);
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
