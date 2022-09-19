using VeganLife.ViewModels;
using VeganLife.Views;

namespace VeganLife;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				fonts.AddFont("fa-solid-900.ttf", "AwesomeSolid");
			});

		builder.Services.AddSingleton<SettingPage>();
		builder.Services.AddSingleton<SettingViewModel>();

		return builder.Build();
	}
}
