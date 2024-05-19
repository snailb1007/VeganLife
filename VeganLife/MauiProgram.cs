// <copyright file="MauiProgram.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

#if DEBUG
using Microsoft.Extensions.Logging;
#endif
using Android.Widget;
using ChatGptNet;
using ChatGptNet.Models;
using FFImageLoading.Maui;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using Mopups.Hosting;
using PanCardView;
using Sharpnado.MaterialFrame;
using Sharpnado.Tabs;
using SkiaSharp.Views.Maui.Controls.Hosting;
using VeganLife.Data.LocalData;
using VeganLife.Handlers;
using VeganLife.Helpers.AppSetting;
using VeganLife.Services.CommunityFreeService;
using VeganLife.Services.LocalDataServices;
using VeganLife.Services.OpenAIService;
using VeganLife.Services.UserServices;
using VeganLife.ViewModels.ContentViewModels;
using VeganLife.ViewModels.PopupViewModels;
using VeganLife.ViewModels.TabsViewModel;
using VeganLife.ViewModels.ToolsFlyoutViewModel;
using VeganLife.Views.ChatFlyout;
using VeganLife.Views.ContentViews.Tabs;
using VeganLife.Views.Controls;
using VeganLife.Views.MainPageFlyout;
using VeganLife.Views.MainPageFlyout.FoodTab;
using VeganLife.Views.MainPageFlyout.VitaminTab;
using VeganLife.Views.Popups;
using VeganLife.Views.PortionTab;
using VeganLife.Views.SettingFlyout;
using VeganLife.Views.SettingTab;
using VeganLife.Views.ToolFlyout;

namespace VeganLife
{
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

                    // v6.5.1
                    fonts.AddFont("fa-solid-900.ttf", "FASolid");
                    fonts.AddFont("fa-regular-400.ttf", "FARegular");
                    fonts.AddFont("fa-thin-100.ttf", "FAThin");
                    fonts.AddFont("fa-light-300.ttf", "FALight");
                });
#if DEBUG
            builder.Logging.AddDebug();
#endif
            builder
                .ConfigureMopups()
                .UseFFImageLoading()
                .UseMauiCommunityToolkit()
                .UseCardsView()
                .UseSkiaSharp(true)
                .UseSharpnadoTabs(loggerEnable: false)
                .UseSharpnadoMaterialFrame(loggerEnable: false);
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

        // public static string GetDatabasePath() => Path.Combine(FileSystem.AppDataDirectory, "Report.db");
        private static void RegisterServices(IServiceCollection services)
        {
            // service
            services.AddChatGpt(options =>
            {
                options.UseOpenAI(apiKey: ConstantHelper.OpenAIConstant.OpenAITokenVip);
                options.DefaultModel = OpenAIChatGptModels.Gpt4_o;
                options.MessageLimit = 15; // Default: 15
                options.MessageExpiration = TimeSpan.FromMinutes(3); // Default: 1 hour
            });
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<IDataService, DataService>();
            services.AddSingleton<ISQLite, SQLiteService>();
            services.AddSingleton<IDeviceService, DeviceService>();
            services.AddSingleton<IPopupNaviService, PopupNaviService>();
            services.AddSingleton<IUserDataService, UserDataService>();
            services.AddSingleton<IOpenAIService, OpenAIService>();
            services.AddSingleton<USDAApiService>();
            services.AddSingleton<UserInfoDataStoreServie>();
            services.AddSingleton<FoodDetailDataStoreService>();
            services.AddSingleton<UsdaFoodDataStoreService>();
            services.AddSingleton<FoodPreviewDataStoreService>();
            services.AddSingleton<NutritionMealLogDataStoreService>();
            services.AddSingleton<ChatLogsDataStoreService>();
            services.AddSingleton<SentryService>();

            // page
            // services.AddTransient<WebViewPage, WebViewViewModel>();
            // services.AddTransient<WelcomePage, WelcomeViewModel>();
            // services.AddTransient<ChatListPage>();
            // services.AddTransient<LoginPage>();
            // services.AddTransient<LoginViewModel>();
            // services.AddTransient<RegistrationPage>();
            // services.AddTransient<RegistrationViewModel>();
            services.AddTransient<SettingPage, SettingViewModel>();
            services.AddTransient<MainTool, MainToolViewModel>();
            services.AddTransient<MainPage, MainViewModel>();
            services.AddTransient<NewsFeedPage, NewsFeedViewModel>();
            services.AddTransient<FlyoutHeader, FlyouttHeaderViewModel>();
            services.AddTransient<VitaminAndMineralPage, VitaminAndMineralViewModel>();
            services.AddTransient<FoodDetailPage, FoodDetailViewModel>();
            services.AddTransient<BookmarkPage, BookmarkViewModel>();
            services.AddTransient<FoodsByCategoryPage, FoodsByCategoryViewModel>();
            services.AddTransient<DetailVitaminAndMineralPage, DetailVitaminAndMineralViewModel>();
            services.AddTransient<LicensePage, LicenseViewModel>();
            services.AddTransient<ProfilePage, ProfileViewModel>();
            services.AddTransient<ReportPage, ReportPageViewModel>();
            services.AddTransient<CaloriesTab, CaloriesViewModel>();
            services.AddTransient<MacrosTab, MacrosViewModel>();
            services.AddTransient<NutrientsTab, NutrientsViewModel>();
            services.AddTransient<BMICalculatorPage, BmiCalculatorViewModel>();
            services.AddTransient<BMRCalculatorPage, BmrCalculatorViewModel>();
            services.AddTransient<UsdaFoodFactDetailPage, UsdaFoodFactDetailVM>();
            services.AddTransient<ConversationPage, ConversationViewModel>();
            services.AddTransient<SupportPage, SupportPageVM>();

            services.AddTransient<ChatGPTDisClaimerPage>();
            services.AddTransient<ChatGPTDetailPage>();

            // Toolkit pop-up
            services.AddTransient<AboutAppPopup>();

            services.AddTransientPopup<BmiMoreInfoToolBarPopup, BmiMoreInfoToolBarPopupVM>();

            // Mopup
            services.AddTransient<BmiResultPopup, BmiResultPopupViewmodel>();
            services.AddTransient<ProfilePopup, ProfilePopupViewModel>();
        }

        private static void AllowMultiLineTruncationOnAndroid()
        {
            static void UpdateMaxLines(LabelHandler handler, ILabel label)
            {
                var textView = handler.PlatformView;
#if ANDROID
                if (label is Label controlsLabel
                    && textView.Ellipsize == Android.Text.TextUtils.TruncateAt.End && controlsLabel.MaxLines != -1)
                {
                    textView.SetMaxLines(controlsLabel.MaxLines);
                }
#elif IOS
                if (label is Label controlsLabel
                          && textView.LineBreakMode == UILineBreakMode.TailTruncation)
                {
                    textView.Lines = controlsLabel.MaxLines;
                }
#endif
            }

            LabelHandler.Mapper.AppendToMapping(
               nameof(Label.LineBreakMode), (h, v) => UpdateMaxLines((LabelHandler)h, v));
            LabelHandler.Mapper.AppendToMapping(
              nameof(Label.MaxLines), (h, v) => UpdateMaxLines((LabelHandler)h, v));
        }

        private static void CustomEntry()
        {
            EntryHandler.Mapper.AppendToMapping("RemoveUnderline", (handler, entry) =>
            {
#if ANDROID
                handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
                handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Colors.Transparent.ToAndroid());
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
                {
                    item.SetColorFilter(Colors.Gray.ToAndroid());
                }

                // remove underline
                handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Colors.Transparent.ToAndroid());
#endif
            });
        }
    }
}