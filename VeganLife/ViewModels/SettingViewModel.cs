using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using VeganLife.Views;

namespace VeganLife.ViewModels
{
    public partial class SettingViewModel : ObservableObject
    {
        [ObservableProperty]
        bool _isDarkMode = false;

        [RelayCommand]
        async Task GoTools()
        {
            await Shell.Current.GoToAsync(nameof(BMICalculatorPage));
        }

        public SettingViewModel()
        {
            Init();
        }

        private void Init()
        {
            var currentDeviceTheme = App.Current.PlatformAppTheme;
            _isDarkMode = currentDeviceTheme == AppTheme.Dark;
        }
    }
}
