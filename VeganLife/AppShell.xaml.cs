// <copyright file="AppShell.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Mopups.Services;
using VeganLife.Helpers;
using VeganLife.Resources.Translations;
using VeganLife.Views.Base;
using VeganLife.Views.MainPageFlyout;
using VeganLife.Views.MainPageFlyout.FoodTab;
using VeganLife.Views.SettingTab;
using VeganLife.Views.ToolFlyout;

// Ignore Spelling: App
namespace VeganLife
{
    /// <summary>
    /// auto-generated.
    /// </summary>
    public partial class AppShell : Shell
    {
        private ShellNavigationSource _currentShellNavigationSource;

        public IEnumerable<Page> PreviousPageStack { get; set; }

        public Dictionary<string, Type> Routes { get; private set; } = new Dictionary<string, Type>();

        /// <summary>
        /// Initializes a new instance of the <see cref="AppShell"/> class.
        /// </summary>
        public AppShell()
        {
            this.InitializeComponent();
            this.FlyoutWidth = App.MainWidthSize * 0.7;
            this.RegisterRoutes();
            Connectivity.ConnectivityChanged += OnConnectivityChanged;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ServicesHelper.GetService<IDeviceService>().SetNavigationBarColor("#144d5a");
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName.Equals("FlyoutIsPresented")
                && this.CurrentPage is IBaseRootPage page
                && page is not null)
            {
                if (this.FlyoutIsPresented)
                {
                    if (!page.IsAnimated)
                    {
                        Shell.SetTabBarIsVisible(CurrentPage, false);
                        page.OnOpenedShellFlyout();
                        page.IsAnimated = true;
                    }
                }
                else
                {
                    if (page.IsAnimated)
                    {
                        page.OnClosedShellFlyout();
                        SetTabBarIsVisible(CurrentPage, true);
                        page.IsAnimated = false;
                    }
                }
            }
        }

        public static void ShowFlyOut()
        {
            // TODO: https://github.com/dotnet/maui/issues/8226
            Shell.Current.FlyoutBehavior = FlyoutBehavior.Locked;
            Shell.Current.FlyoutBehavior = FlyoutBehavior.Flyout;
            Shell.Current.FlyoutIsPresented = true;
        }

        public void SwitchShellContentToolsTab(byte index, BMIResultModel? bMIResult = null)
        {
            switch (index)
            {
                case 0:
                    this.CurrentItem = mainTool_tool;
                    break;

                    //case 1:
                //    this.CurrentItem = bmiCalculator_tool;
                //    break;
                //case 2:
                //    this.CurrentItem = bmrCalculator_tool;
                //    break;
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
                        Application.Current?.Quit(); // Or anything else
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

        /// <inheritdoc/>
        protected override void OnNavigating(ShellNavigatingEventArgs args)
        {
            // if (args.Source != ShellNavigationSource.Unknown)
            //    this.IsBusy = true;
            _currentShellNavigationSource = args.Source;
            base.OnNavigating(args);
        }

        /// <inheritdoc/>
        protected override void OnNavigated(ShellNavigatedEventArgs args)
        {
            // TODO: make crash
            base.OnNavigated(args);
            var currentSectionStack = this.Items
                .SelectMany(item => item.Items)
                .SelectMany(section => GetNavigationStack(section.Navigation)).ToHashSet();
            if (_currentShellNavigationSource != ShellNavigationSource.ShellSectionChanged
                && PreviousPageStack is not null
                && PreviousPageStack.Count() > currentSectionStack.Count)
            {
                var pagesToRemove = PreviousPageStack.Except(currentSectionStack);
                foreach (var page in pagesToRemove)
                {
                    if (page is null)
                    {
                        continue;
                    }

                    if (page.BindingContext is BaseViewModel vm)
                    {
                        this.Dispatcher.Dispatch(async ()
                            => await vm.ViewIsRemovedAsync().ConfigureAwait(false)!);
                    }

                    // page.TearDown();
                }
            }

            PreviousPageStack = currentSectionStack;
        }

        public Task DisplayNoInternetAlert()
        {
            return this.DisplayAlert(
                AppResources.noInternet_common,
                AppResources.checkInternet_common,
                "OK");
        }

        private static bool IsRootPage(VisualElement page)
        {
            if (MopupService.Instance.PopupStack.Any())
            {
                return false;
            }

            return page is MainPage || page is NewsFeedPage || page is MainTool;
        }

        private void RegisterRoutes()
        {
            // this.Routes.Add(nameof(MainPage), typeof(MainPage));
            // this.Routes.Add(nameof(RationPlanPage), typeof(RationPlanPage));
            // this.Routes.Add(nameof(VitaminAndMineralPage), typeof(VitaminAndMineralPage));
            // this.Routes.Add(nameof(NewsFeedPage), typeof(NewsFeedPage));
            // this.Routes.Add(nameof(MainTool), typeof(MainTool));
            // this.Routes.Add(nameof(BMICalculatorPage), typeof(BMICalculatorPage));
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

        private HashSet<Page> GetNavigationStack(INavigation navigation)
            => navigation.NavigationStack
                .Concat(navigation.ModalStack)
                .Where(p => p is not null).ToHashSet();

        [PropertyChanged.SuppressPropertyChangedWarnings]
        private void OnConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (e.NetworkAccess != NetworkAccess.Internet)
            {
                // No internet connection
                MainThread.BeginInvokeOnMainThread(() => DisplayNoInternetAlert());
            }
        }

        // List<Page> GetCurrentSectionStack()
        // {
        //    var result = new List<Page>();
        //    foreach (var item in this.CurrentItem.CurrentItem.Navigation.NavigationStack)
        //    {
        //        if (item is null)
        //            continue;
        //        result.Add(item);
        //    }

        // foreach (var item in this.CurrentItem.CurrentItem.Navigation.ModalStack)
        //    {
        //        if (item is null)
        //            continue;
        //        result.Add(item);
        //    }

        // return result;
        // }
    }
}