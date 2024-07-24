
using VeganLife.Helpers;
using VeganLife.Services.UserServices;

namespace VeganLife.ViewModels
{
    public partial class MealLogsPageVM : BaseViewModel
    {
        private readonly IUserDataService _userDataService;

        public List<string> ActivityLevels => new List<string>
        {
            "Sedentary",
            "Lightly Active",
            "Moderately Active",
            "Very Active",
            "Extra Active",
        };

        [ObservableProperty]
        private UserInfo localUser;

        [ObservableProperty]
        private HealthDiagnosisModel healthDiagnosisResult;

        [ObservableProperty]
        private int selectedActivityLevelIndex = 0;

        public MealLogsPageVM()
            : base()
        {
            localUser = new UserInfo();
            _userDataService = ServicesHelper.GetService<IUserDataService>();
        }

        public async override Task ViewAppearingVM()
        {
            if (isInitialized)
            {
                return;
            }

            using (await this.loadingService.Show())
            {
                await this._userDataService.Refresh();
                LocalUser = _userDataService.GetUserInfo();

                var bmr = TDEEHelper.CalculateBMR(
                    weight: LocalUser.Weight,
                    height: LocalUser.Height,
                    age: LocalUser.Age,
                    isMale: LocalUser.IsMale);
                if (LocalUser.BMRResult != bmr)
                {
                    LocalUser.BMRResult = bmr;
                    _ = _userDataService.SaveData(LocalUser);
                }

                HealthDiagnosisResult = BMICalculateHelper.GetWeightStatusCategory(LocalUser.Age, LocalUser.IsMale, LocalUser.BMIResult);
                await base.ViewAppearingVM();
            }

            isInitialized = true;
        }
    }
}
