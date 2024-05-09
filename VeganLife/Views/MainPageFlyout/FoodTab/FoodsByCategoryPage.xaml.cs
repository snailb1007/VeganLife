// <copyright file="FoodsByCategoryPage.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using VeganLife.ViewModels.ContentViewModels;

namespace VeganLife.Views.MainPageFlyout.FoodTab
{
    public partial class FoodsByCategoryPage : ContentPage
    {
        public FoodsByCategoryPage(FoodsByCategoryViewModel vm)
        {
            this.InitializeComponent();
            this.BindingContext = vm;
        }
    }
}