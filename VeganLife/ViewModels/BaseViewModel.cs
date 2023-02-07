namespace VeganLife.ViewModels
{
    public partial class BaseViewModel : ObservableObject
    {
        protected readonly INavigationService navigationService;

        [ObservableProperty]
        bool _isLoading;
        public BaseViewModel()
        {
        }
    }
}
