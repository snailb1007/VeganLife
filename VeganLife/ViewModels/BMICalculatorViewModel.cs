// <copyright file="BMICalculatorViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.ViewModels
{
    using VeganLife.Helpers;
    using VeganLife.Helpers.AppSetting;
    using VeganLife.Views.Popups;

    public partial class BMICalculatorViewModel : BaseViewModel
    {
        public string WeightBMIRegexPattern { get; } = @"^(?:[1-9]\d*|0)+(?:\.(\d)?(\d)?)?$";

        public string AgeBMIRegexPattern { get; } = @"^\d+$";

        private float weight;
        private short age;

        [ObservableProperty]
        private float height;

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
        private float bmiResult;

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

            var view = (BMICalculatorPage)obj;
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

        public BMICalculatorViewModel()
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
                BMIResult = this.BmiResult,
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

            this.weight = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
            if (weight < 2)
            {
                this.WeightErrMess = "Qúa thấp, dường như bạn nhập sai";
            }
            else if (weight > 635)
            {
                this.WeightErrMess = "Qúa lớn, dường như bạn nhập sai";
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
                this.AgeErrMess = "Quá nhỏ, nhập lại";
            }
            else if (age >= 140)
            {
                this.AgeErrMess = "Quá lớn, dường như bạn nhập sai";
            }
            else
            {
                this.AgeErrMess = null;
            }

            this.IsEnableSubmit = CheckEnableButtonCalculate();
        }

        partial void OnHeightChanged(float value)
        {
            this.Height = (float)Math.Round(Height, 2);
        }

        private bool CheckEnableButtonCalculate()
        {
            return string.IsNullOrEmpty(this.WeightErrMess) && string.IsNullOrEmpty(this.AgeErrMess)
            && !string.IsNullOrEmpty(this.WeightValue) && !string.IsNullOrEmpty(this.AgeValue);
        }
    }
}
