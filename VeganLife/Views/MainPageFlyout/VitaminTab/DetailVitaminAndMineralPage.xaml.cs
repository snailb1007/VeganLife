// <copyright file="DetailVitaminAndMineralPage.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using VeganLife.ViewModels.ContentViewModels;
using VeganLife.Views.Base;

namespace VeganLife.Views.MainPageFlyout.VitaminTab
{
    public partial class DetailVitaminAndMineralPage : BasePage<DetailVitaminAndMineralViewModel>
    {
        public DetailVitaminAndMineralPage(DetailVitaminAndMineralViewModel vm)
            : base(vm)
        {
            this.InitializeComponent();
        }
    }
}