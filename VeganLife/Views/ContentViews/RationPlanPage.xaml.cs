using VeganLife.ViewModels.ContentViewModels;
using VeganLife.Views.Base;

namespace VeganLife.Views.ContentViews;

public partial class RationPlanPage : BasePage<RationPlanViewModel>
{
    public RationPlanPage(RationPlanViewModel vm) : base(vm)
    {
        InitializeComponent();
    }

    private void PieChart_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        //var chart = sender as LiveChartsCore.SkiaSharpView.Maui.PieChart;
        //Console.WriteLine($"thien==>{chart?.Width} {chart?.Height}");
    }
}