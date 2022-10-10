using CommunityToolkit.Mvvm.ComponentModel;
using Firebase.Database;

namespace VeganLife.Helpers.AppSetting
{
    public static class ConstantHelper
    {
        public static class FirebaseRealtimeData
        {
            public static FirebaseClient FirebaseClient;
        }

        public static class ThemeInfo
        {
            public static string ImgBackground { get; set; }
            public static bool IsDarkMode { get; set; }
        }
    }
}
