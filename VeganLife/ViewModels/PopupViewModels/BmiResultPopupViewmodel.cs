// <copyright file="BmiResultPopupViewmodel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using CommunityToolkit.Mvvm.Messaging;
using Mopups.Services;
using PropertyChanged;
using VeganLife.Data;
using VeganLife.Data.LocalData;
using VeganLife.Helpers;
using VeganLife.Messages;

namespace VeganLife.ViewModels.PopupViewModels
{
    /// <summary>
    /// vm for  BmiResultPopup.
    /// </summary>
    public partial class BmiResultPopupViewmodel : BaseViewModel
    {
        private readonly BaseDataStore<UserInfo> _userStoreService;
        private UserInfo _localeUserInfo;
        private float _bmiResult;
        private BMIResultModel _result;

        public string Message { get; set; }

        [ObservableProperty]
        private string _bmiResultText;
        [ObservableProperty]
        private Color _bmiStatusColor;
        [ObservableProperty]
        private string _classifyLabel;
        [ObservableProperty]
        private string _note;
        [ObservableProperty]
        private bool _isReCalculateSelected;
        [ObservableProperty]
        private bool _isGoAnalysisPageSelected = true;
        [ObservableProperty]
        private bool _isSaveSelected;
        [ObservableProperty]
        private bool _isAllowSaveBmiResult;
        [ObservableProperty]
        private bool _isLocaleUser;

        /// <summary>
        /// Initializes a new instance of the <see cref="BmiResultPopupViewmodel"/> class.
        /// </summary>
        public BmiResultPopupViewmodel()
            : base()
        {
            _userStoreService = ServicesHelper.GetService<LocalDataStoreFactory>().GetDataStore<UserInfo>();
        }

        /// <inheritdoc/>
        public override async Task<Task> OnNavigatingTo(object parameter)
        {
            if (parameter == null)
            {
                return base.OnNavigatingTo(null);
            }

            if (parameter is not BMIResultModel model)
            {
                return base.OnNavigatingTo(parameter);
            }

            _result = model;
            _bmiResult = _result.BMIResult;
            this.BmiResultText = _result.BMIResult.ToString(CultureInfo.InvariantCulture);
            var healthDiagnosis = BMICalculateHelper
                .GetWeightStatusCategory(_result.Age, _result.IsMale, _result.BMIResult);
            this.BmiStatusColor = healthDiagnosis.StatusColor;
            this.ClassifyLabel = healthDiagnosis.Classify;
            this.Note = healthDiagnosis.Note;
            _localeUserInfo = (await _userStoreService.GetItemsAsync())?.FirstOrDefault()!;
            if (!string.IsNullOrEmpty(_localeUserInfo.Name)
                && _localeUserInfo is { Age: > 0, Weight: > 0, Height: > 0 })
            {
                this.IsLocaleUser = true;
            }

            return base.OnNavigatingTo(model);
        }

        [RelayCommand]
        private async Task Go()
        {
            if (!IsSaveSelected || this.GoCommand.IsRunning)
            {
                return;
            }

            await Task.WhenAll(
                MopupService.Instance.PopAsync(),
                _userStoreService.AddOrUpdateItemAsync(_localeUserInfo, IsLocaleUser));
            WeakReferenceMessenger.Default.Send(new BmiResultSelectedOptionMessage(1));
        }

        [RelayCommand]
        private async Task CloseAsync()
        {
            await MopupService.Instance.PopAsync();
        }

        [SuppressPropertyChangedWarnings]
        partial void OnIsSaveSelectedChanged(bool value)
        {
            if (!value)
            {
                return;
            }

            this.IsLocaleUser = _localeUserInfo.Age == _result.Age
                                && _localeUserInfo.IsMale == _result.IsMale;
            if (Math.Abs(_localeUserInfo.BMIResult - this._bmiResult) > 0.01)
            {
                _localeUserInfo.BMIResult = this._bmiResult;
            }
        }
    }
}
