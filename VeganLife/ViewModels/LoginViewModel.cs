// <copyright file="LicenseViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// namespace VeganLife.ViewModels
// {
//    public partial class LoginViewModel : BaseViewModel
//    {
//        [RelayCommand]
//        void UnFocus(object obj)
//        {
//            if (obj == null)
//                return;
//            var view = (LoginPage)obj;
//            if (view == null)
//                return;
//            var entryEmail = view.FindByName("entryEmail") as Entry;
//            if (entryEmail != null && entryEmail.IsFocused)
//            {
//                deviceService.HideKeyboard();
//                entryEmail.Unfocus();
//                return;
//            }
//            var entryPassword = view.FindByName("entryPassword") as Entry;
//            if (entryPassword != null && entryPassword.IsFocused)
//            {
//                deviceService.HideKeyboard();
//                entryPassword.Unfocus();
//            }
//        }
//        [RelayCommand]
//        async Task Register()
//        {
//            await navigationService.NavigataToPage<RegistrationPage>();
//        }
//        [RelayCommand]
//        void Close()
//        {
//            Application.Current.MainPage = new AppShell();
//        }
//        public LoginViewModel()
//            : base()
//        {
//        }
//    }
// }
