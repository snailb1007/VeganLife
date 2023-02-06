using SkiaSharp.Views.Maui.Controls.Hosting;
using VeganLife.ViewModels;
using VeganLife.ViewModels.ContentViewModels;
using VeganLife.Views;
using VeganLife.Views.ContentViews;

namespace VeganLife;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
        builder
            .UseSkiaSharp(true)
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				fonts.AddFont("materialdesignicons-webfont.ttf", "AwesomeSolid");
			});

		builder.Services.AddSingleton<SettingPage>();
		builder.Services.AddSingleton<SettingViewModel>();

		builder.Services.AddSingleton<BMICalculatorPage>();
		builder.Services.AddSingleton<BMICalculatorViewModel>();

        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddSingleton<MainViewModel>();

		builder.Services.AddSingleton<NewsFeedPage>();
		builder.Services.AddSingleton<NewsFeedViewModel>();

        builder.Services.AddSingleton<RationPlanPage>();
        builder.Services.AddSingleton<RationPlanViewModel>();

        builder.Services.AddSingleton<LoginPage>();
        builder.Services.AddSingleton<LoginViewModel>();

        builder.Services.AddSingleton<RegistrationPage>();
        builder.Services.AddSingleton<RegistrationViewModel>();

        AllowMultiLineTruncationOnAndroid();

        return builder.Build();
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
