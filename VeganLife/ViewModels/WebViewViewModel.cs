//// <copyright file="WebViewViewModel.cs" company="PlaceholderCompany">
//// Copyright (c) PlaceholderCompany. All rights reserved.
//// </copyright>

//using System.Threading.Tasks;
//using VeganLife.Resources.Translations;

//namespace VeganLife.ViewModels
//{
//    /// <summary>
//    /// vm for WebViewPage.
//    /// </summary>
//    public partial class WebViewViewModel : BaseViewModel
//    {
//        [ObservableProperty]
//        private string sourceWeb;

//        /// <summary>
//        /// Initializes a new instance of the <see cref="WebViewViewModel"/> class.
//        /// </summary>
//        public WebViewViewModel()
//            : base()
//        {
//            // TODO: Update the default URL
//            this.SourceWeb = "https://veganhealthies.wordpress.com/";
//            this.IsLoading = true;
//        }

//        [RelayCommand]
//        private async Task WebViewNavigated(WebNavigatedEventArgs e)
//        {
//            this.IsLoading = false;
//            if (e.Result != WebNavigationResult.Success)
//            {
//                // TODO: handle failed navigation in an appropriate way
//                await Shell.Current.DisplayAlert(AppResources.navigationFailed_alert_common, e.Result.ToString(), AppResources.ok_common);
//            }
//        }

//        [RelayCommand]
//        private void NavigateBack(WebView webView)
//        {
//            if (webView.CanGoBack)
//            {
//                webView.GoBack();
//            }
//        }

//        [RelayCommand]
//        private void NavigateForward(WebView webView)
//        {
//            if (webView.CanGoForward)
//            {
//                webView.GoForward();
//            }
//        }

//        [RelayCommand]
//        private void RefreshPage(WebView webView)
//        {
//            webView.Reload();
//        }

//        [RelayCommand]
//        private async Task OpenInBrowser()
//        {
//            await Launcher.OpenAsync(this.SourceWeb);
//        }

//        [RelayCommand]
//        private void ShowMainMenu()
//        {
//            AppShell.ShowFlyOut();
//        }
//    }
//}
