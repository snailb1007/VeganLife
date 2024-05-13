// <copyright file="DetailVitaminAndMineralPage.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using VeganLife.ViewModels.ContentViewModels;

namespace VeganLife.Views.MainPageFlyout.VitaminTab
{
    public partial class DetailVitaminAndMineralPage : ContentPage
    {
        public DetailVitaminAndMineralPage(DetailVitaminAndMineralViewModel vm)
        {
            this.BindingContext = vm;
            this.InitializeComponent();
        }
    }
}