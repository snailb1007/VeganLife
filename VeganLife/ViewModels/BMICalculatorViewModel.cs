using System.Text.RegularExpressions;
using VeganLife.Helpers.AppSetting;

namespace VeganLife.ViewModels
{
    public partial class BMICalculatorViewModel : BaseViewModel
    {
        private float _weight;
        private short _age;

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

        [RelayCommand]
        void CalculateBMI()
        {

        }

        public BMICalculatorViewModel(INavigationService navigationService, IDataService dataService)
            : base(navigationService, dataService)
        {
            Init();
        }

        private void Init()
        { }

        partial void OnWeightValueChanged(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                WeightErrMess = null;
                IsEnableSubmit = false;
                return;
            }

            Regex pattern = new (ConstantHelper.Validator.WeightBMIRegexPattern);
            if (pattern.IsMatch(value))
            {
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
            }
            else
            {
                WeightErrMess = "Sai định dạng";
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

            Regex pattern = new(ConstantHelper.Validator.AgeBMIRegexPattern);
            if (pattern.IsMatch(value))
            {
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
            }
            else
            {
                AgeErrMess = "Sai định dạng";
            }

            IsEnableSubmit = CheckEnableButtonCalculate();
        }

        private bool CheckEnableButtonCalculate()
        {
            return string.IsNullOrEmpty(WeightErrMess) && string.IsNullOrEmpty(AgeErrMess)
            && !string.IsNullOrEmpty(WeightValue) && !string.IsNullOrEmpty(AgeValue);
        }
    }
}
