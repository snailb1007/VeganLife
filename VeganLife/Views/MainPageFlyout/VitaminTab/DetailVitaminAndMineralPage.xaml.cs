// <copyright file="DetailVitaminAndMineralPage.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.Views.MainPageFlyout.VitaminTab
{
    using VeganLife.ViewModels.ContentViewModels;

    public partial class DetailVitaminAndMineralPage : ContentPage
    {
        public DetailVitaminAndMineralPage(DetailVitaminAndMineralViewModel vm)
        {
            this.BindingContext = vm;
            this.InitializeComponent();
        }
    }
}