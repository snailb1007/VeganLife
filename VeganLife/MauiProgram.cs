using SkiaSharp.Views.Maui.Controls.Hosting;
using VeganLife.ViewModels.ContentViewModels;
using VeganLife.Views.ContentViews;
using VeganLife.Views.Controls;
using VeganLife.Views.FoodTab;

namespace VeganLife;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
        builder
            .UseSkiaSharp(true)
			.UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("FontAwesome6FreeBrands.otf", "FontAwesomeBrands");
                fonts.AddFont("FontAwesome6FreeRegular.otf", "FontAwesomeRegular");
                fonts.AddFont("FontAwesome6FreeSolid.otf", "FontAwesomeSolid");
            });
        RegisterServices(builder.Services);
        AllowMultiLineTruncationOnAndroid();

        return builder.Build();
	}

    static void RegisterServices(IServiceCollection services)
    {
        services.AddSingleton<SettingPage>();
        services.AddSingleton<SettingViewModel>();

        services.AddSingleton<BMICalculatorPage>();
        services.AddSingleton<BMICalculatorViewModel>();

        services.AddSingleton<MainPage>();
        services.AddSingleton<MainViewModel>();

        services.AddSingleton<NewsFeedPage>();
        services.AddSingleton<NewsFeedViewModel>();

        services.AddSingleton<RationPlanPage>();
        services.AddSingleton<RationPlanViewModel>();

        services.AddSingleton<LoginPage>();
        services.AddSingleton<LoginViewModel>();

        services.AddSingleton<RegistrationPage>();
        services.AddSingleton<RegistrationViewModel>();

        services.AddSingleton<WebViewPage>();
        services.AddSingleton<WebViewViewModel>();

        services.AddSingleton<FlyoutHeader>();
        services.AddSingleton<FlyouttHeaderViewModel>();

        services.AddSingleton<VitaminAndMineralPage>();
        services.AddSingleton<VitaminAndMineralViewModel>();

        services.AddSingleton<FoodDetailPage>();
        services.AddSingleton<FoodDetailViewModel>();
    }

    static void AllowMultiLineTruncationOnAndroid()
    {
#if ANDROID
        static void UpdateMaxLines(Microsoft.Maui.Handlers.LabelHandler handler, ILabel label)
        {
            var textView = handler.PlatformView;
            if (label is Label controlsLabel && textView.Ellipsize == Android.Text.TextUtils.TruncateAt.End)
            {
                textView.SetMaxLines(controlsLabel.MaxLines);
            }
        };

        Label.ControlsLabelMapper.AppendToMapping(
           nameof(Label.LineBreakMode), UpdateMaxLines);

        Label.ControlsLabelMapper.AppendToMapping(
            nameof(Label.MaxLines), UpdateMaxLines);
#endif
    }
}
