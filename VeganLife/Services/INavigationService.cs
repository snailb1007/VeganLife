namespace VeganLife.Services
{
    public interface INavigationService
    {
        Task<Page> PopAsync();
        Task<bool> DisplayAlert(string title, string message, string ok, string cancel);
        Task DisplayAlert(string title, string message, string ok);
        BaseViewModel GetPageViewModedl(Page page);

        Task NavigateToRegisPage();
    }
}
