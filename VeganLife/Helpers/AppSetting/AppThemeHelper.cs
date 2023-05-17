namespace VeganLife.Helpers.AppSetting
{
    public static class AppThemeHelper
    {
        public static void SetTheme(AppTheme theme)
        {
            App.Current.UserAppTheme = theme;
            ConstantHelper.ThemeInfo.IsDarkMode = theme == AppTheme.Dark;
        }
    }
}
