// <copyright file="MainPage.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife
{
    using VeganLife.Views.Base;

    /// <summary>
    /// auto-generated.
    /// </summary>
    public partial class MainPage : BasePage<MainViewModel>
    {
        private readonly MainViewModel viewModel;

        public MainPage(MainViewModel vm)
            : base(vm)
        {
            this.InitializeComponent();
            this.viewModel = vm;
        }

        // private bool processing;

        //private void CarouselView_PositionChanged(object sender, PositionChangedEventArgs e)
        //{
        //    if (this.processing)
        //    {
        //        return;
        //    }

        //    this.processing = true;
        //    var menu = sender as CarouselView;
        //    foreach (var i in menu?.VisibleViews)
        //    {
        //        var img = i.FindByName<Image>("imgMenu");
        //        if (img == null)
        //        {
        //            return;
        //        }

        //        Microsoft.Maui.Controls.ViewExtensions.CancelAnimations(img);
        //        Task.Run(async () => await img.RelRotateTo(360, 5000, Easing.BounceOut));
        //    }

        //    this.processing = false;
        //}

        private void gridTransparent_TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
        {
            this.searchBar.Unfocus();
        }

        private void RefreshView_Refreshing(object sender, EventArgs e)
        {
            viewModel.LoadDataCommand.Execute(null);
            (sender as RefreshView).IsRefreshing = false;
        }
    }
}