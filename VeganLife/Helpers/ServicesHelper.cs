// <copyright file="ServicesHelper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Mopups.Interfaces;

namespace VeganLife.Helpers
{
    public static class ServicesHelper
    {
        public static T GetService<T>() => IPlatformApplication.Current.Services.GetService<T>();

        public static BaseViewModel? GetCurrentViewModel()
        {
            if (GetService<IPopupNaviService>().GetPopupStackCount() > 0)
            {
                // Mop-up
                return GetService<IPopupNavigation>().PopupStack.LastOrDefault()?.BindingContext
                       as BaseViewModel;
            }
            else if (Shell.Current?.CurrentPage != null)
            {
                return Shell.Current.CurrentPage.BindingContext as BaseViewModel;
            }
            else
            {
                return Application.Current?.MainPage?.BindingContext as BaseViewModel;
            }
        }

        // get current viewmodel by type
        public static T GetCurrentViewModel<T>() where T : BaseViewModel
        {
            return GetCurrentViewModel() as T;
        }

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
    }
}
