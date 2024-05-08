// <copyright file="FoodDetailPage.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using VeganLife.ViewModels.ContentViewModels;
using VeganLife.Views.Base;

namespace VeganLife.Views.MainPageFlyout.FoodTab
{
    public partial class FoodDetailPage : BasePage<FoodDetailViewModel>
    {
        private double widthOfTabView;

        public double WidthOfTabView
        {
            get => this.widthOfTabView;
            set => SetProperty(ref this.widthOfTabView, value);
        }

        public FoodDetailPage(FoodDetailViewModel vm)
            : base(vm)
        {
            this.InitializeComponent();
            WidthOfTabView = App.MainSize / 2;
        }

        private void Image_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        }

        private void VerticalStackLayout_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        }

        private void GoNutriFact_SwipeGesture_Swiped(object sender, SwipedEventArgs e)
        {
        }

        private void tabItemNutritionFacts_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        }
    }
}