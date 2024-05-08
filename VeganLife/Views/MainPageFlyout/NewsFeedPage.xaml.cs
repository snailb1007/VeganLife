// <copyright file="NewsFeedPage.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using VeganLife.Views.Base;

namespace VeganLife.Views.MainPageFlyout
{
    public partial class NewsFeedPage : BasePage<NewsFeedViewModel>
    {
        public NewsFeedPage(NewsFeedViewModel vm)
            : base(vm)
        {
            this.InitializeComponent();
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