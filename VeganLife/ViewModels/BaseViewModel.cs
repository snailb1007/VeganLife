using VeganLife.Helpers;
using VeganLife.Services.LocalDataServices;

namespace VeganLife.ViewModels
{
    public partial class BaseViewModel : ObservableObject
    {
        [ObservableProperty]
        bool _isLoading;

        protected IDataService data_service;
        protected INavigationService navigation_service;
        protected IDeviceService device_service => ServicesHelper.GetService<IDeviceService>();
        protected ISQLite local_database => ServicesHelper.GetService<ISQLite>();

        protected bool IsNetworkConnected => Connectivity.Current.NetworkAccess == NetworkAccess.Internet;

        public BaseViewModel(INavigationService navigationService, IDataService dataService)
        {
            navigation_service = navigationService;
            data_service = dataService;
        }

        public virtual Task OnNavigatingTo(object parameter)
            => Task.CompletedTask;

        public virtual Task OnNavigatedFrom(bool isForwardNavigation)
            => Task.CompletedTask;

        public virtual Task OnNavigatedTo()
            => Task.CompletedTask;
    }
}
