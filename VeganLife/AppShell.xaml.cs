// <copyright file="AppShell.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife
{
    using Mopups.Services;
    using VeganLife.Helpers;
    using VeganLife.Views.ContentViews;
    using VeganLife.Views.FoodTab;
    using VeganLife.Views.SettingTab;

    public partial class AppShell : Shell
    {
        public Dictionary<string, Type> Routes { get; private set; } = new Dictionary<string, Type>();

        public AppShell()
        {
            this.InitializeComponent();
            this.RegisterRoutes();
        }

        protected override bool OnBackButtonPressed()
        {
            if (IsRootPage(this.CurrentPage))
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    bool result = await this.DisplayAlert("Alert!", "Do you really want to exit?", "Yes", "No");
                    if (result)
                    {
                        Process.GetCurrentProcess().CloseMainWindow(); // Or anything else
                    }
                });
                return true;

            }
            else if (ServicesHelper.GetService<INavigationService>().GetStackCount() > 1)
            {
                Shell.Current.Navigation.PopAsync();
                return true;
            }
            else
            {
                return base.OnBackButtonPressed();
            }
        }

        public static void ShowFlyout()
        {
            // TODO https://github.com/dotnet/maui/issues/8226
            Shell.Current.FlyoutBehavior = FlyoutBehavior.Locked;
            Shell.Current.FlyoutBehavior = FlyoutBehavior.Flyout;
            Shell.Current.FlyoutIsPresented = true;
        }

        private void RegisterRoutes()
        {
            this.Routes.Add(nameof(MainPage), typeof(MainPage));
            this.Routes.Add(nameof(RationPlanPage), typeof(RationPlanPage));
            this.Routes.Add(nameof(VitaminAndMineralPage), typeof(VitaminAndMineralPage));
            this.Routes.Add(nameof(NewsFeedPage), typeof(NewsFeedPage));
            this.Routes.Add(nameof(BMICalculatorPage), typeof(BMICalculatorPage));
            this.Routes.Add(nameof(FoodsByCategoryPage), typeof(FoodsByCategoryPage));
            this.Routes.Add(nameof(FoodDetailPage), typeof(FoodDetailPage));
            this.Routes.Add(nameof(LicensePage), typeof(LicensePage));

            // Routes.Add(nameof(LoginPage), typeof(LoginPage));
            // Routes.Add(nameof(RegistrationPage), typeof(RegistrationPage));
            foreach (var item in this.Routes)
            {
                Routing.RegisterRoute(item.Key, item.Value);
            }
        }

        private static bool IsRootPage(VisualElement page)
        {
            if (MopupService.Instance.PopupStack.Count() > 0)
            {
                return false;
            }

            return page is MainPage || page is RationPlanPage || page is NewsFeedPage || page is BMICalculatorPage;
        }
    }
}