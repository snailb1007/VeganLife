// <copyright file="NavigationService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.Services
{
    using VeganLife.Helpers;

    public class NavigationService : INavigationService
    {
        private readonly IServiceProvider services;

        private Page mainPage => Application.Current?.MainPage;

        protected INavigation navigation
        {
            get
            {
                var navigation = this.mainPage?.Navigation;
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

        public int GetStackCount() => this.navigation?.NavigationStack?.Count ?? 0;

        public NavigationService(IServiceProvider services) => this.services = services;

        public BaseViewModel GetPageViewModedl(Page page) => page?.BindingContext as BaseViewModel;

        public async Task<bool> DisplayAlert(string title, string message, string ok, string cancel)
            => await this.mainPage.DisplayAlert(title, message, ok, cancel);

        public async Task DisplayAlert(string title, string message, string ok)
            => await this.mainPage.DisplayAlert(title, message, ok);

        public async Task<Page> PopAsync()
        {
            if (this.GetStackCount() > 1)
            {
                return await this.navigation.PopAsync();
            }

            throw new InvalidOperationException("No pages to navigate back to!");
        }

        public async Task PopToRootAsync() => await this.navigation.PopToRootAsync();

        public async Task NavigataToPage<T>(object paramater = null)
            where T : Page
        {
            var toPage = this.ResolvePage<T>();
            if (toPage is not null)
            {
                toPage.NavigatedTo += this.Page_NavigatedTo;
                var toViewModel = this.GetPageViewModedl(toPage);

                // passing param
                if (toViewModel is not null)
                {
                    await toViewModel.OnNavigatingTo(paramater);
                }

                ServicesHelper.GetService<IDeviceService>().HideKeyboard();

                // navigate
                await this.navigation.PushAsync(toPage);

                // subscribe
                toPage.NavigatedFrom += this.Page_NavigatedFrom;
            }
            else
            {
                throw new InvalidOperationException($"Unable to resolve type {typeof(T).FullName}");
            }
        }

        private async void Page_NavigatedTo(object sender, NavigatedToEventArgs e)
            => await CallNavigatedTo(sender as Page);

        private Task CallNavigatedTo(Page p)
        {
            var fromViewModel = GetPageViewModedl(p);
            if (fromViewModel is not null)
                return fromViewModel.OnNavigatedTo();
            return Task.CompletedTask;
        }

        private async void Page_NavigatedFrom(object sender, NavigatedFromEventArgs e)
        {
            //To determine forward navigation, we look at the 2nd to last item on the NavigationStack
            //If that entry equals the sender, it means we navigated forward from the sender to another page
            bool isForwardNavigation = navigation.NavigationStack.Count > 1
                && navigation.NavigationStack[^2] == sender;

            if (sender is Page thisPage)
            {
                if (!isForwardNavigation)
                {
                    thisPage.NavigatedTo -= Page_NavigatedTo;
                    thisPage.NavigatedFrom -= Page_NavigatedFrom;
                }

                await CallNavigatedFrom(thisPage, isForwardNavigation);
            }
        }

        private Task CallNavigatedFrom(Page p, bool isForward)
        {
            var fromViewModel = GetPageViewModedl(p);

            if (fromViewModel is not null)
                return fromViewModel.OnNavigatedFrom(isForward);
            return Task.CompletedTask;
        }

        private T ResolvePage<T>()
            where T : Page
            => this.services.GetService<T>();
    }
}
