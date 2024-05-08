// <copyright file="BmiResultPopupViewmodel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using VeganLife.Data.LocalData;

namespace VeganLife.ViewModels.PopupViewModels
{
    using CommunityToolkit.Mvvm.Messaging;
    using Mopups.Services;
    using VeganLife.Helpers;
    using VeganLife.messages;

    /// <summary>
    /// vm for  BmiResultPopup.
    /// </summary>
    public partial class BmiResultPopupViewmodel : BaseViewModel
    {
        [ObservableProperty]
        private string bmiResultText;

        [ObservableProperty]
        private Color bmiStatusColor;

        public string Message { get; set; }

        [ObservableProperty]
        private string classifyLabel;
        [ObservableProperty]
        private string? note;
        [ObservableProperty]
        private bool isReCalculateSelected;
        [ObservableProperty]
        private bool isGoAnalysisPageSelected = true;
        [ObservableProperty]
        private bool isSaveSelected;
        [ObservableProperty]
        private bool isAllowSaveBmiResult;

        private UserInfo localeUserInfo;
        private readonly UserInfoDataStoreServie userStoreService;

        /// <summary>
        /// Initializes a new instance of the <see cref="BmiResultPopupViewmodel"/> class.
        /// </summary>
        public BmiResultPopupViewmodel()
            : base()
        {
            userStoreService = ServicesHelper.GetService<UserInfoDataStoreServie>();
        }

        private float bmiResult;
        [ObservableProperty]
        private bool isLocaleUser;
        private BMIResultModel result;
        /// <inheritdoc/>
        public override async Task<Task> OnNavigatingTo(object parameter)
        {
            if (parameter != null)
            {
                if (parameter is not BMIResultModel) return base.OnNavigatingTo(parameter);
                result = (BMIResultModel)parameter;
                bmiResult = result.BMIResult;
                this.BmiResultText = result.BMIResult.ToString();
                var healthDiagnosis = BMICalculateHelper
                    .GetWeightStatusCategory(result.Age, result.IsMale, result.BMIResult);
                this.BmiStatusColor = healthDiagnosis.StatusColor;
                this.ClassifyLabel = healthDiagnosis.Classify;
                this.Note = healthDiagnosis.Note;
                localeUserInfo = (await userStoreService.GetItemsAsync())?.FirstOrDefault()!;
                if (localeUserInfo is not null
                    && !string.IsNullOrEmpty(localeUserInfo.Name)
                    && localeUserInfo.Age > 0
                    && localeUserInfo.Weight > 0
                    && localeUserInfo.Height > 0)
                {
                    this.IsLocaleUser = true;
                }
            }

            return base.OnNavigatingTo(parameter!);
        }

        [RelayCommand]
        private async Task SelectButton(string option)
        {
            if (option.Equals("0"))
            {
                WeakReferenceMessenger.Default.Send(new BmiResultSelectedOptionMessage(0));
                await MopupService.Instance.PopAsync();
                IsReCalculateSelected = true;
                IsGoAnalysisPageSelected = false;
                IsSaveSelected = false;
            }
            else if (option.Equals("1"))
            {
                if (this.IsGoAnalysisPageSelected)
                {
                    this.GoCommand.Execute(null);
                    WeakReferenceMessenger.Default.Send(new BmiResultSelectedOptionMessage(1));
                    return;
                }

                IsReCalculateSelected = false;
                IsGoAnalysisPageSelected = true;
                IsSaveSelected = false;
            }

            //else
            //{
            //    IsReCalculateSelected = false;
            //    IsGoAnalysisPageSelected = false;
            //    IsSaveSelected = true;
            //}
        }

        [RelayCommand]
        private async Task Go()
        {
            if (this.GoCommand.IsRunning)
            {
                return;
            }

            await MopupService.Instance.PopAsync();
            if (this.IsSaveSelected)
            {
                await this.userStoreService.AddOrUpdateItemAsync(localeUserInfo, IsLocaleUser);
            }
        }

        partial void OnIsSaveSelectedChanged(bool value)
        {
            if (value)
            {
                this.IsLocaleUser = localeUserInfo.Age == result.Age
                    && localeUserInfo.IsMale == result.IsMale;
                if (localeUserInfo.BMIResult != this.bmiResult)
                {
                    localeUserInfo.BMIResult = this.bmiResult;
                }
            }
        }

        partial void OnIsAllowSaveBmiResultChanged(bool value)
        {

        }
    }
}
