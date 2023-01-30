using VeganLife.ViewModels.ContentViewModels;

namespace VeganLife.Views.ContentViews;

public partial class RationPlanPage : ContentPage
{
	public RationPlanPage(RationPlanViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }

    private void PieChart_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        //var chart = sender as LiveChartsCore.SkiaSharpView.Maui.PieChart;
        //Console.WriteLine($"thien==>{chart?.Width} {chart?.Height}");
    }
}