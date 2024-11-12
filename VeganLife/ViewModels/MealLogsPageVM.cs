// <copyright file="MealLogsPageVM.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Mopups.Services;
using PropertyChanged;
using VeganLife.Helpers;
using VeganLife.Resources.Translations;
using VeganLife.Services.UserServices;
using VeganLife.Views.Popups;
using static VeganLife.Helpers.AppSetting.ConstantHelper.CalculateHelper;

namespace VeganLife.ViewModels
{
    public partial class MealLogsPageVM : BaseViewModel
    {
        private readonly IUserDataService _userDataService;

        public List<string> ActivityLevels =>
        [
            AppResources.mealLogsPage_sedentary,
            AppResources.mealLogsPage_LightlyActive,
            AppResources.mealLogsPage_ModeratelyActive,
            AppResources.mealLogsPage_VeryActive,
            AppResources.mealLogsPage_SuperActive
        ];

        [ObservableProperty]
        private UserInfo _localUser;

        [ObservableProperty]
        private HealthDiagnosisModel _healthDiagnosisResult;

        [ObservableProperty]
        private int _selectedActivityLevelIndex = 0;

        [ObservableProperty]
        private double _goalWeight = -1;

        public MealLogsPageVM()
            : base()
        {
            LocalUser = new UserInfo();
            _userDataService = ServicesHelper.GetService<IUserDataService>();
        }

        public override async Task ViewAppearingVM()
        {
            if (isInitialized)
            {
                return;
            }

            await this._userDataService.Refresh();
            LocalUser = _userDataService.GetUserInfo();
            SelectedActivityLevelIndex = (int)LocalUser.NormalFormatActivityLv;
            HealthDiagnosisResult = BMICalculateHelper.GetWeightStatusCategory(LocalUser.Age, LocalUser.IsMale, LocalUser.BMIResult);
            GoalWeight = Math.Round(Math.Pow(LocalUser.Height / 100f, 2) * BMICalculateHelper.NormalAVG, 1);
            await base.ViewAppearingVM();

            isInitialized = true;
        }

        [RelayCommand]
        private async Task OnInfoClickedAsync(string param)
        {
            string data = param switch
            {
                "BMI" => AppResources.mealLogsPage_BMI_description,
                "BMR" => AppResources.mealLogsPage_BMR_description,
                "TDEE" => AppResources.mealLogsPage_TDEE_description,
                _ => string.Empty,
            };

            await MopupService.Instance.PushAsync(new SimpleInformationPopup(data));
        }

        [SuppressPropertyChangedWarnings]
        partial void OnSelectedActivityLevelIndexChanged(int value)
        {
            if (value == (int)LocalUser.NormalFormatActivityLv)
            {
                return;
            }

            async void Action()
            {
                await UpdateActivityLevelAsync();
            }

            MainThread.BeginInvokeOnMainThread(Action);

            async Task UpdateActivityLevelAsync()
            {
                var newActivityLevel = (ActivityLevel)value;
                var newTDEE = TDEEHelper.CalculateTDEE(LocalUser.BMRResult, newActivityLevel);
                LocalUser.TDEEResult = newTDEE;
                LocalUser.ActivityLevelData = newActivityLevel.ToString();
                await _userDataService.SaveData(LocalUser);
            }
        }
    }
}
