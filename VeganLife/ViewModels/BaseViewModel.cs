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
    }
}
