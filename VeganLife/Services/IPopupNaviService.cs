// <copyright file="IPopupNaviService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.Services
{
    using Mopups.Pages;

    public interface IPopupNaviService
    {
        Task PushAsync<T>(object param = null, bool animate = true)
            where T : PopupPage;

        Task PopAsync(bool animate = true);

        Task PopAllAsync(bool animate = true);
    }
}
