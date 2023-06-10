using VeganLife.Helpers;
using VeganLife.Helpers.AppSetting;
using VeganLife.Resources.Translations;

namespace VeganLife;

public partial class App : Application
{
    public App(INavigationService service, IDataService dataService)
    {
        InitializeComponent();

        SetupTheme();
        SetupLanguage();
        // MainPage = new NavigationPage(new LoginPage(new LoginViewModel(service, dataService)));
        MainPage = new WelcomePage(new WelcomeViewModel(service, dataService));
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

    private void SetupColor()
    {

    }
}
