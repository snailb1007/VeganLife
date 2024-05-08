// <copyright file="ProfilePopup.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Mopups.Pages;
using VeganLife.ViewModels.PopupViewModels;

namespace VeganLife.Views.Popups
{
    /// <summary>
    /// behind for  ProfilePopup.
    /// </summary>
    public partial class ProfilePopup : PopupPage
    {
        private ProfilePopupViewModel viewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfilePopup"/> class.
        /// </summary>
        public ProfilePopup(ProfilePopupViewModel vm)
        {
            this.InitializeComponent();
            this.BindingContext = vm;
            viewModel = vm;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.ViewAppearing();
        }
    }
}