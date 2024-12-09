// <copyright file="MealLogsPageVM.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AsyncAwaitBestPractices;
using Mopups.Services;
using PropertyChanged;
using VeganLife.Data;
using VeganLife.Data.LocalData;
using VeganLife.Helpers;
using VeganLife.Helpers.Extensions;
using VeganLife.Resources.Translations;
using VeganLife.Services.UserServices;
using VeganLife.Views.Popups;
using static VeganLife.Helpers.AppSetting.ConstantHelper.CalculateHelper;

namespace VeganLife.ViewModels
{
    public partial class MealLogsPageVM : BaseViewModel
    {
        private readonly IUserDataService _userDataService;
        private readonly BaseDataStore<NutritionMealLogModel> _nutritionMealLogDataStoreService;

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

        [ObservableProperty]
        private List<NutritionMealLogModel> _nutritionMealLogs;

        public MealLogsPageVM(LocalDataStoreFactory localDataStoreFactory)
            : base()
        {
            LocalUser = new UserInfo();
            _userDataService = FFImageLoading.Helpers.ServiceHelper.GetService<IUserDataService>();
            this._nutritionMealLogDataStoreService  = localDataStoreFactory.GetDataStore<NutritionMealLogModel>();
        }

        public override async Task ViewAppearingVM()
        {
            this.busyManager.Increase();

            _nutritionMealLogDataStoreService.GetItemsAsync()
                .ContinueWith(t =>
                {
                    NutritionMealLogs = t.Result.ToList();
                    foreach (var i in t.Result)
                    {
                        Console.WriteLine("==> " + i.Name);
                    }
                }).SafeFireAndForget();

            if (isInitialized)
            {
                this.busyManager.Decrease();
                return;
            }

            await this._userDataService.Refresh();
            LocalUser = _userDataService.GetUserInfo();
            SelectedActivityLevelIndex = (int)LocalUser.NormalFormatActivityLv;
            await this.UpdateActivityLevelAsync(SelectedActivityLevelIndex);
            HealthDiagnosisResult = BMICalculateHelper.GetWeightStatusCategory(LocalUser.Age, LocalUser.IsMale, LocalUser.BMIResult);
            GoalWeight = Math.Round(Math.Pow(LocalUser.Height / 100f, 2) * BMICalculateHelper.NormalAVG, 1);
            await base.ViewAppearingVM();
            this.busyManager.Decrease();

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

        [RelayCommand]
        private async Task CalendarClicked()
        {
            busyManager.Increase();
            await this.popupNaviService.PushAsync<MealLogsCalendarMopup>();
            busyManager.Decrease();
        }

        [SuppressPropertyChangedWarnings]
        partial void OnSelectedActivityLevelIndexChanged(int value)
        {
            if (value == (int)LocalUser.NormalFormatActivityLv)
            {
                return;
            }

            MainThread.BeginInvokeOnMainThread(async void () =>
            {
                try
                {
                    await UpdateActivityLevelAsync(value);
                }
                catch (Exception e)
                {
                    e.LogError();
                }
            });
        }

        private async Task UpdateActivityLevelAsync(int levelIndex = 0 )
        {
            var newActivityLevel = (ActivityLevel)levelIndex;
            var newTDEE = TDEEHelper.CalculateTDEE(LocalUser.BMRResult, newActivityLevel);
            LocalUser.TDEEResult = newTDEE;
            LocalUser.ActivityLevelData = newActivityLevel.ToString();
            await _userDataService.SaveData(LocalUser);
        }
    }
}
