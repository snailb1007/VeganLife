namespace VeganLife.Services
{
    public class NavigationService : INavigationService
    {
        private Page _mainPage => Application.Current.MainPage;

        public async Task<bool> DisplayAlert(string title, string message, string ok, string cancel)
        {
            return await _mainPage.DisplayAlert(title, message, ok, cancel);
        }

        public async Task DisplayAlert(string title, string message, string ok)
        {
            await _mainPage.DisplayAlert(title, message, ok);
        }

        public async Task<Page> PopAsync()
        {
            return await _mainPage.Navigation.PopAsync();
        }

        public async Task PushAsync(Page page)
        {
            await _mainPage.Navigation.PushAsync(page);
        }
    }
}
