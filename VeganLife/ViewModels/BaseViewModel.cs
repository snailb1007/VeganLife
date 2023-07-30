// <copyright file="BaseViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.ViewModels
{
    using VeganLife.Helpers;
    using VeganLife.Services.LocalDataServices;

    /// <summary>
    /// Base class for view-model class.
    /// </summary>
    public abstract partial class BaseViewModel : ObservableObject
    {
        protected readonly IDataService dataService;
        protected readonly INavigationService navigationService;
        protected readonly IDeviceService deviceService;
        protected readonly ISQLite localDatabase;
        protected readonly IPopupNaviService popupNaviService;

        protected bool IsNetworkConnected => Connectivity.Current.NetworkAccess == NetworkAccess.Internet;

        [ObservableProperty]
        private bool isLoading;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseViewModel"/> class.
        /// </summary>
        public BaseViewModel()
        {
            this.dataService = ServicesHelper.GetService<IDataService>();
            this.navigationService = ServicesHelper.GetService<INavigationService>();
            this.deviceService = ServicesHelper.GetService<IDeviceService>();
            this.localDatabase = ServicesHelper.GetService<ISQLite>();
            this.popupNaviService = ServicesHelper.GetService<IPopupNaviService>();
        }

        /// <summary>
        /// Joins a first name and a last name together into a single string.
        /// </summary>
        /// <param name="parameter">The first name to join.</param>
        /// <returns>>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public virtual Task OnNavigatingTo(object parameter)
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

        public virtual Task ViewAppearingVM() => Task.CompletedTask;
        public virtual Task ViewDisappearingVM() => Task.CompletedTask;
    }
}
