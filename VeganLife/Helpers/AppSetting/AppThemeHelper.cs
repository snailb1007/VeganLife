using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeganLife.Helpers.AppSetting
{
    public static class AppThemeHelper
    {
        public static void SetTheme(AppTheme theme)
        {
            App.Current.UserAppTheme = theme;
            ConstantHelper.ThemeInfo.IsDarkMode = theme == AppTheme.Dark;
        }

        public static string GetBackgroundIMG()
        {
            string result = string.Empty;
            if (ConstantHelper.ThemeInfo.IsDarkMode)
            {
                result = ConstantHelper.ThemeInfo.UniverseDarkLink;
            }

            return result;
        }
    }
}
