// <copyright file="MainPage.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife
{
    using VeganLife.Views.Base;

    public partial class MainPage : BasePage<MainViewModel>
    {
        private readonly MainViewModel viewModel;

        public MainPage(MainViewModel vm)
            : base(vm)
        {
            this.InitializeComponent();
            this.viewModel = vm;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (!this.viewModel.IsLoadDataOnAppearingDone)
            {
                this.viewModel.LoadDataCommand.Execute(null);
            }
        }

        private bool processing;

        private void CarouselView_PositionChanged(object sender, PositionChangedEventArgs e)
        {
            if (this.processing)
            {
                return;
            }

            this.processing = true;
            var menu = sender as CarouselView;
            foreach (var i in menu?.VisibleViews)
            {
                var img = i.FindByName<Image>("imgMenu");
                if (img == null)
                {
                    return;
                }

                Microsoft.Maui.Controls.ViewExtensions.CancelAnimations(img);
                Task.Run(async () => await img.RelRotateTo(360, 5000, Easing.BounceOut));
            }

            this.processing = false;
        }

        private void gridTransparent_TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
        {
            this.searchBar.Unfocus();
        }
    }
}