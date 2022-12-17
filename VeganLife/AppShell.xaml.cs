using VeganLife.Views;

namespace VeganLife;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(BMICalculatorPage), typeof(BMICalculatorPage));
	}
}
