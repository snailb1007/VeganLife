// <copyright file="PopupNaviService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Mopups.Interfaces;
using Mopups.Pages;
using Mopups.Services;
using VeganLife.Helpers;

namespace VeganLife.Services
{
    public class PopupNaviService : IPopupNaviService
    {
        private readonly IServiceProvider services;

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

        public BaseViewModel GetPopupViewModel(PopupPage? popup) => popup?.BindingContext as BaseViewModel;

        public PopupNaviService(IServiceProvider serviceProvider)
        {
            this.services = serviceProvider;
        }

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
            if (this.GetPopupStackCount() > 0)
            {
                await this.Navigation.PopAsync(animate);
            }
            else
            {
                throw new InvalidOperationException("No pages to navigate back to!");
            }
        }

        public async Task PushAsync<T>(object? param = null, bool animate = true)
            where T : PopupPage
        {
            var toPage = this.ResolvePage<T>();
            if (toPage is not null)
            {
                toPage.NavigatedTo += Page_NavigatedTo;
                var toViewModel = this.GetPopupViewModel(toPage);

                // passing param
                if (toViewModel is not null)
                {
                    await toViewModel.OnNavigatingTo(param);
                }

                ServicesHelper.GetService<IDeviceService>().HideKeyboard();

                // navigate
                await this.Navigation.PushAsync(toPage);

                // subscribe
                toPage.NavigatedFrom += this.Page_NavigatedFrom;
            }
            else
            {
                throw new InvalidOperationException($"Unable to resolve type {typeof(T).FullName}");
            }
        }

        private async void Page_NavigatedTo(object? sender, NavigatedToEventArgs e)
            => await this.CallNavigatedTo(sender as PopupPage);

        private Task CallNavigatedTo(PopupPage? p)
        {
            var fromViewModel = this.GetPopupViewModel(p);
            if (fromViewModel is not null)
            {
                return fromViewModel.OnNavigatedTo();
            }

            return Task.CompletedTask;
        }

        private async void Page_NavigatedFrom(object? sender, NavigatedFromEventArgs e)
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

                await this.CallNavigatedFrom(thisPage, isForwardNavigation);
            }
        }

        private Task CallNavigatedFrom(PopupPage p, bool isForward)
        {
            var fromViewModel = this.GetPopupViewModel(p);

            if (fromViewModel is not null)
            {
                return fromViewModel.OnNavigatedFrom(isForward);
            }

            return Task.CompletedTask;
        }

        private T ResolvePage<T>()
            where T : PopupPage
        {
            return this.services.GetService<T>();
        }
    }
}
