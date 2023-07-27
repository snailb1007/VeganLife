// <copyright file="ServicesHelper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Mopups.Interfaces;

namespace VeganLife.Helpers
{
    public static class ServicesHelper
    {
        public static T GetService<T>() => MauiApplication.Current.Services.GetService<T>();

        public static BaseViewModel GetCurrentViewModel()
        {
            if (GetService<IPopupNaviService>().GetPopupStackCount() > 0)
            {
                // Mopup
                return GetService<IPopupNavigation>().PopupStack.LastOrDefault().BindingContext as BaseViewModel;
            }
            else if (Shell.Current?.CurrentPage != null)
            {
                return Shell.Current.CurrentPage.BindingContext as BaseViewModel;
            }
            else
            {
                return Application.Current.MainPage.BindingContext as BaseViewModel;
            }
        }
    }
}
