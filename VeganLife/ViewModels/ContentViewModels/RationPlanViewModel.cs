using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;

namespace VeganLife.ViewModels.ContentViewModels;

public partial class RationPlanViewModel : ObservableObject
{
    [ObservableProperty]
    public ISeries[] _series;

    public RationPlanViewModel()
    {
        Init();
    }

    void Init()
    {
        Series = new ISeries[]
            {
                new PieSeries<double> { Values = new double[] { 2 } },
                new PieSeries<double> { Values = new double[] { 4 } },
                new PieSeries<double> { Values = new double[] { 1 } },
                new PieSeries<double> { Values = new double[] { 4 } },
                new PieSeries<double> { Values = new double[] { 3 } }
            };
    }
}