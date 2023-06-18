namespace VeganLife.ViewModels
{
    using VeganLife.Helpers;
    using VeganLife.Services.LocalDataServices;

    public abstract partial class BaseViewModel : ObservableObject
    {
        [ObservableProperty]
        bool _isLoading;

        protected IDataService dataService;
        protected INavigationService navigationService;

        protected IDeviceService deviceService => ServicesHelper.GetService<IDeviceService>();

        protected ISQLite localDatabase => ServicesHelper.GetService<ISQLite>();

        protected bool IsNetworkConnected => Connectivity.Current.NetworkAccess == NetworkAccess.Internet;

        public BaseViewModel(INavigationService navigationService, IDataService dataService)
        {
            this.navigationService = navigationService;
            this.dataService = dataService;
        }

        public virtual Task OnNavigatingTo(object parameter)
            => Task.CompletedTask;

        public virtual Task OnNavigatedFrom(bool isForwardNavigation)
            => Task.CompletedTask;

        public virtual Task OnNavigatedTo()
            => Task.CompletedTask;
    }
}
