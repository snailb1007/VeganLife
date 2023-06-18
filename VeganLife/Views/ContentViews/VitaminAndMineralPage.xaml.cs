// <copyright file="VitaminAndMineralPage.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.Views.ContentViews
{
    using VeganLife.ViewModels.ContentViewModels;
    using VeganLife.Views.Base;

    public partial class VitaminAndMineralPage : BasePage<VitaminAndMineralViewModel>
    {
        // private readonly VitaminAndMineralViewModel viewModel;

        public VitaminAndMineralPage(VitaminAndMineralViewModel vm)
            : base(vm)
        {
            this.InitializeComponent();

            // this.viewModel = vm;
        }

        private void vitaminsCollection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
    }
}