namespace VeganLife.ViewModels
{
    public partial class BaseViewModel : ObservableObject
    {
        protected readonly INavigationService navigationService;

        [ObservableProperty]
        bool _isLoading;

        protected IDataService data_service;
        protected INavigationService navigation_service;
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
