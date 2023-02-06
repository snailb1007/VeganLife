using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using VeganLife.Services;
using VeganLife.Views;

namespace VeganLife.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
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
                var deviceService = new DeviceService();
                deviceService.HideKeyboard();
                entryEmail.Unfocus();
                return;
            }

            var entryPassword = view.FindByName("entryPassword") as Entry;
            if (entryPassword != null && entryPassword.IsFocused)
            {
                var deviceService = new DeviceService();
                deviceService.HideKeyboard();
                entryPassword.Unfocus();
            }
        }

        [RelayCommand]
        async Task Register()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new RegistrationPage());
        }

        [RelayCommand]
        void Close()
        {
            Application.Current.MainPage = new AppShell();
        }

        public LoginViewModel()
        {

        }
    }
}
