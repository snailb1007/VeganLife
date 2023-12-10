// <copyright file="VitaminAndMineralPage.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.Views.MainPageFlyout
{
    using VeganLife.ViewModels.ContentViewModels;
    using VeganLife.Views.Base;

    /// <summary>
    /// Service to handle navigation for shell app.
    /// </summary>
    public partial class VitaminAndMineralPage : BasePage<VitaminAndMineralViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VitaminAndMineralPage"/> class.
        /// </summary>
        /// <param name="vm">vm to processing logic.</param>
        public VitaminAndMineralPage(VitaminAndMineralViewModel vm)
            : base(vm)
        {
            this.InitializeComponent();
        }

        private void VitaminsCollection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
    }
}