using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using VeganLife.Data.LocalData;
using VeganLife.Helpers;
using VeganLife.Helpers.AppSetting;

namespace VeganLife.ViewModels.ToolsFlyoutViewModel
{
    public partial class BmiCalculatorViewModel : BaseViewModel, IQueryAttributable
    {
        [ObservableProperty]
        private UserInfo localUserInfo;
        [ObservableProperty]
        private double goalWeight;
        [ObservableProperty]
        private double differentGoalWeight;

        [ObservableProperty]
        public ISeries[] series;
        public BmiCalculatorViewModel()
           : base()
        {
        }

        const string heightMaleAvgVN = "/HealthDiagonosis/goal_weight_height_avg/vn/height_male";
        const string heightFemaleAvgVN = "/HealthDiagonosis/goal_weight_height_avg/vn/height_female";
        public override async Task<Task> ViewAppearingVM()
        {
            var tasks = new List<Task>();
            if (StaticHelper.MaleHeightAvgVN <= 0)
            {
                var t = this.dataService.GetFireBaseValue(heightMaleAvgVN).ContinueWith(t =>
                {
                    StaticHelper.MaleHeightAvgVN = float.Parse(t.Result, CultureInfo.InvariantCulture);
                });
                tasks.Add(t);
            }

            if (StaticHelper.FemaleHeightAvgVN <= 0)
            {
                var t = this.dataService.GetFireBaseValue(heightFemaleAvgVN).ContinueWith(t =>
                {
                    StaticHelper.FemaleHeightAvgVN = float.Parse(t.Result, CultureInfo.InvariantCulture);
                });
                tasks.Add(t);
            }

            if (StaticHelper.MaleHeightAvgUS <= 0)
            {
                var t = this.dataService.GetFireBaseValue(heightMaleAvgVN.Replace("vn", "us")).ContinueWith(t =>
                {
                    StaticHelper.MaleHeightAvgUS = float.Parse(t.Result, CultureInfo.InvariantCulture);
                });
                tasks.Add(t);
            }

            if (StaticHelper.FemaleHeightAvgUS <= 0)
            {
                var t = this.dataService.GetFireBaseValue(heightFemaleAvgVN.Replace("vn", "us")).ContinueWith(t =>
                {
                    StaticHelper.FemaleHeightAvgUS = float.Parse(t.Result, CultureInfo.InvariantCulture);
                });
                tasks.Add(t);
            }

            var getLocalUserTask = ServicesHelper.GetService<UserInfoDataStoreServie>().GetItemAsync();
            tasks.Add(getLocalUserTask);
            await Task.WhenAll(tasks);
            this.LocalUserInfo = getLocalUserTask.Result;
            if (LocalUserInfo != null && LocalUserInfo.BMIResult > 0)
            {
                this.GoalWeight = Math.Round(Math.Pow(this.LocalUserInfo.Height / 100f, 2) * BMICalculateHelper.NormalAVG, 1);
                this.DifferentGoalWeight = Math.Abs(GoalWeight - this.LocalUserInfo.Weight);
                var heightAvgVN = this.LocalUserInfo.IsMale ? StaticHelper.MaleHeightAvgVN : StaticHelper.FemaleHeightAvgVN;
                var heightAvgUS = this.LocalUserInfo.IsMale ? StaticHelper.MaleHeightAvgUS : StaticHelper.FemaleHeightAvgUS;
                this.Series = new ISeries[]
                {
                    new ColumnSeries<double>
                            {
                                Values = new ObservableCollection<double> { this.LocalUserInfo.Height},
                                IsVisible = true
                            },
                            new ColumnSeries<double>
                            {
                                Values = new ObservableCollection<double> { heightAvgUS},
                                IsVisible = true
                            },
                            new ColumnSeries<double>
                            {
                                Values = new ObservableCollection<double> { heightAvgVN},
                                IsVisible = true
                            },
                };
            }

            return base.ViewAppearingVM();
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            var data = query[nameof(BMIResultModel)] as BMIResultModel;
        }
    }
}
