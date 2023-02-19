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
