namespace VeganLife.ViewModels
{
    public partial class WebViewViewModel : BaseViewModel
    {
        [ObservableProperty]
        string _sourceWeb;

        public WebViewViewModel(INavigationService navigationService, IDataService dataService)
            : base(navigationService, dataService)
        {
            // TODO: Update the default URL
            SourceWeb = "https://veganhealthies.wordpress.com/";
            IsLoading = true;
        }

        [RelayCommand]
        private async void WebViewNavigated(WebNavigatedEventArgs e)
        {
            IsLoading = false;
            if (e.Result != WebNavigationResult.Success)
            {
                // TODO: handle failed navigation in an appropriate way
                await Shell.Current.DisplayAlert("Navigation failed", e.Result.ToString(), "OK");
            }
        }

        [RelayCommand]
        private void NavigateBack(WebView webView)
        {
            if (webView.CanGoBack)
            {
                webView.GoBack();
            }
        }

        [RelayCommand]
        private void NavigateForward(WebView webView)
        {
            if (webView.CanGoForward)
            {
                webView.GoForward();
            }
        }

        [RelayCommand]
        private void RefreshPage(WebView webView)
        {
            webView.Reload();
        }

        [RelayCommand]
        private async void OpenInBrowser()
        {
            await Launcher.OpenAsync(SourceWeb);
        }
    }
}
