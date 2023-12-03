// <copyright file="BMICalculatorViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.ViewModels
{
    using CommunityToolkit.Mvvm.Messaging;
    using VeganLife.Helpers;
    using VeganLife.messages;
    using VeganLife.Views.Popups;
    using VeganLife.Views.ToolFlyout;

    public partial class MainToolViewModel : BaseViewModel, IRecipient<BmiResultSelectedOptionMessage>
    {
        public string WeightBmiRegexPattern { get; } = @"^(?:[1-9]\d*|0)+(?:\.(\d)?(\d)?)?$";

        public string AgeBmiRegexPattern { get; } = @"^\d+$";

        private float weight;
        // private short age;

        [ObservableProperty]
        private int height;

        [ObservableProperty]
        private bool isDisplayedSexDetail;

        [ObservableProperty]
        private bool isMale;

        [ObservableProperty]
        private bool isEnableSubmit;

        [ObservableProperty]
        private string weightValue;

        [ObservableProperty]
        private byte ageValue;

        [ObservableProperty]
        private string backgroundImg;

        [ObservableProperty]
        private string weightErrMess;

        [ObservableProperty]
        private string ageErrMess;

        [ObservableProperty]
        private string generalError;

        [ObservableProperty]
        private double bmiResult;

        [RelayCommand]
        private void HelpSexDetail(string parameter)
        {
            if (!string.IsNullOrEmpty(parameter) && parameter.Equals("closeSexDetail"))
            {
                this.IsDisplayedSexDetail = false;
            }
            else
            {
                this.IsDisplayedSexDetail = !this.IsDisplayedSexDetail;
            }
        }

        [RelayCommand]
        private void UnFocus(object obj)
        {
            if (obj == null)
            {
                return;
            }

            var view = (MainTool)obj;
            if (view == null)
            {
                return;
            }

            var entryWeight = view.FindByName("entryWeight") as Entry;

            if (entryWeight != null && entryWeight.IsFocused)
            {
                this.deviceService.HideKeyboard();
                entryWeight.Unfocus();
                return;
            }

            var entryAge = view.FindByName("entryAge") as Entry;
            if (entryAge != null && entryAge.IsFocused)
            {
                this.deviceService.HideKeyboard();
                entryAge.Unfocus();
            }
        }

        [RelayCommand]
        private void SelectGender(string parameter)
        {
            this.IsMale = !this.IsMale;
        }

        public MainToolViewModel()
            : base()
        {
            this.Init();
        }

        public override Task ViewAppearingVM()
        {
            WeakReferenceMessenger.Default.Register<BmiResultSelectedOptionMessage>(this);
            return base.ViewAppearingVM();
        }

        public override Task ViewDisappearingVM()
        {
            WeakReferenceMessenger.Default.Unregister<BmiResultSelectedOptionMessage>(this);
            return base.ViewDisappearingVM();
        }

        private void Init()
        {
        }

        private BMIResultModel bmiResultData;
        [RelayCommand]
        private async Task CalculateBmi()
        {
            this.BmiResult = BMICalculateHelper.Calculate(this.weight, this.Height / 100f);
            bmiResultData = new BMIResultModel()
            {
                BMIResult = (float)this.BmiResult,
                IsMale = this.IsMale,
                Age = this.AgeValue,
            };
            await ServicesHelper.GetService<IPopupNaviService>().PushAsync<BmiResultPopup>(bmiResultData);
        }

        partial void OnWeightValueChanged(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                this.WeightErrMess = string.Empty;
                this.IsEnableSubmit = false;
                return;
            }

            this.weight = float.TryParse(value, provider: CultureInfo.InvariantCulture.NumberFormat, out var outValue) ? outValue : 0;
            if (weight < 2)
            {
                this.WeightErrMess = Resources.Translations.AppResources.wrongWeight_tooLow_bmiCalculatePage;
            }
            else if (weight > 635)
            {
                this.WeightErrMess = Resources.Translations.AppResources.wrongWeight_tooHigh_bmiCalculatePage;
            }
            else
            {
                this.WeightErrMess = string.Empty;
            }

            this.IsEnableSubmit = CheckEnableButtonCalculate();
        }

        partial void OnAgeValueChanged(byte value)
        {
            if (value <= 0)
            {
                this.AgeErrMess = string.Empty;
                this.IsEnableSubmit = false;
                return;
            }

            if (value <= 1)
            {
                this.AgeErrMess = Resources.Translations.AppResources.wrongAge_tooLow_bmiCalculatePage;
            }
            else if (value >= 140)
            {
                this.AgeErrMess = Resources.Translations.AppResources.wrongAge_tooHigh_bmiCalculatePage;
            }
            else
            {
                this.AgeErrMess = string.Empty;
            }

            this.IsEnableSubmit = CheckEnableButtonCalculate();
        }

        partial void OnHeightChanged(int value)
        {
            this.Height = (int)Height;
        }

        private bool CheckEnableButtonCalculate()
        {
            return string.IsNullOrEmpty(this.WeightErrMess) && string.IsNullOrEmpty(this.AgeErrMess)
            && !string.IsNullOrEmpty(this.WeightValue) && (AgeValue >= 1);
        }

        [RelayCommand]
        void Increase(object data)
        {
            if (data is null)
                return;

            if (data.ToString() == "weight")
            {
                if (WeightValue is null)
                    WeightValue = "0";

                if (Int32.TryParse(WeightValue, out int weightNumber))
                {
                    WeightValue = (++weightNumber).ToString();
                }
            }
            else
            {
                AgeValue++;
            }
        }

        [RelayCommand]
        void Decrease(object data)
        {
            if (data is null)
                return;

            if (data.ToString() == "weight")
            {
                if (WeightValue is null)
                    WeightValue = "0";

                if (Int32.TryParse(WeightValue, out int weightNumber) && weightNumber >= 1)
                {
                    WeightValue = (--weightNumber).ToString();
                }
            }

            if (data.ToString() == "age")
            {
                AgeValue--;
            }
        }

        public void Receive(BmiResultSelectedOptionMessage message)
        {
            if (message is not null)
            {
                var param = message.Value;
                if (Application.Current?.MainPage is AppShell currentShell)
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        currentShell.SwitchShellContentToolsTab(param, this.bmiResultData);
                    });
                }
            }
        }
    }
}
