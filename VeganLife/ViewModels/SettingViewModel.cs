using VeganLife.Helpers;
using VeganLife.Helpers.AppSetting;
using VeganLife.Views.SettingTab;

namespace VeganLife.ViewModels
{
    public partial class SettingViewModel : BaseViewModel
    {
        [ObservableProperty]
        bool _isDarkMode;

        [ObservableProperty]
        string _imgBackground = string.Empty;

        [ObservableProperty]
        int _indexPickerOption;

        public SettingViewModel(INavigationService navigationService, IDataService dataService) : base(navigationService, dataService)
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

        [RelayCommand]
        void SwitchTheme(Microsoft.Maui.Controls.Switch parameter)
        {
            if (parameter == null)
                return;
            IsDarkMode = parameter.IsToggled;
            var goalTheme = IsDarkMode ? AppTheme.Dark : AppTheme.Light;
            AppThemeHelper.SetTheme(goalTheme);
            UserSettingsHelper.Set(UserSettingKey.SelectedTheme, goalTheme.ToString());
        }

        [RelayCommand]
        async Task OpenLicensePage()
        {
            await navigationService.NavigataToPage<LicensePage>();
        }
    }
}
