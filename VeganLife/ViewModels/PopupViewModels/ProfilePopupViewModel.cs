// <copyright file="ProfilePopupViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.ViewModels.PopupViewModels
{
    /// <summary>
    /// vm for  ProfilePopup.
    /// </summary>
    public partial class ProfilePopupViewModel : BaseViewModel
    {
        const float averageDaysInYear = 365.25f;
        [ObservableProperty]
        private UserInfo userInfo;

        [ObservableProperty]
        private string userName;
        [ObservableProperty]
        private bool isWrongFormatName;

        [ObservableProperty]
        private DateTime selectedDate;
        [ObservableProperty]
        private bool isWrongDate;

        [ObservableProperty]
        private string userHeight;
        [ObservableProperty]
        private bool isWrongFormatHeight;

        [ObservableProperty]
        private string userWeight;
        [ObservableProperty]
        private bool isWrongFormatWeight;

        [ObservableProperty]
        private string errorMess;
        /// <summary>
        /// Initializes a new instance of the <see cref="ProfilePopupViewModel"/> class.
        /// </summary>
        public ProfilePopupViewModel()
        {
            UserInfo = new UserInfo();
        }

        [RelayCommand]
        private async Task CloseThisPopup()
        {
            if (CloseThisPopupCommand.IsRunning)
            {
                return;
            }

            await popupNaviService.PopAsync();
        }

        [RelayCommand]
        private async Task SaveData()
        {
            if (string.IsNullOrEmpty(UserName))
            {
                IsWrongFormatName = true;
            }

            if (string.IsNullOrEmpty(UserWeight))
            {
                IsWrongFormatWeight = true;
                ErrorMess = Resources.Translations.AppResources.emptyWeight_profilePopupEdit;
            }
            else
            {
                if (float.TryParse(this.UserWeight, provider: CultureInfo.InvariantCulture.NumberFormat, out var outValue))
                {
                    UserInfo.Weight = outValue;
                }
                else
                {
                    IsWrongFormatWeight = true;
                    ErrorMess = Resources.Translations.AppResources.wrongWeight_profilePopupEdit;
                }
            }

            if (string.IsNullOrEmpty(ErrorMess))
            {
                UserInfo.Name = UserName;
                var totalDaysDifference = DateTime.Today.Subtract(SelectedDate).TotalDays;
                UserInfo.Age = (byte)(totalDaysDifference / averageDaysInYear);
                if (float.TryParse(this.UserWeight, provider: CultureInfo.InvariantCulture.NumberFormat, out var outValue))
                {
                    UserInfo.Weight = outValue;
                }
            }
        }

        partial void OnUserNameChanged(string value)
        {
            IsWrongFormatName = false;
        }

        partial void OnSelectedDateChanged(DateTime value)
        {
            IsWrongDate = false;
        }

        partial void OnUserWeightChanged(string value)
        {
            IsWrongFormatWeight = false;
        }

        partial void OnIsWrongFormatNameChanged(bool value)
        {
            SetupErrorMess();
        }

        partial void OnIsWrongFormatWeightChanged(bool value)
        {
            SetupErrorMess();
        }

        private void SetupErrorMess()
        {
            bool isHasAnyError = IsWrongFormatName || IsWrongDate || IsWrongFormatWeight;
            if (!isHasAnyError)
            {
                ErrorMess = string.Empty;
            }
        }
    }
}
