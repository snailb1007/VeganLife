// <copyright file="ProfilePopupViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.Messaging;
using PropertyChanged;
using VeganLife.Helpers;
using VeganLife.Messages;
using VeganLife.Services.UserServices;

namespace VeganLife.ViewModels.PopupViewModels
{
    /// <summary>
    /// vm for  ProfilePopup.
    /// </summary>
    public partial class ProfilePopupViewModel : BaseViewModel
    {
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

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
        [ObservableProperty]
        private bool isUserLocalDataUpdating;

        [ObservableProperty]
        private bool isMale;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfilePopupViewModel"/> class.
        /// </summary>
        public ProfilePopupViewModel()
        {
            UserInfo = new UserInfo();
        }

        public void ViewAppearing()
        {
            UserInfo = ServicesHelper.GetService<IUserDataService>().GetUserInfo();
            if (string.IsNullOrEmpty(UserInfo.Name))
            {
                return;
            }

            UserName = UserInfo.Name;
            SelectedDate = UserInfo.DateOfBirth.Value;
            UserWeight = UserInfo.Weight.ToString();
            UserHeight = UserInfo.Height;
            IsMale = UserInfo.IsMale;
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

            if (string.IsNullOrEmpty(ErrorMess) &&
                !string.IsNullOrEmpty(UserName) &&
                (UserInfo.Name != UserName || UserInfo.DateOfBirth != SelectedDate || UserInfo.Height != UserHeight || UserInfo.IsMale != this.IsMale))
            {
                IsUserLocalDataUpdating = true;
                UserInfo.Name = UserName;
                UserInfo.DateOfBirth = SelectedDate;
                UserInfo.Height = UserHeight;
                UserInfo.IsMale = IsMale;
                if (float.TryParse(this.UserWeight, provider: CultureInfo.InvariantCulture.NumberFormat, out var outValue))
                {
                    UserInfo.Weight = outValue;
                }

                await ServicesHelper.GetService<IUserDataService>().SaveData(this.UserInfo);
                IsUserLocalDataUpdating = false;
                var toast = Toast.Make(Resources.Translations.AppResources.infoAlert_userDataSaved_profilePopupEdit);
                await toast.Show(_cancellationTokenSource.Token);
                WeakReferenceMessenger.Default.Send(new ProfileChangedMessage(null));
            }
        }

        [SuppressPropertyChangedWarnings]
        partial void OnUserNameChanged(string value)
        {
            IsWrongFormatName = false;
        }

        [SuppressPropertyChangedWarnings]
        partial void OnSelectedDateChanged(DateTime value)
        {
            IsWrongDate = false;
        }

        [SuppressPropertyChangedWarnings]
        partial void OnUserWeightChanged(string value)
        {
            IsWrongFormatWeight = false;
        }

        [SuppressPropertyChangedWarnings]
        partial void OnIsWrongFormatNameChanged(bool value)
        {
            SetupErrorMess();
        }

        [SuppressPropertyChangedWarnings]
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
