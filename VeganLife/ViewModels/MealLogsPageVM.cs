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

        public List<string> ActivityLevels => new List<string>
        {
            AppResources.mealLogsPage_sedentary,
            AppResources.mealLogsPage_LightlyActive,
            AppResources.mealLogsPage_ModeratelyActive,
            AppResources.mealLogsPage_VeryActive,
            AppResources.mealLogsPage_SuperActive,
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

            await this._userDataService.Refresh();
            LocalUser = _userDataService.GetUserInfo();
            SelectedActivityLevelIndex = (int)LocalUser.NormalFormatActivityLv;
            HealthDiagnosisResult = BMICalculateHelper.GetWeightStatusCategory(LocalUser.Age, LocalUser.IsMale, LocalUser.BMIResult);
            await base.ViewAppearingVM();

            isInitialized = true;
        }

        [RelayCommand]
        private async Task OnInfoClickedAsync(string param)
        {
            string data = string.Empty;
            switch (param)
            {
                case "BMI":
                    data = AppResources.mealLogsPage_BMI_description;
                    break;
                case "BMR":
                    data = AppResources.mealLogsPage_BMR_description;
                    break;
                case "TDEE":
                    data = AppResources.mealLogsPage_TDEE_description;
                    break;
            }

            await MopupService.Instance.PushAsync(new SimpleInformationPopup(data));
        }

        [SuppressPropertyChangedWarnings]
        partial void OnSelectedActivityLevelIndexChanged(int value)
        {
            if (value == (int)LocalUser.NormalFormatActivityLv)
            {
                return;
            }

            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await UpdateActivityLevelAsync();
            });

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
