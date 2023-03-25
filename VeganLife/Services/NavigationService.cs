using VeganLife.Helpers;
using VeganLife.Views.FoodTab;

namespace VeganLife.Services
{
    public class NavigationService : INavigationService
    {
        readonly IServiceProvider _services;
        Page _mainPage => Application.Current?.MainPage;

        protected INavigation navigation
        {
            get
            {
                var navigation = _mainPage?.Navigation;
                if (navigation is not null)
                    return navigation;
                else
                {
                    //This is not good!
                    if (Debugger.IsAttached)
                        Debugger.Break();
                    throw new Exception();
                }
            }
        }

        public NavigationService(IServiceProvider services) => _services = services;
        public BaseViewModel GetPageViewModedl(Page page) => page?.BindingContext as BaseViewModel;

        public async Task<bool> DisplayAlert(string title, string message, string ok, string cancel)
            => await _mainPage.DisplayAlert(title, message, ok, cancel);

        public async Task DisplayAlert(string title, string message, string ok)
            => await _mainPage.DisplayAlert(title, message, ok);

        public async Task<Page> PopAsync()
        {
            if (navigation.NavigationStack.Count > 1)
                return await navigation.PopAsync();
            throw new InvalidOperationException("No pages to navigate back to!");
        }

        public Task NavigateToRegisPage() => NavigataToPage<RegistrationPage>();
        public Task NavigateToFoodDetail(object foodPreview) => NavigataToPage<FoodDetailPage>(foodPreview);
        public Task NavigateToCategoryPage(object foods) => NavigataToPage<FoodsByCategoryPage>(foods);


        private async Task NavigataToPage<T>(object paramater = null) where T : Page
        {
            var toPage = ResolvePage<T>();
            if (toPage is not null)
            {
                toPage.NavigatedTo += Page_NavigatedTo;
                var toViewModel = GetPageViewModedl(toPage);
                //passing param
                if (toViewModel is not null)
                    await toViewModel.OnNavigatingTo(paramater);
                ServicesHelper.GetService<IDeviceService>().HideKeyboard();
                // navigate
                await navigation.PushAsync(toPage);
                // subscribe
                toPage.NavigatedFrom += Page_NavigatedFrom;
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

        private T ResolvePage<T>() where T : Page => _services.GetService<T>();
    }
}
