// <copyright file="AppShell.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

// Ignore Spelling: App

namespace VeganLife
{
    using Mopups.Services;
    using VeganLife.Helpers;
    using VeganLife.Views.ContentViews;
    using VeganLife.Views.FoodTab;
    using VeganLife.Views.SettingTab;
    using VeganLife.Views.ToolFlyout;
    using static VeganLife.Helpers.AppSetting.StaticHelper;

    /// <summary>
    /// auto-generated.
    /// </summary>
    public partial class AppShell : Shell
    {
        public Dictionary<string, Type> Routes { get; private set; } = new Dictionary<string, Type>();

        /// <summary>
        /// Initializes a new instance of the <see cref="AppShell"/> class.
        /// </summary>
        public AppShell()
        {
            this.InitializeComponent();
            this.RegisterRoutes();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            this.Dispatcher.Dispatch(async () =>
            {
                await ServicesHelper.GetService<IDataService>().GetHealthDiagnosisFirebaseDataModel()
                .ContinueWith(t =>
                {
                    HealthDiagnosisFirebaseDataModel.BMIModel = t.Result;
                })
                .ConfigureAwait(false);
            });
        }

        public static void ShowFlyOut()
        {
            // TODO: https://github.com/dotnet/maui/issues/8226
            Shell.Current.FlyoutBehavior = FlyoutBehavior.Locked;
            Shell.Current.FlyoutBehavior = FlyoutBehavior.Flyout;
            Shell.Current.FlyoutIsPresented = true;
        }

        public void SwitchShellContentToolsTab(byte index, BMIResultModel bMIResult = null)
        {
            switch (index)
            {
                case 0:
                    this.CurrentItem = mainTool_tool;
                    break;
                case 1:
                    this.CurrentItem = bmiCalculator_tool;
                    break;
                case 2:
                    this.CurrentItem = bmrCalculator_tool;
                    break;

            }
        }

        /// <inheritdoc/>
        protected override bool OnBackButtonPressed()
        {
            if (IsRootPage(this.CurrentPage))
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    bool result = await this.DisplayAlert("Alert!", "Do you really want to exit?", "Yes", "No");
                    if (result)
                    {
                        Application.Current.Quit(); // Or anything else
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

        protected override void OnNavigating(ShellNavigatingEventArgs args)
        {
            if (args.Source != ShellNavigationSource.Unknown)
                this.IsBusy = true;
            base.OnNavigating(args);
        }

        protected override void OnNavigated(ShellNavigatedEventArgs args)
        {
            base.OnNavigated(args);
            this.IsBusy = false;
        }

        private static bool IsRootPage(VisualElement page)
        {
            if (MopupService.Instance.PopupStack.Any())
            {
                return false;
            }

            return page is MainPage || page is RationPlanPage || page is NewsFeedPage || page is MainTool;
        }

        private void RegisterRoutes()
        {
            this.Routes.Add(nameof(MainPage), typeof(MainPage));
            this.Routes.Add(nameof(RationPlanPage), typeof(RationPlanPage));
            this.Routes.Add(nameof(VitaminAndMineralPage), typeof(VitaminAndMineralPage));
            this.Routes.Add(nameof(NewsFeedPage), typeof(NewsFeedPage));
            this.Routes.Add(nameof(MainTool), typeof(MainTool));
            this.Routes.Add(nameof(FoodsByCategoryPage), typeof(FoodsByCategoryPage));
            this.Routes.Add(nameof(FoodDetailPage), typeof(FoodDetailPage));
            this.Routes.Add(nameof(LicensePage), typeof(LicensePage));
            this.Routes.Add(nameof(BMICalculatorPage), typeof(BMICalculatorPage));

            // Routes.Add(nameof(LoginPage), typeof(LoginPage));
            // Routes.Add(nameof(RegistrationPage), typeof(RegistrationPage));
            foreach (var item in this.Routes)
            {
                Routing.RegisterRoute(item.Key, item.Value);
            }
        }
    }
}