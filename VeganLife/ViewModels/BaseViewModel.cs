// <copyright file="BaseViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Reactive.Linq;
using System.Reactive.Subjects;
using VeganLife.Helpers;
using VeganLife.Resources.Translations;
using VeganLife.Services.LocalDataServices;

namespace VeganLife.ViewModels
{
    /// <summary>
    /// Base class for view-model class.
    /// </summary>
    public abstract partial class BaseViewModel : ObservableObject, IDisposable
    {
        private bool _hasShownAlert;
        private readonly IDisposable _busySubscription;

        protected readonly IDataService dataService;
        protected readonly INavigationService navigationService;
        protected readonly IDeviceService deviceService;
        //protected readonly ISQLite localDatabase;
        protected readonly IPopupNaviService popupNaviService;

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

        protected readonly BusyManager busyManager;

        [ObservableProperty]
        private bool _isLoading;

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
            //this.localDatabase = ServicesHelper.GetService<ISQLite>();
            this.popupNaviService = ServicesHelper.GetService<IPopupNaviService>();
            busyManager = new();

            _busySubscription = busyManager.IsBusy.Subscribe(busy => IsLoading = busy);
        }

        public Task DisplayNoInternetAlert()
        {
            return navigationService.DisplayAlert(
                AppResources.noInternet_common,
                AppResources.checkInternet_common,
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

        void IDisposable.Dispose()
        {
            _busySubscription.Dispose();
        }
    }

    public class BusyManager : IDisposable
    {
        private readonly BehaviorSubject<bool> _isBusySubject = new BehaviorSubject<bool>(false);

        private int _busyCount;

        public IObservable<bool> IsBusy => _isBusySubject.AsObservable();

        public void Increase()
        {
            int newCount = Interlocked.Increment(ref _busyCount);
            UpdateBusyState(newCount);
        }

        public void Decrease()
        {
            int newCount = Interlocked.Decrement(ref _busyCount);
            if (newCount < 0)
            {
                throw new InvalidOperationException("Busy count cannot be negative.");
            }

            UpdateBusyState(newCount);
        }

        private void UpdateBusyState(int newCount)
        {
            _isBusySubject.OnNext(newCount > 0);
        }

        void IDisposable.Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}