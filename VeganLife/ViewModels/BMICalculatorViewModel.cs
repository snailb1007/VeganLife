using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kotlin.Text;
using System.Globalization;
using VeganLife.Helpers.AppSetting;
using VeganLife.Services;
using VeganLife.Views;

namespace VeganLife.ViewModels
{
    public partial class BMICalculatorViewModel : ObservableObject
    {
        private float _weight;
        private short _age;

        [ObservableProperty]
        bool _isDisplayedSexDetail;

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
            var entryWeight = view.FindByName("entryWeight") as Entry;

            if (entryWeight != null && entryWeight.IsFocused)
            {
                var deviceService = new DeviceService();
                deviceService.HideKeyboard();
                entryWeight.Unfocus();
                return;
            }

            var entryAge = view.FindByName("entryAge") as Entry;
            if (entryAge != null && entryAge.IsFocused)
            {
                var deviceService = new DeviceService();
                deviceService.HideKeyboard();
                entryAge.Unfocus();
            }
        }

        public BMICalculatorViewModel()
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
                return;
            }

            Regex pattern = new (ConstantHelper.Validator.WeightBMIRegexPattern);
            if (pattern.Matches(value))
            {
                _weight = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
                if (_weight < 2 && _weight > 635)
                {
                    WeightErrMess = "";
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
        }

        partial void OnAgeValueChanged(string value)
        {
            Console.WriteLine($"thien==>{_ageValue}");
        }
    }
}
