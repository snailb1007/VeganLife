using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using VeganLife.Data.LocalData;
using VeganLife.Helpers;
using VeganLife.Helpers.AppSetting;

namespace VeganLife.ViewModels.ToolsFlyoutViewModel
{
    public partial class BmiCalculatorViewModel : BaseViewModel, IQueryAttributable
    {
        private const string HeightMaleAvgVN = "/HealthDiagonosis/goal_weight_height_avg/vn/height_male";
        private const string HeightFemaleAvgVN = "/HealthDiagonosis/goal_weight_height_avg/vn/height_female";

        [ObservableProperty]
        private UserInfo localUserInfo;
        [ObservableProperty]
        private double goalWeight;
        [ObservableProperty]
        private double differentGoalWeight;
        [ObservableProperty]
        private HealthDiagnosisModel healthDiagnosis;

        [ObservableProperty]
        private ISeries[] series;

        public BmiCalculatorViewModel()
           : base()
        {
        }

        private float _heightAvgVN;
        private float _heightAvgUS;

        public override async Task<Task> ViewAppearingVM()
        {
            var tasks = new List<Task>
            {
                this.UpdateHeightAveragesAsync(),
            };
            var getLocalUserTask = ServicesHelper.GetService<UserInfoDataStoreServie>().GetItemAsync();
            tasks.Add(getLocalUserTask);
            await Task.WhenAll(tasks);
            MainThread.BeginInvokeOnMainThread(() => this.LocalUserInfo = getLocalUserTask.Result);
            if (LocalUserInfo != null && LocalUserInfo.BMIResult > 0)
            {
                this.GoalWeight = Math.Round(Math.Pow(this.LocalUserInfo.Height / 100f, 2) * BMICalculateHelper.NormalAVG, 1);
                this.DifferentGoalWeight = Math.Abs(GoalWeight - this.LocalUserInfo.Weight);
                _heightAvgVN = this.LocalUserInfo.IsMale ? StaticHelper.MaleHeightAvgVN : StaticHelper.FemaleHeightAvgVN;
                _heightAvgUS = this.LocalUserInfo.IsMale ? StaticHelper.MaleHeightAvgUS : StaticHelper.FemaleHeightAvgUS;
                if (Series == null)
                {
                    this.Series =
                    [
                    new ColumnSeries<double>
                            {
                                Name = $"{this.LocalUserInfo.Name} {this.LocalUserInfo.Height}cm",
                                Values = new ObservableCollection<double> { this.LocalUserInfo.Height },
                                IsVisible = true,
                            },
                    new ColumnSeries<double>
                            {
                                Name = $"Trung binh o VN: {_heightAvgVN}cm",
                                Values = new ObservableCollection<double> { _heightAvgVN },
                                IsVisible = true,
                            },
                    new ColumnSeries<double>
                            {
                                Name = $"Trung binh o US: {_heightAvgUS}cm",
                                Values = new ObservableCollection<double> { _heightAvgUS },
                                IsVisible = true,
                            },
                    ];
                }

                // TODO: bug not gen ui
                MainThread.BeginInvokeOnMainThread(() => this.HealthDiagnosis = BMICalculateHelper.GetWeightStatusCategory(LocalUserInfo.Age, LocalUserInfo.IsMale, LocalUserInfo.BMIResult));
            }

            return base.ViewAppearingVM();
        }

        [RelayCommand]
        private void HiddenOrShowClicked(string param)
        {
            if (this.Series == null)
            {
                return;
            }

            if (param.Equals("vn"))
            {
                this.Series[1].IsVisible = !this.Series[1].IsVisible;
                this.Series[1].Name = this.Series[1].IsVisible ? $"Trung binh o VN: {_heightAvgVN}cm" : "Is Hidden";
            }
            else
            {
                this.Series[2].IsVisible = !this.Series[2].IsVisible;
                this.Series[2].Name = this.Series[2].IsVisible ? $"Trung binh o US: {_heightAvgUS}cm" : "Is Hidden";
            }
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            var data = query[nameof(BMIResultModel)] as BMIResultModel;
        }

        private async Task UpdateHeightAveragesAsync()
        {
            var tasks = new List<Task>();

            // Adding tasks conditionally based on static helper values
            if (StaticHelper.MaleHeightAvgVN <= 0)
            {
                tasks.Add(UpdateStaticValueAsync(HeightMaleAvgVN, value => StaticHelper.MaleHeightAvgVN = value));
            }

            if (StaticHelper.FemaleHeightAvgVN <= 0)
            {
                tasks.Add(UpdateStaticValueAsync(HeightFemaleAvgVN, value => StaticHelper.FemaleHeightAvgVN = value));
            }

            if (StaticHelper.MaleHeightAvgUS <= 0)
            {
                tasks.Add(UpdateStaticValueAsync(HeightMaleAvgVN.Replace("vn", "us"), value => StaticHelper.MaleHeightAvgUS = value));
            }

            if (StaticHelper.FemaleHeightAvgUS <= 0)
            {
                tasks.Add(UpdateStaticValueAsync(HeightFemaleAvgVN.Replace("vn", "us"), value => StaticHelper.FemaleHeightAvgUS = value));
            }

            await Task.WhenAll(tasks);

            async Task UpdateStaticValueAsync(string key, Action<float> updateAction)
            {
                try
                {
                    string result = await this.dataService.GetFireBaseValue(key);
                    float parsedResult = float.Parse(result, CultureInfo.InvariantCulture);
                    updateAction(parsedResult);
                }
                catch (Exception ex)
                {
                    // Handle errors such as parsing errors or network issues
                    Debug.WriteLine($"Error updating value for {key}: {ex.Message}");
                }
            }
        }
    }
}
