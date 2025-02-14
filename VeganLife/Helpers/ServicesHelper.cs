// <copyright file="ServicesHelper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using VeganLife.Views.Popups;

namespace VeganLife.Helpers
{
    public static class ServicesHelper
    {
        public static BaseViewModel GetCurrentViewModel()
        {
            var popupService = FFImageLoading.Helpers.ServiceHelper.GetService<IPopupNaviService>();
            var currentPopup = popupService.GetLastMopupPage();
            if (popupService.GetPopupStackCount() > 0
                && currentPopup != null
                && currentPopup is not LoadingPopup)
            {
                // Mop-up
                return currentPopup?.BindingContext as BaseViewModel;
            }
            else if (Shell.Current?.CurrentPage != null)
            {
                return Shell.Current.CurrentPage.BindingContext as BaseViewModel;
            }
            else
            {
                return Application.Current?.Windows[0]?.Page?.BindingContext as BaseViewModel;
            }
        }

        // get current viewmodel by type
        //public static T GetCurrentViewModel<T>()
        //    where T : BaseViewModel
        //{
        //    return GetCurrentViewModel() as T;
        //}

        public static async Task OpenViaBrowserAsync(string uri)
        {
            try
            {
                await Browser.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
            }
            catch (Exception ex) when (ex.ToString().Contains("ActivityNotFoundException"))
            {
                //await AlertHelper.ShowErrorAlertAsync(I18nHelper.Get("Common_Error_BrowserNotFound"));
            }
        }

        public static bool GetNetworkStatus()
        {
            return Connectivity.Current.NetworkAccess == NetworkAccess.Internet;
        }
    }
}
