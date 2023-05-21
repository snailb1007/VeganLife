using Maui.FixesAndWorkarounds;
using Microsoft.Maui.Handlers;
using SimpleToolkit.Core;
using SkiaSharp.Views.Maui.Controls.Hosting;
using VeganLife.Data.LocalData;
using VeganLife.Handlers;
using VeganLife.Services.LocalDataServices;
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
            .UseSimpleToolkit()
            .ConfigureKeyboardAutoScroll()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				fonts.AddFont("PlayfairDisplay-SemiBold.ttf", "PlayfairDisplaySemiBold");
                fonts.AddFont("FontAwesome6FreeBrands.otf", "FontAwesomeBrands");
                fonts.AddFont("FontAwesome6FreeRegular.otf", "FontAwesomeRegular");
                fonts.AddFont("FontAwesome6FreeSolid.otf", "FontAwesomeSolid");
            });
        RegisterServices(builder.Services);
        AllowMultiLineTruncationOnAndroid();
        builder.ConfigureMauiHandlers((h) =>
        {
            h.AddHandler(typeof(Shell), typeof(ShellHandler));
        });
        EntryHandler.Mapper.AppendToMapping("RemoveUnderline", (handler, entry) =>
        {
#if ANDROID
            handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
#elif IOS
			handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#endif
        });
        return builder.Build();
	}

    static void RegisterServices(IServiceCollection services)
    {
        // service
        services.AddSingleton<INavigationService, NavigationService>();
        services.AddSingleton<IDataService, DataService>();
        services.AddSingleton<ISQLite, SQLiteService>();
        services.AddSingleton<IDeviceService, DeviceService>();
        // page
        services.AddTransient<SettingPage>();
        services.AddTransient<SettingViewModel>();

        services.AddTransient<BMICalculatorPage>();
        services.AddTransient<BMICalculatorViewModel>();

        services.AddTransient<MainPage>();
        services.AddTransient<MainViewModel>();

        services.AddTransient<NewsFeedPage>();
        services.AddTransient<NewsFeedViewModel>();

        services.AddTransient<RationPlanPage>();
        services.AddTransient<RationPlanViewModel>();

        services.AddTransient<LoginPage>();
        services.AddTransient<LoginViewModel>();

        services.AddTransient<RegistrationPage>();
        services.AddTransient<RegistrationViewModel>();

        services.AddTransient<WebViewPage>();
        services.AddTransient<WebViewViewModel>();

        services.AddTransient<FlyoutHeader>();
        services.AddTransient<FlyouttHeaderViewModel>();

        services.AddTransient<VitaminAndMineralPage>();
        services.AddTransient<VitaminAndMineralViewModel>();

        services.AddTransient<FoodDetailPage>();
        services.AddTransient<FoodDetailViewModel>();

        services.AddTransient<BookmarkPage>();
        services.AddTransient<BookmarkViewModel>();

        services.AddTransient<FoodsByCategoryPage>();
        services.AddTransient<FoodsByCategoryViewModel>();

        services.AddTransient<DetailVitaminAndMineralPage>();
        services.AddTransient<DetailVitaminAndMineralViewModel>();
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
