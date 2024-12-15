// <copyright file="PopupNaviService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AsyncAwaitBestPractices;
using Mopups.Interfaces;
using Mopups.Pages;
using Mopups.Services;
using VeganLife.Helpers.Extensions;

namespace VeganLife.Services
{
    public class PopupNaviService : IPopupNaviService
    {
        private IPopupNavigation Navigation
        {
            get
            {
                var navigation = MopupService.Instance;
                if (navigation is not null)
                {
                    return navigation;
                }
                else
                {
                    // This is not good!
                    if (Debugger.IsAttached)
                    {
                        Debugger.Break();
                    }

                    throw new Exception();
                }
            }
        }

        public int GetPopupStackCount() => this.Navigation?.PopupStack?.Count ?? 0;

        public async Task PopAllAsync(bool animate = true)
        {
            if (this.GetPopupStackCount() > 0)
            {
                await this.Navigation.PopAllAsync(animate);
            }

            throw new InvalidOperationException("No pages to navigate back to!");
        }

        public async Task PopAsync(bool animate = true)
        {
            if (this.GetPopupStackCount() < 0)
            {
                throw new InvalidOperationException("No pages to navigate back to!");
            }

            await this.Navigation.PopAsync(animate);
        }

        public async Task PushAsync<T>(object param = null, bool animate = true)
            where T : PopupPage
        {
            var toPage = FFImageLoading.Helpers.ServiceHelper.GetService<T>();
            if (toPage is null)
            {
                throw new InvalidOperationException($"Unable to resolve type {typeof(T).FullName}");
            }

            toPage.NavigatedTo += Page_NavigatedTo;
            var toViewModel = GetPopupViewModel(toPage);

            // passing param
            if (toViewModel is not null)
            {
                await toViewModel.OnNavigatingTo(param);
            }

            FFImageLoading.Helpers.ServiceHelper.GetService<IDeviceService>().HideKeyboard();

            // navigate
            await this.Navigation.PushAsync(toPage);

            // subscribe
            toPage.NavigatedFrom += this.Page_NavigatedFrom;
        }

        public PopupPage GetLastMopupPage()
        {
            if (this.GetPopupStackCount() < 1)
            {
                return null;
            }

            var result = this.Navigation.PopupStack.Last();
            return result;
        }

        private static BaseViewModel GetPopupViewModel(PopupPage popup) => popup?.BindingContext as BaseViewModel;

        private void Page_NavigatedTo(object sender, NavigatedToEventArgs e)
            => this.CallNavigatedTo(sender as PopupPage).SafeFireAndForget(onException: ex => ex.LogError());

        private Task CallNavigatedTo(PopupPage p)
        {
            var fromViewModel = GetPopupViewModel(p);
            return fromViewModel is not null ? fromViewModel.OnNavigatedTo() : Task.CompletedTask;
        }

        private void Page_NavigatedFrom(object sender, NavigatedFromEventArgs e)
        {
            // To determine forward navigation, we look at the 2nd to last item on the NavigationStack
            // If that entry equals the sender, it means we navigated forward from the sender to another page
            bool isForwardNavigation = this.GetPopupStackCount() > 1
                && this.Navigation.PopupStack[^2] == sender;

            if (sender is PopupPage thisPage)
            {
                if (!isForwardNavigation)
                {
                    thisPage.NavigatedTo -= this.Page_NavigatedTo;
                    thisPage.NavigatedFrom -= this.Page_NavigatedFrom;
                }

                this.CallNavigatedFrom(thisPage, isForwardNavigation).SafeFireAndForget(onException: ex => ex.LogError());
            }
        }

        private Task CallNavigatedFrom(PopupPage p, bool isForward)
        {
            var fromViewModel = GetPopupViewModel(p);

            return fromViewModel is not null ? fromViewModel.OnNavigatedFrom(isForward) : Task.CompletedTask;
        }
    }
}
