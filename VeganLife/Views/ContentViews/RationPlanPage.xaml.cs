// <copyright file="RationPlanPage.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.Views.ContentViews
{
    using VeganLife.ViewModels.ContentViewModels;
    using VeganLife.Views.Base;

    public partial class RationPlanPage : BasePage<RationPlanViewModel>
    {
        public RationPlanPage(RationPlanViewModel vm)
            : base(vm)
        {
            this.InitializeComponent();
        }

        private void PieChart_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            // var chart = sender as LiveChartsCore.SkiaSharpView.Maui.PieChart;
            // Console.WriteLine($"thien==>{chart?.Width} {chart?.Height}");
        }
    }
}