using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace VeganLife.ViewModels
{
    public partial class MealLogsAnalysisPageVM : BaseViewModel
    {
        [ObservableProperty]
        private double _calories;

        [ObservableProperty]
        private double _protein;

        [ObservableProperty]
        private double _fat;

        [ObservableProperty]
        private double _carbs;

        [ObservableProperty]
        public IEnumerable<ISeries> _series;

        public override Task OnNavigatingTo(object parameter)
        {
            var data = parameter as List<NutritionMealLogModel>;
            if (data != null)
            {
                Calories = data.Sum(x => x.DisplayCalories);
                Protein = data.Sum(x => x.DisplayProtein);
                Fat = data.Sum(x => x.DisplayFat);
                Carbs = data.Sum(x => x.DisplayCarbs);
            }

            Series =
            [
                new PieSeries<double>
                {
                    Values = [Protein],
                    Name = "Protein",
                    MaxRadialColumnWidth = 60,
                    DataLabelsPaint = new SolidColorPaint(SKColors.Black),
                    DataLabelsSize = 18,
                    DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Middle,
                    DataLabelsFormatter = p => Protein.ToString("N2")
                },
                new PieSeries<double>
                {
                    Values = new[] {Fat},
                    Name = "Fat",
                    MaxRadialColumnWidth = 60,
                    DataLabelsPaint = new SolidColorPaint(SKColors.Black),
                    DataLabelsSize = 20,
                    DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Middle,
                    DataLabelsFormatter = p => Fat.ToString("N2")
                },
                new PieSeries<double>
                {
                    Values = new[] {Carbs},
                    Name = "Carbs",
                    MaxRadialColumnWidth = 60,
                    DataLabelsPaint = new SolidColorPaint(SKColors.Black),
                    DataLabelsSize = 18,
                    DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Middle,
                    DataLabelsFormatter = p => Carbs.ToString("N2")
                }
            ];

            return base.OnNavigatingTo(parameter);
        }

        public override Task ViewIsRemovedAsync()
        {
            base.ViewIsRemovedAsync();
            return Task.CompletedTask;
        }
    }
}
