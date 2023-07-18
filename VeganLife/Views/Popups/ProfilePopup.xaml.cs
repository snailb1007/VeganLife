// <copyright file="ProfilePopup.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.Views.Popups
{
    using Mopups.Pages;
    using VeganLife.ViewModels.PopupViewModels;

    /// <summary>
    /// behind for  ProfilePopup.
    /// </summary>
    public partial class ProfilePopup : PopupPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProfilePopup"/> class.
        /// </summary>
        public ProfilePopup(ProfilePopupViewModel vm)
        {
            this.InitializeComponent();
            this.BindingContext = vm;
        }
    }
}