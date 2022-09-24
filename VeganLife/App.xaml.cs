using System.Globalization;
using VeganLife.Resources.Translations;

namespace VeganLife;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
		SetupLanguage();

		MainPage = new AppShell();
	}

	private void SetupLanguage()
	{
		var culture = new CultureInfo("vi");
		CultureInfo.CurrentCulture = culture;
		Thread.CurrentThread.CurrentUICulture = culture;
        AppResources.Culture = culture;
	}
}
