using VeganLife.Helpers.AppSetting;

namespace VeganLife.ViewModels.ToolsFlyoutViewModel
{
    public partial class BmiCalculatorViewModel : BaseViewModel, IQueryAttributable
    {
        public BmiCalculatorViewModel()
           : base()
        {
        }

        const string heightMaleAvg = "/HealthDiagonosis/goal_weight_height_avg/vn/height_female";
        const string heightFemaleAvg = "/HealthDiagonosis/goal_weight_height_avg/vn/height_male";
        public override async Task<Task> ViewAppearingVM()
        {
            if (string.IsNullOrEmpty(StaticHelper.MaleHeightAvgVN) || string.IsNullOrEmpty(StaticHelper.FemaleHeightAvgVN))
            {
                var t1 = this.dataService.GetFireBaseValue(heightMaleAvg);
                var t2 = this.dataService.GetFireBaseValue(heightFemaleAvg);
                await Task.WhenAll(t1, t2).ContinueWith(t =>
                {
                    StaticHelper.MaleHeightAvgVN = t2.Result;
                    StaticHelper.FemaleHeightAvgVN = t1.Result;
                    Console.WriteLine($"==> MaleHeightAvgVN {StaticHelper.MaleHeightAvgVN} {StaticHelper.FemaleHeightAvgVN}");
                });
            }

            return base.ViewAppearingVM();
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            var data = query[nameof(BMIResultModel)] as BMIResultModel;
        }
    }
}
