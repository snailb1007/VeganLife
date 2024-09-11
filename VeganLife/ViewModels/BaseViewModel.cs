// <copyright file="BaseViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using PropertyChanged;
using VeganLife.Helpers;
using VeganLife.Services.LocalDataServices;

namespace VeganLife.ViewModels
{
    /// <summary>
    /// Base class for view-model class.
    /// </summary>
    public abstract partial class BaseViewModel : ObservableObject
    {
        private bool _hasShownAlert;

        protected readonly IDataService dataService;
        protected readonly INavigationService navigationService;
        protected readonly IDeviceService deviceService;
        protected readonly ISQLite localDatabase;
        protected readonly IPopupNaviService popupNaviService;
        protected readonly ILoadingService loadingService;

        protected bool isInitialized;

        protected bool IsNetworkConnected
        {
            get
            {
                var isConnected = ServicesHelper.GetNetworkStatus();

                if (!isConnected && !_hasShownAlert)
                {
                    _hasShownAlert = true; // Prevent showing multiple alerts consecutively
                    MainThread.BeginInvokeOnMainThread(() => _ = DisplayNoInternetAlert());
                }
                else if (isConnected)
                {
                    _hasShownAlert = false;
                }

                return isConnected;
            }
        }

        public bool IsLoading => (loadingService as LoadingService)?.IsLoading ?? false;

        [ObservableProperty]
        private bool _isNeedReloadAppearing;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseViewModel"/> class.
        /// </summary>
        protected BaseViewModel()
        {
            this.dataService = ServicesHelper.GetService<IDataService>();
            this.navigationService = ServicesHelper.GetService<INavigationService>();
            this.deviceService = ServicesHelper.GetService<IDeviceService>();
            this.localDatabase = ServicesHelper.GetService<ISQLite>();
            this.popupNaviService = ServicesHelper.GetService<IPopupNaviService>();
            this.loadingService = ServicesHelper.GetService<ILoadingService>();
            Connectivity.ConnectivityChanged += OnConnectivityChanged;
        }

        [SuppressPropertyChangedWarnings]
        private void OnConnectivityChanged(object? sender, ConnectivityChangedEventArgs e)
        {
            if (e.NetworkAccess != NetworkAccess.Internet)
            {
                // No internet connection
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    _ = DisplayNoInternetAlert();
                });
            }
        }

        public Task DisplayNoInternetAlert()
        {
            return navigationService.DisplayAlert(
                "Connectivity Issue",
                "No Internet connection is available. Please check your connection and try again.",
                "OK");
        }

        /// <summary>
        /// Joins a first name and a last name together into a single string.
        /// </summary>
        /// <param name="parameter">The first name to join.</param>
        /// <returns>>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public virtual Task OnNavigatingTo(object? parameter)
            => Task.CompletedTask;

        /// <summary>
        /// Invoked immediately after the Page is unloaded and is no longer the current source of a parent Frame.
        /// </summary>
        /// <param name="isForwardNavigation">Forward status.</param>
        /// <returns>>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public virtual Task OnNavigatedFrom(bool isForwardNavigation)
            => Task.CompletedTask;

        /// <summary>
        /// Invoked when the Page is loaded and becomes the current source of a parent Frame.
        /// </summary>
        /// <returns>>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public virtual Task OnNavigatedTo()
            => Task.CompletedTask;

        public virtual Task ViewAppearingVM()
        {
            return Task.CompletedTask;
        }

        public virtual Task ViewDisappearingVM() => Task.CompletedTask;

        public virtual Task ViewIsRemovedAsync() => Task.CompletedTask;
        }
}