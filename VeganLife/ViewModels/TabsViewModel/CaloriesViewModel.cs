// <copyright file="CaloriesViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;

namespace VeganLife.ViewModels.TabsViewModel
{
    public partial class CaloriesViewModel : BaseViewModel
    {
        public ISeries[] Series { get; set; } =
        {
            new StackedColumnSeries<int>
            {
                Values = new List<int> { 3 },
                Stroke = null,
                StackGroup = 0,
            },
            new StackedColumnSeries<int>
            {
                Values = new List<int> { 4 },
                Stroke = null,
                StackGroup = 0,
            },
            new StackedColumnSeries<int>
            {
                Values = new List<int> { 0, 2 },
                Stroke = null,
                StackGroup = 1,
            },
            new StackedColumnSeries<int>
            {
                Values = new List<int> { 0, 5 },
                Stroke = null,
                StackGroup = 1,
            },
            new StackedColumnSeries<int>
            {
                Values = new List<int> { 0, 0, 5 },
                Stroke = null,
                StackGroup = 2,
            },
            new StackedColumnSeries<int>
            {
                Values = new List<int> { 0,  0, 10 },
                Stroke = null,
                StackGroup = 2,
            },
        };

        public Axis[] XAxis { get; set; } =
        {
            new Axis
            {
                Labels = new[] { "Mo 18", "Tu 19", "We 20" },
            },
        };

        public CaloriesViewModel()
            : base()
        {
        }
    }
}
