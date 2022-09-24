using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace VeganLife.ViewModels
{
    public partial class BMICalculatorViewModel : ObservableObject
    {
        [ObservableProperty]
        bool _isDisplayedSexDetail;

        [RelayCommand]
        void HelpSexDetail()
        {
            _isDisplayedSexDetail = !_isDisplayedSexDetail;
        }

        public BMICalculatorViewModel()
        {
        }

        private void Init()
        { }
    }
}
