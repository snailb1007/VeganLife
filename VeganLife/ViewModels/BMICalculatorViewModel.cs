using Mopups.Services;
using VeganLife.Helpers;
using VeganLife.Resources.Translations;
using VeganLife.Views.Popups;

namespace VeganLife.ViewModels
{
    public partial class BMICalculatorViewModel : BaseViewModel
    {
        public string WeightBMIRegexPattern { get; } = @"^(?:[1-9]\d*|0)+(?:\.(\d)?(\d)?)?$";
        public string AgeBMIRegexPattern { get; } = @"^\d+$";

        private float _weight;
        private short _age;

        [ObservableProperty]
        float _height;

        [ObservableProperty]
        bool _isDisplayedSexDetail;

        [ObservableProperty]
        bool _isMale;

        [ObservableProperty]
        bool _isEnableSubmit;

        [ObservableProperty]
        string _weightValue;

        [ObservableProperty]
        string _ageValue;

        [ObservableProperty]
        string _backgroundIMG;

        [ObservableProperty]
        string _weightErrMess;

        [ObservableProperty]
        string _ageErrMess;

        [ObservableProperty]
        string _generalError;

        [ObservableProperty]
        float _bmiResult;

        [RelayCommand]
        void HelpSexDetail(string parameter)
        {
            if (!string.IsNullOrEmpty(parameter) && parameter.Equals("closeSexDetail"))
                IsDisplayedSexDetail = false;
            else
                IsDisplayedSexDetail = !IsDisplayedSexDetail;
        }

        [RelayCommand]
        void UnFocus(object obj)
        {
            if (obj == null)
                return;
            var view = (BMICalculatorPage)obj;
            if (view == null) return;
            var entryWeight = view.FindByName("entryWeight") as Entry;

            if (entryWeight != null && entryWeight.IsFocused)
            {
                device_service.HideKeyboard();
                entryWeight.Unfocus();
                return;
            }

            var entryAge = view.FindByName("entryAge") as Entry;
            if (entryAge != null && entryAge.IsFocused)
            {
                device_service.HideKeyboard();
                entryAge.Unfocus();
            }
        }

        [RelayCommand]
        void SelectGender(string parameter)
        {
            IsMale = !IsMale;
        }

        public BMICalculatorViewModel(INavigationService navigationService, IDataService dataService)
            : base(navigationService, dataService)
        {
            Init();
        }

        private void Init()
        { }

        [RelayCommand]
        async Task CalculateBMI()
        {
            BmiResult = BMICalculateHelper.Calculate(_weight, Height / 100f);
            await MopupService.Instance.PushAsync(new BmiResultPopup(new BMIResultModel()
            {
                BMIResult = BmiResult,
                Sex = IsMale ? AppResources.male_bmiPage : AppResources.female_bmiPage,
                Age = AgeValue
            }));
        }

        partial void OnWeightValueChanged(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                WeightErrMess = null;
                IsEnableSubmit = false;
                return;
            }

            _weight = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
            if (_weight < 2)
            {
                WeightErrMess = "Qúa thấp, dường như bạn nhập sai";
            }
            else if (_weight > 635)
            {
                WeightErrMess = "Qúa lớn, dường như bạn nhập sai";
            }
            else
            {
                WeightErrMess = null;
            }

            IsEnableSubmit = CheckEnableButtonCalculate();
        }

        partial void OnAgeValueChanged(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                AgeErrMess = null;
                IsEnableSubmit = false;
                return;
            }

            _age = short.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
            if (_age <= 1)
            {
                AgeErrMess = "Quá nhỏ, nhập lại";
            }
            else if (_age >= 140)
            {
                AgeErrMess = "Quá lớn, dường như bạn nhập sai";
            }
            else
            {
                AgeErrMess = null;
            }

            IsEnableSubmit = CheckEnableButtonCalculate();
        }

        partial void OnHeightChanged(float value)
        {
            Height = (float)Math.Round(Height, 2);
        }

        private bool CheckEnableButtonCalculate()
        {
            return string.IsNullOrEmpty(WeightErrMess) && string.IsNullOrEmpty(AgeErrMess)
            && !string.IsNullOrEmpty(WeightValue) && !string.IsNullOrEmpty(AgeValue);
        }
    }
}
