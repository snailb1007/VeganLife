using FFImageLoading.Maui;
using Microsoft.Maui.Handlers;
using SkiaSharp.Views.Maui.Controls.Hosting;
using VeganLife.Handlers;
using VeganLife.Services.LocalDataServices;
using VeganLife.ViewModels.ContentViewModels;
using VeganLife.Views.ContentViews;
using VeganLife.Views.Controls;
using VeganLife.Views.FoodTab;
using VeganLife.Views.SettingTab;
using ChatGptNet;
using Mopups.Hosting;
#if ANDROID
using Android.Widget;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
#endif

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
                fonts.AddFont("PlayfairDisplay-SemiBold.ttf", "PlayfairDisplaySemiBold");
                fonts.AddFont("FontAwesome6FreeBrands.otf", "FontAwesomeBrands");
                fonts.AddFont("FontAwesome6FreeRegular.otf", "FontAwesomeRegular");
                fonts.AddFont("FontAwesome6FreeSolid.otf", "FontAwesomeSolid");
            });
        builder.ConfigureMopups().UseFFImageLoading().UseMauiCommunityToolkit();
        RegisterServices(builder.Services);
        builder.ConfigureMauiHandlers((h) =>
        {
            h.AddHandler(typeof(Shell), typeof(ShellHandler));
        });
        CustomEntry();
        CustomSearchBar();
        AllowMultiLineTruncationOnAndroid();
        return builder.Build();
    }

    static void RegisterServices(IServiceCollection services)
    {
        // service
        services.AddChatGpt(options =>
        {
            //options.UseOpenAI(apiKey: $"sk-{APIConstants.OpenAIToken.Trim(new char[] { '-' })}");
            options.UseOpenAI(apiKey: "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");
            options.DefaultModel = "gpt-3.5-turbo";
            options.MessageLimit = 15; // Default: 15
            options.MessageExpiration = TimeSpan.FromMinutes(5); // Default: 1 hour
        });
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
        // services.AddTransient<LoginPage>();
        // services.AddTransient<LoginViewModel>();
        // services.AddTransient<RegistrationPage>();
        // services.AddTransient<RegistrationViewModel>();
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
        services.AddTransient<LicensePage>();
        services.AddTransient<LicenseViewModel>();
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

    static void CustomEntry()
    {
        EntryHandler.Mapper.AppendToMapping("RemoveUnderline", (handler, entry) =>
        {
#if ANDROID
            handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
#elif IOS
			handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#endif
        });
    }

    static void CustomSearchBar()
    {
        SearchBarHandler.Mapper.AppendToMapping("CustomizationSearchBar", (handler, view) =>
        {
#if ANDROID
            LinearLayout linearLayout = handler.PlatformView.GetChildAt(0) as LinearLayout;
            linearLayout = linearLayout.GetChildAt(2) as LinearLayout;
            linearLayout = linearLayout.GetChildAt(1) as LinearLayout;
            linearLayout.Background = null;
            // remove underline
            handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Colors.Transparent.ToAndroid());
#endif
        });
    }
}
