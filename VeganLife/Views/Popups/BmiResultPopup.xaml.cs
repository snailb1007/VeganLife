// <copyright file="BmiResultPopup.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Mopups.Pages;
using Mopups.Services;
using VeganLife.ViewModels.PopupViewModels;

namespace VeganLife.Views.Popups
{
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