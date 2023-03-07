using VeganLife.Views.FoodTab;

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
}
