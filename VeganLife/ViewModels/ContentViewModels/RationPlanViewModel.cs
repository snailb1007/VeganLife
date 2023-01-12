using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using LiveChartsCore.SkiaSharpView.VisualElements;

namespace VeganLife.ViewModels.ContentViewModels;

public partial class RationPlanViewModel : ObservableObject
{
    public ISeries[] Series { get; private set; } =
        {
            new PieSeries<float> { Values = new List<float> { 3 }, InnerRadius = 50, MaxOuterRadius = 0.9, Name = "Bữa sáng" },
            new PieSeries<float> { Values = new List<float> { 4 }, InnerRadius = 50, MaxOuterRadius = 1.0, Name = "Bữa trưa" },
            new PieSeries<float> { Values = new List<float> { 2.5f }, InnerRadius = 50, MaxOuterRadius = 0.8, Name = "Bữa tối" },
            new PieSeries<float> { Values = new List<float> { 0.5f }, InnerRadius = 50, MaxOuterRadius = 0.7, Name = "Bữa phụ" }
        };

    public LabelVisual Title { get; private set; } =
        new LabelVisual
        {
            Text = "Phân bổ năng lượng các bữa ăn trong ngày",
            TextSize = 50,
            Padding = new LiveChartsCore.Drawing.Padding(1),
            Paint = new SolidColorPaint(SKColors.DarkSlateGray)
        };

    public RationPlanViewModel()
    {
        Init();
    }

    void Init()
    {
        foreach (var item in Series)
        {
            var x = item as PieSeries<float>;
            x.DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Outer;
        }
    }
}