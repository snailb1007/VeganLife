// <copyright file="BmiResultPopupViewmodel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using CommunityToolkit.Mvvm.Messaging;
using Mopups.Services;
using VeganLife.Data.LocalData;
using VeganLife.Helpers;
using VeganLife.messages;

namespace VeganLife.ViewModels.PopupViewModels
{
    /// <summary>
    /// vm for  BmiResultPopup.
    /// </summary>
    public partial class BmiResultPopupViewmodel : BaseViewModel
    {
        private readonly UserInfoDataStoreServie _userStoreService;
        private UserInfo _localeUserInfo;
        private float _bmiResult;
        private BMIResultModel _result;

        public string Message { get; set; }

        [ObservableProperty]
        private string bmiResultText;
        [ObservableProperty]
        private Color bmiStatusColor;
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
        [ObservableProperty]
        private bool isLocaleUser;

        /// <summary>
        /// Initializes a new instance of the <see cref="BmiResultPopupViewmodel"/> class.
        /// </summary>
        public BmiResultPopupViewmodel()
            : base()
        {
            _userStoreService = ServicesHelper.GetService<UserInfoDataStoreServie>();
        }

        /// <inheritdoc/>
        public override async Task<Task> OnNavigatingTo(object? parameter)
        {
            if (parameter != null)
            {
                if (parameter is not BMIResultModel)
                {
                    return base.OnNavigatingTo(parameter);
                }

                _result = (BMIResultModel)parameter;
                _bmiResult = _result.BMIResult;
                this.BmiResultText = _result.BMIResult.ToString();
                var healthDiagnosis = BMICalculateHelper
                    .GetWeightStatusCategory(_result.Age, _result.IsMale, _result.BMIResult);
                this.BmiStatusColor = healthDiagnosis.StatusColor;
                this.ClassifyLabel = healthDiagnosis.Classify;
                this.Note = healthDiagnosis.Note;
                _localeUserInfo = (await _userStoreService.GetItemsAsync())?.FirstOrDefault()!;
                if (_localeUserInfo is not null
                    && !string.IsNullOrEmpty(_localeUserInfo.Name)
                    && _localeUserInfo.Age > 0
                    && _localeUserInfo.Weight > 0
                    && _localeUserInfo.Height > 0)
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
                await this._userStoreService.AddOrUpdateItemAsync(_localeUserInfo, IsLocaleUser);
            }
        }

        partial void OnIsSaveSelectedChanged(bool value)
        {
            if (value)
            {
                this.IsLocaleUser = _localeUserInfo.Age == _result.Age
                    && _localeUserInfo.IsMale == _result.IsMale;
                if (_localeUserInfo.BMIResult != this._bmiResult)
                {
                    _localeUserInfo.BMIResult = this._bmiResult;
                }
            }
        }

        partial void OnIsAllowSaveBmiResultChanged(bool value)
        {

        }
    }
}
