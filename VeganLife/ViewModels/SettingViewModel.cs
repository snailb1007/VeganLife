using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using VeganLife.Data.FireBaseData;
using VeganLife.Helpers;
using VeganLife.Helpers.AppSetting;
using VeganLife.Views;

namespace VeganLife.ViewModels
{
    public partial class SettingViewModel : ObservableObject
    {
        const string _basicPickerOption = "Cơ bản";
        const string _universePickerOption = "Vũ trụ";

        [ObservableProperty]
        bool _isDarkMode;

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
            IsDarkMode = parameter.IsToggled;
            var goalTheme = _isDarkMode ? AppTheme.Dark : AppTheme.Light;
            AppThemeHelper.SetTheme(goalTheme);

            UserSettingsHelper.Set(UserSettingKey.ThemeMode, ConstantHelper.Theme_Mode_Fixed);
            UserSettingsHelper.Set(UserSettingKey.SelectedTheme, goalTheme.ToString());
        }

        public SettingViewModel()
        {
            Init();
        }

        private void Init()
        {
            IndexPickerOption = 0;
            ImgBackground = ConstantHelper.ThemeInfo.ImgBackground;
            var currentDeviceTheme = App.Current.UserAppTheme;
            IsDarkMode = currentDeviceTheme == AppTheme.Dark;
        }

        partial void OnIndexPickerOptionChanged(int value)
        {
            //if (value == 1)
            //{
            //    _ = SetupThemeIMG();
            //}
        }

        private async Task SetupThemeIMG()
        {
            //if (ConstantHelper.FirebaseData.FirebaseRealtimeData == null)
            //{
            //    ConstantHelper.FirebaseData.FirebaseRealtimeData = new FirebaseRealtimeData();
            //}

            //var backgrounds = await ConstantHelper.FirebaseData.FirebaseRealtimeData.GetBackgroundImage("dark_2k");
            //ConstantHelper.ThemeInfo.ImgBackground = backgrounds;
            //App.Current.MainPage = new AppShell();
        }
    }
}
