namespace VeganLife.ViewModels
{
    public partial class LoginViewModel : BaseViewModel
    {
        readonly INavigationService _navigationService;

        [RelayCommand]
        void UnFocus(object obj)
        {
            if (obj == null)
                return;
            var view = (LoginPage)obj;
            if (view == null)
                return;
            var entryEmail = view.FindByName("entryEmail") as Entry;

            if (entryEmail != null && entryEmail.IsFocused)
            {
                device_service.HideKeyboard();
                entryEmail.Unfocus();
                return;
            }

            var entryPassword = view.FindByName("entryPassword") as Entry;
            if (entryPassword != null && entryPassword.IsFocused)
            {
                device_service.HideKeyboard();
                entryPassword.Unfocus();
            }
        }

        [RelayCommand]
        async Task Register()
        {
            await navigation_service.NavigateToRegisPage();
        }

        [RelayCommand]
        void Close()
        {
            Application.Current.MainPage = new AppShell();
        }

        public LoginViewModel(INavigationService navigationService, IDataService dataService)
            : base(navigationService, dataService)
        {
        }
    }
}
