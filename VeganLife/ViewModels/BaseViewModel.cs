namespace VeganLife.ViewModels
{
    public partial class BaseViewModel : ObservableObject
    {
        protected readonly INavigationService navigationService;
        //protected

        [ObservableProperty]
        bool _isLoading;
        public BaseViewModel()
        {
        }

        public virtual Task OnNavigatingTo(object parameter)
            => Task.CompletedTask;

        public virtual Task OnNavigatedFrom(bool isForwardNavigation)
            => Task.CompletedTask;

        public virtual Task OnNavigatedTo()
            => Task.CompletedTask;
    }
}
