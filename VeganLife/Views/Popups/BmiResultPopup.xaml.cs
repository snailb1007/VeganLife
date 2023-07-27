// <copyright file="BmiResultPopup.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.Views.Popups
{
    using Mopups.Pages;
    using Mopups.Services;
    using VeganLife.ViewModels.PopupViewModels;

    public partial class BmiResultPopup : PopupPage
    {
        public BmiResultPopup(BmiResultPopupViewmodel vm)
        {
            this.InitializeComponent();
            this.BindingContext = vm;
        }

        private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
        {
            await MopupService.Instance.PopAsync();
        }
    }
}