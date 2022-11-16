using CommunityToolkit.Mvvm.ComponentModel;
using Firebase.Database;
using VeganLife.Data.FireBaseData;

namespace VeganLife.Helpers.AppSetting
{
    public static class ConstantHelper
    {
        public const string Language_Vietnam = "vi";
        public const string Language_English = "en";

        public const string Theme_Mode_Auto = "auto";
        public const string Theme_Mode_Fixed = "fixed";

        public static class RssFeedNews
        {
            public const string Google_News = "https://rss.app/feeds/7fS2eOsF7lFj2faH.xml";
        }

        public static class FirebaseData
        {
            public static FirebaseRealtimeData FirebaseRealtimeData;
        }

        public static class ThemeInfo
        {
            public static string ImgBackground { get; set; }
            public static bool IsDarkMode { get; set; }
        }

        public static class Validator
        {
            public const string WeightBMIRegexPattern = @"^(?:[1-9]\d*|0)+(?:\.(\d)?(\d)?)?$";
            public const string AgeBMIRegexPattern = @"^\d+$";
        }

    }
}
