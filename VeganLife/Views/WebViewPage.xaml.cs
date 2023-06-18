// <copyright file="WebViewPage.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.Views
{
    public partial class WebViewPage : ContentPage
    {
        public WebViewPage(WebViewViewModel vm)
        {
            this.InitializeComponent();
            this.BindingContext = vm;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            AppShell.ShowFlyout();
        }
    }
}