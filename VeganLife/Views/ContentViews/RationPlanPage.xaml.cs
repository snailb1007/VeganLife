// <copyright file="RationPlanPage.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.Views.ContentViews
{
    using VeganLife.ViewModels.ContentViewModels;
    using VeganLife.Views.Base;

    /// <summary>
    /// class for RationPlanPage xaml.
    /// </summary>
    public partial class RationPlanPage : BasePage<RationPlanViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RationPlanPage"/> class.
        /// </summary>
        public RationPlanPage(RationPlanViewModel vm)
            : base(vm)
        {
            this.InitializeComponent();
        }
    }
}