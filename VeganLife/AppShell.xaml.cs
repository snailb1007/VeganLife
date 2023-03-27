using VeganLife.Helpers;
using VeganLife.Views;
using VeganLife.Views.ContentViews;
using VeganLife.Views.FoodTab;

namespace VeganLife;

public partial class AppShell : SimpleToolkit.SimpleShell.SimpleShell
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
            Shell.Current.Navigation.PopToRootAsync();
            return true;
        }
        else
        {
            return base.OnBackButtonPressed();
        }
    }

    void RegisterRoutes()
    {
        Routes.Add(nameof(BMICalculatorPage), typeof(BMICalculatorPage));
        Routes.Add(nameof(LoginPage), typeof(LoginPage));
        Routes.Add(nameof(RegistrationPage), typeof(RegistrationPage));
        Routes.Add(nameof(FoodDetailPage), typeof(FoodDetailPage));

        foreach (var item in Routes)
        {
            Routing.RegisterRoute(item.Key, item.Value);
        }
    }

    static bool isRootPage(VisualElement page)
    {
        return page is MainPage || page is RationPlanPage || page is NewsFeedPage || page is BMICalculatorPage;
    }
}
