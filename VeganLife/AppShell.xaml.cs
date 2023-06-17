using Mopups.Pages;
using Mopups.Services;
using VeganLife.Helpers;
using VeganLife.Views.ContentViews;
using VeganLife.Views.FoodTab;
using VeganLife.Views.SettingTab;

namespace VeganLife;

public partial class AppShell : Shell
{
    public Dictionary<string, Type> Routes { get; private set; } = new Dictionary<string, Type>();

    public AppShell()
	{
		InitializeComponent();
        RegisterRoutes();
    }

    protected override bool OnBackButtonPressed()
    {
        if (isRootPage(CurrentPage))
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                bool result = await this.DisplayAlert("Alert!", "Do you really want to exit?", "Yes", "No");
                if (result)
                {
                    Process.GetCurrentProcess().CloseMainWindow(); // Or anything else
                }
            });
            return true;

        }
        else if(ServicesHelper.GetService<INavigationService>().GetStackCount() > 1)
        {
            Shell.Current.Navigation.PopAsync();
            return true;
        }
        else
        {
            return base.OnBackButtonPressed();
        }
    }

    public static void ShowFlyout()
    {
        // TODO https://github.com/dotnet/maui/issues/8226
        Shell.Current.FlyoutBehavior = FlyoutBehavior.Locked;
        Shell.Current.FlyoutBehavior = FlyoutBehavior.Flyout;
        Shell.Current.FlyoutIsPresented = true;
    }

    void RegisterRoutes()
    {
        Routes.Add(nameof(MainPage), typeof(MainPage));
        Routes.Add(nameof(RationPlanPage), typeof(RationPlanPage));
        Routes.Add(nameof(VitaminAndMineralPage), typeof(VitaminAndMineralPage));
        Routes.Add(nameof(NewsFeedPage), typeof(NewsFeedPage));
        Routes.Add(nameof(BMICalculatorPage), typeof(BMICalculatorPage));
        // Routes.Add(nameof(LoginPage), typeof(LoginPage));
        // Routes.Add(nameof(RegistrationPage), typeof(RegistrationPage));
        Routes.Add(nameof(FoodsByCategoryPage), typeof(FoodsByCategoryPage));
        Routes.Add(nameof(FoodDetailPage), typeof(FoodDetailPage));
        Routes.Add(nameof(LicensePage), typeof(LicensePage));
        foreach (var item in Routes)
        {
            Routing.RegisterRoute(item.Key, item.Value);
        }
    }

    static bool isRootPage(VisualElement page)
    {
        if (MopupService.Instance.PopupStack.Count() > 0)
            return false;
        return page is MainPage || page is RationPlanPage || page is NewsFeedPage || page is BMICalculatorPage;
    }
}
