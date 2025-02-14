// <copyright file="MainPage.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using VeganLife.Helpers.Extensions;
using VeganLife.Views.Base;

namespace VeganLife
{
    /// <summary>
    /// auto-generated.
    /// </summary>
    public partial class MainPage : IBaseRootPage
    {
        public MainViewModel ViewModel { get; private set; }

        public bool IsAnimated { get; set; }

        public MainPage(MainViewModel vm)
            : base(vm)
        {
            this.InitializeComponent();
            this.ViewModel = vm;
        }

        private void GridTransparentTapped(object sender, TappedEventArgs e)
        {
            this.searchBar.Unfocus();
        }

        private void RefreshView_Refreshing(object sender, EventArgs e)
        {
            ViewModel.LoadDataCommand.Execute(null);
            if (sender is RefreshView refreshView)
            {
                refreshView.IsRefreshing = false;
            }
        }

        public void OnOpenedShellFlyout()
        {
            this.AnimateShellMenu(mainGridContent);
        }

        public void OnClosedShellFlyout()
        {
            this.AnimateCloseShellMenu(mainGridContent);
        }
    }
}