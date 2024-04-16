// <copyright file="ShellItemHandlerAndroid.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.Platforms.Android.HandlerAndroid
{
    using Microsoft.Maui.Controls.Platform.Compatibility;
    using VeganLife.Helpers;

    public class ShellItemHandlerAndroid : ShellItemRenderer
    {
        public ShellItemHandlerAndroid(IShellContext shellContext)
            : base(shellContext)
        {
        }

        /// <inheritdoc/>
        protected override void OnTabReselected(ShellSection shellSection)
        {
            base.OnTabReselected(shellSection);
            this.DisplayedPage.Dispatcher.Dispatch(async () => await PerformTabReselectedAsync());
        }

        protected override bool OnItemSelected(global::Android.Views.IMenuItem item)
        {
            if (Shell.Current.IsBusy
                || (ServicesHelper.GetCurrentViewModel()?.IsLoading ?? false))
            {
                return false;
            }

            return base.OnItemSelected(item);
        }

        async Task PerformTabReselectedAsync()
        {
            var currentVM = ServicesHelper.GetCurrentViewModel();
            if (currentVM != null)
            {
                INavigationService navigationService = ServicesHelper.GetService<INavigationService>();
                if (navigationService is null)
                    return;
                if (navigationService?.GetStackCount() == 1 && currentVM is IScrollToTop)
                {
                    ((IScrollToTop)currentVM).ScrollToTop();
                }
                else if (!currentVM.IsLoading)
                {
                    await navigationService?.PopToRootAsync()!;
                }
            }
        }
    }
}
