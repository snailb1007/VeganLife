// <copyright file="MauiProgram.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife
{
    using FFImageLoading.Maui;
    using Microsoft.Maui.Handlers;
    using Mopups.Hosting;
    using VeganLife.Handlers;
    using VeganLife.Services.LocalDataServices;
    using VeganLife.ViewModels.ContentViewModels;
    using VeganLife.Views.ContentViews;
    using VeganLife.Views.Controls;
    using VeganLife.Views.FoodTab;
    using VeganLife.Views.Popups;
    using VeganLife.Views.SettingTab;
#if GPT
    using ChatGptNet;
#endif
#if ANDROID
    using Android.Widget;
    using Microcharts.Maui;
    using Microsoft.Maui.Controls.Compatibility.Platform.Android;
    using VeganLife.ViewModels.PopupViewModels;
    using VeganLife.Data.LocalData;
    using Microsoft.Maui.Platform;
    using Microsoft.AppCenter.Crashes;
    using Microsoft.AppCenter;
    using VeganLife.Services.UserServices;
    using PanCardView;
    using VeganLife.Views.ToolFlyout;
    using VeganLifeDataCenter.Data;
    using Microsoft.EntityFrameworkCore;
    using Sharpnado.Tabs;
    using VeganLife.ViewModels.TabsViewModel;
    using VeganLife.Views.ContentViews.Tabs;
    using SkiaSharp.Views.Maui.Controls.Hosting;
    using VeganLife.ViewModels.ToolsFlyoutViewModel;
    using VeganLife.Services.CommunityFreeService;
    using VeganLife.Views.PortionTab;
    using UraniumUI;
#endif

    /// <summary>
    /// auto-generated.
    /// </summary>
    public static class MauiProgram
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MauiProgram"/> class.
        /// </summary>
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
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
            builder
                .ConfigureMopups()
                .UseFFImageLoading()
                .UseMauiCommunityToolkit()
                .UseCardsView()
                .UseSkiaSharp(true)
                .UseSharpnadoTabs(loggerEnable: false)
                .UseUraniumUIBlurs()
                .UseMicrocharts();
            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite($"Filename={GetDatabasePath()}", x => x.MigrationsAssembly(nameof(VeganLifeDataCenter))));
            // AppCenter.Start("2772beb2-5a37-4296-9ecb-d8ba262856ca", typeof(Crashes));
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

        public static string GetDatabasePath() => Path.Combine(FileSystem.AppDataDirectory, "Report.db");

        private static void RegisterServices(IServiceCollection services)
        {
            // service
#if GPT
            services.AddChatGpt(options =>
            {
                //options.UseOpenAI(apiKey: $"sk-");
                options.UseOpenAI(apiKey: "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");
                options.DefaultModel = "gpt-3.5-turbo";
                options.MessageLimit = 15; // Default: 15
                options.MessageExpiration = TimeSpan.FromMinutes(5); // Default: 1 hour
            });
#endif
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<IDataService, DataService>();
            services.AddSingleton<ISQLite, SQLiteService>();
            services.AddSingleton<IDeviceService, DeviceService>();
            services.AddSingleton<IPopupNaviService, PopupNaviService>();
            services.AddSingleton<IUserDataService, UserDataService>();
            services.AddSingleton<USDAApiService>();
            services.AddSingleton<UserInfoDataStoreServie>();
            services.AddSingleton<FoodDetailDataStoreService>();
            services.AddSingleton<UsdaFoodDataStoreService>();
            // page
            services.AddTransient<SettingPage>();
            services.AddTransient<SettingViewModel>();
            services.AddTransient<MainTool>();
            services.AddTransient<MainToolViewModel>();
            services.AddTransient<MainPage>();
            services.AddTransient<MainViewModel>();
            services.AddTransient<NewsFeedPage>();
            services.AddTransient<NewsFeedViewModel>();
            services.AddTransient<RationPlanPage>();
            services.AddTransient<RationPlanViewModel>();
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
            services.AddTransient<ProfilePage>();
            services.AddTransient<ProfileViewModel>();
            services.AddTransient<WelcomePage>();
            services.AddTransient<WelcomeViewModel>();
            services.AddTransient<ReportPage>();
            services.AddTransient<ReportPageViewModel>();
            services.AddTransient<CaloriesViewModel>();
            services.AddTransient<MacrosViewModel>();
            services.AddTransient<NutrientsViewModel>();
            services.AddTransient<CaloriesTab>();
            services.AddTransient<MacrosTab>();
            services.AddTransient<NutrientsTab>();
            services.AddTransient<BMICalculatorPage>();
            services.AddTransient<BmiCalculatorViewModel>();
            services.AddTransient<BMRCalculatorPage>();
            services.AddTransient<BmrCalculatorViewModel>();
            services.AddTransient<UsdaFoodFactDetailPage>();
            services.AddTransient<UsdaFoodFactDetailVM>();

            // Popup
            services.AddTransient<BmiResultPopup>();
            services.AddTransient<BmiResultPopupViewmodel>();
            services.AddTransient<ProfilePopup>();
            services.AddTransient<ProfilePopupViewModel>();
            services.AddTransient<AboutAppPopup>();
            services.AddTransient<BmiMoreInfoToolBarPopup>();
            services.AddTransient<BmiMoreInfoToolBarPopupVM>();

            // services.AddTransient<LoginPage>();
            // services.AddTransient<LoginViewModel>();
            // services.AddTransient<RegistrationPage>();
            // services.AddTransient<RegistrationViewModel>();
        }

        private static void AllowMultiLineTruncationOnAndroid()
        {
#if ANDROID
            static void UpdateMaxLines(LabelHandler handler, ILabel label)
            {
                var textView = handler.PlatformView;
                if (label is Label controlsLabel && textView.Ellipsize == Android.Text.TextUtils.TruncateAt.End)
                {
                    textView.SetMaxLines(controlsLabel.MaxLines);
                }
            }

            Label.ControlsLabelMapper.AppendToMapping(
               nameof(Label.LineBreakMode), UpdateMaxLines);

            Label.ControlsLabelMapper.AppendToMapping(
                nameof(Label.MaxLines), UpdateMaxLines);
#endif
        }

        private static void CustomEntry()
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

        private static void CustomSearchBar()
        {
            SearchBarHandler.Mapper.AppendToMapping("CustomizationSearchBar", (handler, view) =>
            {
#if ANDROID
                var child = handler.PlatformView.GetChildrenOfType<ImageView>();
                foreach (var item in child)
                    item.SetColorFilter(Colors.Gray.ToAndroid());
                // remove underline
                handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Colors.Transparent.ToAndroid());
#endif
            });
        }
    }
}