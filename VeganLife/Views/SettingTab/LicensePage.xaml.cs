// <copyright file="LicensePage.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.Views.SettingTab
{
    using VeganLife.ViewModels.ContentViewModels;

    public partial class LicensePage : ContentPage
    {
        public LicensePage(LicenseViewModel vm)
        {
            this.InitializeComponent();
            this.BindingContext = vm;
        }
    }
}