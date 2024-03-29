// <copyright file="LicensePage.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.Views.SettingTab
{
    using VeganLife.ViewModels.ContentViewModels;

    /// <summary>
    /// class for LicensePage xaml.
    /// </summary>
    public partial class LicensePage : ContentPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LicensePage"/> class.
        /// </summary>
        public LicensePage(LicenseViewModel vm)
        {
            this.InitializeComponent();
            this.BindingContext = vm;
        }
    }
}