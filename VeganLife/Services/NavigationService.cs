// <copyright file="NavigationService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AsyncAwaitBestPractices;
using VeganLife.Helpers;
using VeganLife.Helpers.Extensions;

namespace VeganLife.Services
{
    /// <summary>
    /// Service to handle navigation for shell app.
    /// </summary>
    public class NavigationService : INavigationService
    {
        private INavigation Navigation
        {
            get
            {
                var navigation = Shell.Current?.CurrentItem.CurrentItem.Navigation;
                if (navigation is not null)
                {
                    return navigation;
                }

                navigation = Application.Current?.Windows[0]?.Page.Navigation;
                if (navigation is not null)
                {
                    return navigation;
                }

                // This is not good!
                if (Debugger.IsAttached)
                {
                    Debugger.Break();
                }

                throw new Exception();
            }
        }

        private readonly IServiceProvider _services;

        private Page MainPage => Application.Current?.Windows[0].Page;

        // private IDispatcher? dispatcher => Application.Current?.Dispatcher;

        /// <summary>
        /// Get vm from page.
        /// </summary>
        /// <returns>Total currently stack of navigation.</returns>
        public int GetStackCount() => this.Navigation?.NavigationStack?.Count ?? 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationService"/> class.
        /// </summary>
        public NavigationService(IServiceProvider services)
        {
            this._services = services;
        }

        /// <summary>
        /// Get vm from page.
        /// </summary>
        /// <param name="page">Page to get BindingContext.</param>
        /// <returns>BaseViewModel.</returns>
        public BaseViewModel GetPageViewModel(Page page) => (page?.BindingContext as BaseViewModel);

        /// <inheritdoc/>
        public async Task<bool> DisplayAlert(string title, string message, string ok, string cancel)
            => await this.MainPage?.DisplayAlert(title, message, ok, cancel);

        /// <inheritdoc/>
        public async Task DisplayAlert(string title, string message, string ok)
            => await this.MainPage?.DisplayAlert(title, message, ok);

        /// <inheritdoc/>
        public async Task<Page> PopAsync()
        {
            if (this.GetStackCount() > 1)
            {
                return await this.Navigation.PopAsync();
            }

            throw new InvalidOperationException("No pages to navigate back to!");
        }

        /// <inheritdoc/>
        public async Task PopToRootAsync() => await this.Navigation.PopToRootAsync();

        /// <inheritdoc/>
        public async Task NavigateToPage<T>(BaseViewModel currentViewModel = null, object parameter = null)
            where T : Page
        {
            var toPage = this.ResolvePage<T>();
            if (toPage is not null)
            {
                toPage.NavigatedTo += this.Page_NavigatedTo;
                var toViewModel = this.GetPageViewModel(toPage);

                // passing param
                if (toViewModel is not null)
                {
                    await toViewModel.OnNavigatingTo(parameter);
                }

                FFImageLoading.Helpers.ServiceHelper.GetService<IDeviceService>().HideKeyboard();

                // navigate
                MainThread.BeginInvokeOnMainThread(async void () =>
                {
                    try
                    {
                        await (currentViewModel?.NavigationViewModel ?? this.Navigation).PushAsync(toPage);
                    }
                    catch (Exception e)
                    {
                        e.LogError();
                    }
                });
            }
            else
            {
                throw new InvalidOperationException($"Unable to resolve type {typeof(T).FullName}");
            }
        }

        private void Page_NavigatedTo(object sender, NavigatedToEventArgs e)
            => this.CallNavigatedTo((sender as Page)).SafeFireAndForget(onException: ex => ex.LogError());

        private Task CallNavigatedTo(Page p)
        {
            var fromViewModel = this.GetPageViewModel(p);
            return fromViewModel is not null ? fromViewModel.OnNavigatedTo() : Task.CompletedTask;
        }

        private void Page_NavigatedFrom(object sender, NavigatedFromEventArgs e)
        {
            // To determine forward navigation, we look at the 2nd to last item on the NavigationStack
            // If that entry equals the sender, it means we navigated forward from the sender to another page
            var isForwardNavigation = this.Navigation.NavigationStack.Count > 1
                                      && this.Navigation.NavigationStack[^2] == sender;

            if (sender is not Page thisPage)
            {
                return;
            }

            if (!isForwardNavigation)
            {
                thisPage.NavigatedTo -= this.Page_NavigatedTo;
            }

            this.CallNavigatedFrom(thisPage, isForwardNavigation).SafeFireAndForget(onException: ex => ex.LogError());
        }

        private Task CallNavigatedFrom(Page p, bool isForward)
        {
            var fromViewModel = this.GetPageViewModel(p);

            return fromViewModel is not null ? fromViewModel.OnNavigatedFrom(isForward) : Task.CompletedTask;
        }

        private T ResolvePage<T>()
            where T : Page
            => this._services?.GetService<T>();
    }
}
