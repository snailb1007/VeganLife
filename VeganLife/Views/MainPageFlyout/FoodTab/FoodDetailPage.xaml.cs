// <copyright file="FoodDetailPage.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using VeganLife.ViewModels.ContentViewModels;

namespace VeganLife.Views.MainPageFlyout.FoodTab
{
    public partial class FoodDetailPage
    {
        public double WidthOfTabView { get; set; }

        public FoodDetailPage(FoodDetailViewModel vm)
            : base(vm)
        {
            this.InitializeComponent();
            WidthOfTabView = App.MainWidthSize / 2;
        }
    }
}