// <copyright file="FoodsByCategoryPage.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.Views.FoodTab
{
    using VeganLife.ViewModels.ContentViewModels;

    public partial class FoodsByCategoryPage : ContentPage
    {
        public FoodsByCategoryPage(FoodsByCategoryViewModel vm)
        {
            this.InitializeComponent();
            this.BindingContext = vm;
        }
    }
}