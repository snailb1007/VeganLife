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
            public const string Google_News_VeganFoods = "https://news.google.com/rss/search?q=m%C3%B3n%20chay&hl=vi&gl=VN&ceid=VN%3Avi";
            public const string Google_News_VeganHealthy = "https://news.google.com/rss/search?q=s%C6%B0c%20kh%E1%BB%8Fe%20thu%E1%BA%A7n%20chay&hl=vi&gl=VN&ceid=VN%3Avi";
            public const string Google_News_Religion = "https://news.google.com/rss/search?q=%C4%91%E1%BA%A1o%20ph%E1%BA%ADt&hl=vi&gl=VN&ceid=VN%3Avi";
            public const string Google_News_LiveStrong = "https://news.google.com/rss/search?q=t%E1%BA%ADp%20luy%E1%BB%87n%20s%E1%BB%91ng%20kh%E1%BB%8Fe&hl=vi&gl=VN&ceid=VN%3Avi";
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

        public static class AppSetting
        {
            public static bool IsVietnameseLang { get; set;}
        }

        public static class Validator
        {
            public const string WeightBMIRegexPattern = @"^(?:[1-9]\d*|0)+(?:\.(\d)?(\d)?)?$";
            public const string AgeBMIRegexPattern = @"^\d+$";
        }

    }
}
