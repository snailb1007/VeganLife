// <copyright file="AppThemeHelper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.Helpers.AppSetting
{
    public static class AppThemeHelper
    {
        public static void SetTheme(AppTheme theme)
        {
            App.Current.UserAppTheme = theme;
            StaticHelper.ThemeInfo.IsDarkMode = theme == AppTheme.Dark;
        }
    }
}
