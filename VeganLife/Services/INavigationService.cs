// <copyright file="INavigationService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.Services
{
    public interface INavigationService
    {
        Task<Page> PopAsync();

        Task PopToRootAsync();

        Task<bool> DisplayAlert(string title, string message, string ok, string cancel);

        Task DisplayAlert(string title, string message, string ok);

        BaseViewModel GetPageViewModedl(Page page);

        Task NavigateToPage<T>(object paramater = null)
            where T : Page;

        int GetStackCount();
    }
}
