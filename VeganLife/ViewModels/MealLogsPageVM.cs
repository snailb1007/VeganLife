// <copyright file="MealLogsPageVM.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AsyncAwaitBestPractices;
using CommunityToolkit.Maui.Views;
using Mopups.Services;
using PropertyChanged;
using VeganLife.Data;
using VeganLife.Data.LocalData;
using VeganLife.Helpers;
using VeganLife.Helpers.Extensions;
using VeganLife.Resources.Translations;
using VeganLife.Services.UserServices;
using VeganLife.Views.MainPageFlyout;
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

        [ObservableProperty]
        private NutritionMealLogModel _selectedMealLog;

        public MealLogsPageVM(LocalDataStoreFactory localDataStoreFactory)
            : base()
        {
            LocalUser = new UserInfo();
            _userDataService = FFImageLoading.Helpers.ServiceHelper.GetService<IUserDataService>();
            this._nutritionMealLogDataStoreService = localDataStoreFactory.GetDataStore<NutritionMealLogModel>();
        }

        public override async Task ViewAppearingVM()
        {
            this.busyManager.Increase();

            _nutritionMealLogDataStoreService.GetItemsAsync()
                .ContinueWith(t =>
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        NutritionMealLogs = t.Result.Where(i => i.EatingDay.Date == DateTime.Now.Date).ToList();
                    });

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

        [ObservableProperty]
        private ObservableCollection<string> _selectedDates = [];

        [RelayCommand]
        private async Task CalendarClicked()
        {
            busyManager.Increase();
            var result = await AppHelpers.CurrentMainPage.DisplayPromptAsync("Calendar",
            "Select up to 7 dates (comma-separated):",
            "OK",
            "Cancel");
            if (!string.IsNullOrWhiteSpace(result))
            {
                var dates = result.Split(',')
                                  .Select(date => date.Trim())
                                  .Take(7)
                                  .ToList();

                SelectedDates.Clear();
                foreach (var date in dates)
                {
                    SelectedDates.Add(date);
                }
            }       
            busyManager.Decrease();
        }

        [RelayCommand]
        private async Task OpenMealLogsAnalysisPageAsync()
        {
            busyManager.Increase();
            await this.navigationService.NavigateToPage<MealLogsAnalysisPage>(paramater: NutritionMealLogs);
            busyManager.Decrease();
        }

        [RelayCommand]
        private async Task ShowMealOptions(NutritionMealLogModel mealLog)
        {
            if (mealLog == null)
                return;
            busyManager.Increase();
            SelectedMealLog = mealLog;

            // Create and show the popup
            var popup = new EditMealPopup(mealLog);

            var result = await AppHelpers.CurrentMainPage.ShowPopupAsync(popup);

            if (result is EditMealPopup.EditMealResult editResult)
            {
                if (editResult.IsDeleted)
                {
                    // Delete the meal log from your database
                    await DeleteMealLogAsync(mealLog);
                }
                else if (editResult.IsAmountChanged)
                {
                    // Update the amount in your database
                    mealLog.Amount = editResult.NewAmount;
                    await UpdateMealLogAsync(mealLog);
                }
            }

            busyManager.Decrease();
        }

        private async Task DeleteMealLogAsync(NutritionMealLogModel mealLog)
        {
            try
            {
                busyManager.Increase();

                // Delete from database
                await _nutritionMealLogDataStoreService.DeleteItem(mealLog);

                // Update collection
                if (NutritionMealLogs.Contains(mealLog))
                {
                    var updatedList = new List<NutritionMealLogModel>(NutritionMealLogs);
                    updatedList.Remove(mealLog);
                    NutritionMealLogs = updatedList;
                }
            }
            catch (Exception ex)
            {
                ex.LogError(); // Assuming your error logging extension
                await AppHelpers.CurrentMainPage.DisplayAlert("Error", "Failed to delete meal log", "OK");
            }
            finally
            {
                busyManager.Decrease();
            }
        }

        private async Task UpdateMealLogAsync(NutritionMealLogModel mealLog)
        {
            try
            {
                busyManager.Increase();

                // Update in database
                await _nutritionMealLogDataStoreService.AddOrUpdateItemAsync(mealLog, isUpdate: true);

                // Refresh the UI
                var index = NutritionMealLogs.FindIndex(m => m.Id == mealLog.Id);
                if (index >= 0)
                {
                    var updatedList = new List<NutritionMealLogModel>(NutritionMealLogs);
                    updatedList[index] = mealLog;
                    NutritionMealLogs = updatedList;
                }
            }
            catch (Exception ex)
            {
                ex.LogError();
                await AppHelpers.CurrentMainPage.DisplayAlert("Error", "Failed to update meal log", "OK");
            }
            finally
            {
                busyManager.Decrease();
            }
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

        private async Task UpdateActivityLevelAsync(int levelIndex = 0)
        {
            var newActivityLevel = (ActivityLevel)levelIndex;
            var newTDEE = TDEEHelper.CalculateTDEE(LocalUser.BMRResult, newActivityLevel);
            LocalUser.TDEEResult = newTDEE;
            LocalUser.ActivityLevelData = newActivityLevel.ToString();
            await _userDataService.SaveData(LocalUser);
        }
    }
}
