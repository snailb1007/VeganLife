using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using VeganLife.Data.FireBaseData;
using VeganLife.Helpers.AppSetting;
using VeganLife.Views;

namespace VeganLife.ViewModels
{
    public partial class SettingViewModel : ObservableObject
    {
        const string _basicPickerOption = "Cơ bản";
        const string _universePickerOption = "Vũ trụ";

        [ObservableProperty]
        bool _isDarkMode = false;

        [ObservableProperty]
        string _imgBackground = string.Empty;

        [ObservableProperty]
        int _indexPickerOption;

        [ObservableProperty]
        List<string> _themeSkins = new List<string>() { _basicPickerOption, _universePickerOption };

        [RelayCommand]
        async Task GoTools()
        {
            await Shell.Current.GoToAsync(nameof(BMICalculatorPage));
        }

        [RelayCommand]
        void SwitchTheme(Switch parameter)
        {
            if (parameter == null)
                return;
            bool isDarkMode = parameter.IsToggled;
            AppThemeHelper.SetTheme(isDarkMode ? AppTheme.Dark : AppTheme.Light);
        }

        public SettingViewModel()
        {
            Init();
        }

        private void Init()
        {
            IndexPickerOption = 0;
            var currentDeviceTheme = App.Current.PlatformAppTheme;
            if (currentDeviceTheme == AppTheme.Dark)
            {
                IsDarkMode = true;
            }
        }

        partial void OnIndexPickerOptionChanged(int value)
        {
            if (value == 1)
            {
                _ = SetupThemeIMG();
            }
        }

        private async Task SetupThemeIMG()
        {
            var data = new FirebaseRealtimeData();
            var backgrounds = await data.GetBackgroundImage("dark_2k");
            ImgBackground = backgrounds;
        }
    }
}
