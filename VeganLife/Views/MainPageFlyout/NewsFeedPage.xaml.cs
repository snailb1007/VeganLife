// <copyright file="NewsFeedPage.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using VeganLife.Helpers.Extensions;
using VeganLife.Views.Base;

namespace VeganLife.Views.MainPageFlyout
{
    public partial class NewsFeedPage: IBaseRootPage
    {
        public bool IsAnimated { get; set; }

        public NewsFeedPage(NewsFeedViewModel vm)
            : base(vm)
        {
            this.InitializeComponent();
        }

        public void OnClosedShellFlyout()
        {
            this.AnimateCloseShellMenu(this.mainGridContent);
        }

        public void OnOpenedShellFlyout()
        {
            this.AnimateShellMenu(this.mainGridContent);
        }

        private void DiscoverMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender == null)
            {
                return;
            }

            (this.BindingContext as NewsFeedViewModel).SelectDiscoveryMenuCommand.Execute(e.CurrentSelection);
            this.collectionFeeds.ScrollTo(0, 0);
        }
    }
}