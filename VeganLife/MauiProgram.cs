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
