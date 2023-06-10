namespace VeganLife.Services
{
    public interface INavigationService
    {
        Task<Page> PopAsync();
        Task PopToRootAsync();
        Task<bool> DisplayAlert(string title, string message, string ok, string cancel);
        Task DisplayAlert(string title, string message, string ok);
        BaseViewModel GetPageViewModedl(Page page);
        Task NavigataToPage<T>(object paramater = null) where T : Page;
        int GetStackCount();
    }
}
