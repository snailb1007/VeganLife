// <copyright file="FoodDetailPage.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using VeganLife.ViewModels.ContentViewModels;
using VeganLife.Views.Base;

namespace VeganLife.Views.MainPageFlyout.FoodTab
{
    public partial class FoodDetailPage
    {
        private double _widthOfTabView;

        public double WidthOfTabView
        {
            get => this._widthOfTabView;
            set => SetProperty(ref this._widthOfTabView, value);
        }

        public FoodDetailPage(FoodDetailViewModel vm)
            : base(vm)
        {
            this.InitializeComponent();
            WidthOfTabView = App.MainWidthSize / 2;
        }
    }
}