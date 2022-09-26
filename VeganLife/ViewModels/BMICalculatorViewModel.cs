using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace VeganLife.ViewModels
{
    public partial class BMICalculatorViewModel : ObservableObject
    {
        [ObservableProperty]
        bool _isDisplayedSexDetail = true;

        [ObservableProperty]
        string _weightValue;

        [ObservableProperty]
        string _ageValue;

        [RelayCommand]
        void HelpSexDetail()
        {
            IsDisplayedSexDetail = !IsDisplayedSexDetail;
        }

        public BMICalculatorViewModel()
        {
        }

        private void Init()
        { }

        partial void OnWeightValueChanged(string value)
        {
            if (value.Length < 6)
                return;
            var abc = string.Format("0:0.00", value);
            bool isSuccess = float.TryParse(value, out float x);
            string result = x.ToString();
            if (!isSuccess || !result.Equals(value))
                return;
            WeightValue = result;
        }

        partial void OnAgeValueChanged(string value)
        {
            Console.WriteLine($"thien==>{_ageValue}");
        }
    }
}
