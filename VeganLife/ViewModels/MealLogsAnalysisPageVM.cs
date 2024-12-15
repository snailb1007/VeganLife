using LiveChartsCore;
using LiveChartsCore.SkiaSharpView.Extensions;
using LiveChartsCore.SkiaSharpView.Maui;

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

            Series = new[] { Protein, Fat, Carbs }.AsPieSeries((v, s) =>
            {
                s.MaxRadialColumnWidth = 60;

            });

            return base.OnNavigatingTo(parameter);
        }

        public override Task ViewIsRemovedAsync()
        {
            base.ViewIsRemovedAsync();
            return Task.CompletedTask;
        }
    }
}
