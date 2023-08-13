// <copyright file="ProfilePopupViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using VeganLife.Data.LocalData;
using VeganLife.Helpers;

namespace VeganLife.ViewModels.PopupViewModels
{
    /// <summary>
    /// vm for  ProfilePopup.
    /// </summary>
    public partial class ProfilePopupViewModel : BaseViewModel
    {
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
        private short userHeight;

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
        }

        bool hasOldUserData;
        public async void ViewAppearing()
        {
            UserInfo = await ServicesHelper.GetService<UserInfoDataStoreServie>().GetFirstOrDefaultItem();
            hasOldUserData = UserInfo != null;
            if (hasOldUserData)
            {
                UserName = UserInfo.Name;
                SelectedDate = UserInfo.DateOfBirth;
                UserWeight = UserInfo.Weight.ToString();
            }

            UserInfo ??= new UserInfo();
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
                ErrorMess = Resources.Translations.AppResources.emptyName_profilePopupEdit;
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
                UserInfo.DateOfBirth = SelectedDate;
                if (float.TryParse(this.UserWeight, provider: CultureInfo.InvariantCulture.NumberFormat, out var outValue))
                {
                    UserInfo.Weight = outValue;
                }

                await ServicesHelper.GetService<UserInfoDataStoreServie>().AddOrUpdateItemAsync(UserInfo, hasOldUserData);
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

        partial void OnUserHeightChanged(short value)
        {
            UserHeight = (short)UserHeight;
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
