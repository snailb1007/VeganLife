// <copyright file="RationPlanViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.ViewModels.ContentViewModels
{
    using Microcharts;
    using SkiaSharp;
    using VeganLife.Resources.Translations;

    /// <summary>
    /// vm for RationPlanPage.
    /// </summary>
    public partial class RationPlanViewModel : BaseViewModel
    {
        [ObservableProperty]
        private DonutChart plantChart;

        private ChartEntry[] CreateChartData()
        {
            return new ChartEntry[]
            {
                new(40)
                {
                    Label = AppResources.lunch_rationPlantPage,
                    ValueLabel = "40%",
                    Color = SKColor.Parse("#f9806f"),
                    ValueLabelColor = SKColor.Parse("#f9806f"),
                },
                new(30)
                {
                    Label = AppResources.breakfast_rationPlantPage,
                    ValueLabel = "30",
                    Color = SKColor.Parse("#27c7e0"),
                },
                new(25)
                {
                    Label = AppResources.dinner_rationPlantPage,
                    ValueLabel = "25",
                    Color = SKColor.Parse("#fec45a"),
                },
                new(5)
                {
                    Label = AppResources.sideMeal_rationPlantPage,
                    ValueLabel = "5",
                    Color = SKColor.Parse("#d0d0d2"),
                },
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RationPlanViewModel"/> class.
        /// </summary>
        public RationPlanViewModel()
            : base()
        {
            this.Init();
        }

        private void Init()
        {
            this.PlantChart = new DonutChart
            {
                Entries = this.CreateChartData(),
                LabelTextSize = 35,
                HoleRadius = 0.25f,
            };
        }
    }
}