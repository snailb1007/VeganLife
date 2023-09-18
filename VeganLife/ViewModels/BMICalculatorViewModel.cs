// <copyright file="BMICalculatorViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.ViewModels
{
    using VeganLife.Helpers;
    using VeganLife.Helpers.AppSetting;
    using VeganLife.Views.Popups;
    using VeganLife.Views.ToolFlyout;

    public partial class MainToolViewModel : BaseViewModel
    {
        public string WeightBMIRegexPattern { get; } = @"^(?:[1-9]\d*|0)+(?:\.(\d)?(\d)?)?$";

        public string AgeBMIRegexPattern { get; } = @"^\d+$";

        private float weight;
        private short age;

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
        private string ageValue;

        [ObservableProperty]
        private string backgroundIMG;

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

        private void Init()
        {
        }

        [RelayCommand]
        private async Task CalculateBMI()
        {
            this.BmiResult = BMICalculateHelper.Calculate(this.weight, this.Height / 100f);
            await ServicesHelper.GetService<IPopupNaviService>().PushAsync<BmiResultPopup>(new BMIResultModel()
            {
                BMIResult = (float)this.BmiResult,
                Sex = this.IsMale ? ConstantHelper.BmiData.Male : ConstantHelper.BmiData.Female,
                Age = this.AgeValue,
            });
        }

        partial void OnWeightValueChanged(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                this.WeightErrMess = null;
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
                this.WeightErrMess = null;
            }

            this.IsEnableSubmit = CheckEnableButtonCalculate();
        }

        partial void OnAgeValueChanged(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                this.AgeErrMess = null;
                this.IsEnableSubmit = false;
                return;
            }

            this.age = short.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
            if (age <= 1)
            {
                this.AgeErrMess = Resources.Translations.AppResources.wrongAge_tooLow_bmiCalculatePage;
            }
            else if (age >= 140)
            {
                this.AgeErrMess = Resources.Translations.AppResources.wrongAge_tooHigh_bmiCalculatePage;
            }
            else
            {
                this.AgeErrMess = null;
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
            && !string.IsNullOrEmpty(this.WeightValue) && !string.IsNullOrEmpty(this.AgeValue);
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

            if (data.ToString() == "age")
            {
                if (AgeValue is null)
                    AgeValue = "0";

                if (Int32.TryParse(AgeValue, out int ageNumber))
                {
                    AgeValue = (++ageNumber).ToString();
                }
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
                if (AgeValue is null)
                    AgeValue = "0";

                if (Int32.TryParse(AgeValue, out int ageNumber) && ageNumber >= 1)
                {
                    AgeValue = (--ageNumber).ToString();
                }
            }
        }
    }
}
