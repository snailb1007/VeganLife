// <copyright file="RationPlanViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.ViewModels.ContentViewModels
{
    using LiveChartsCore;
    using LiveChartsCore.SkiaSharpView;
    using LiveChartsCore.SkiaSharpView.Painting;
    using LiveChartsCore.SkiaSharpView.VisualElements;
    using SkiaSharp;

    public partial class RationPlanViewModel : BaseViewModel
    {
        public ISeries[] Series { get; private set; } =
        {
            new PieSeries<short>
            {
                Values = new List<short> { 40 },
                Fill = new SolidColorPaint(SKColor.Parse("#f9806f")),
                MaxOuterRadius = 1.0,
                Name = "Bữa trưa",
            },
            new PieSeries<short>
            {
                Values = new List<short> { 30 },
                Fill = new SolidColorPaint(SKColor.Parse("#27c7e0")),
                MaxOuterRadius = 0.9,
                Name = "Bữa sáng",
            },
            new PieSeries <short>
            {
                Values = new List <short> { 25 },
                Fill = new SolidColorPaint(SKColor.Parse("#fec45a")),
                MaxOuterRadius = 0.8,
                Name = "Bữa tối",
            },
            new PieSeries <short>
            {
                Values = new List <short> { 5 },
                Fill = new SolidColorPaint(SKColor.Parse("#d0d0d2")),
                MaxOuterRadius = 0.7,
                Name = "Bữa phụ",
            },
        };

        public LabelVisual Title { get; private set; } =
            new LabelVisual
            {
                Text = "Phân bổ năng lượng các bữa ăn trong ngày",
                TextSize = 50,
                Padding = new LiveChartsCore.Drawing.Padding(1),
                Paint = new SolidColorPaint(SKColors.DarkSlateGray),
            };

        public RationPlanViewModel(INavigationService navigationService, IDataService dataService)
            : base(navigationService, dataService)
        {
            this.Init();
        }

        private void Init()
        {
            foreach (var item in this.Series)
            {
                var serie = item as PieSeries<short>;

                // shape
                serie.InnerRadius = 200;

                // label
                serie.DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Middle;
                serie.DataLabelsPaint = new SolidColorPaint(SKColors.Black);
                serie.DataLabelsSize = 40;
                serie.DataLabelsFormatter = p => p.PrimaryValue.ToString() + "%";
            }
        }
    }
}