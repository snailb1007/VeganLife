namespace VeganLife.Helpers.AppSetting
{
    public static class ConstantHelper
    {
        public const string Language_Vietnam = "vi";
        public const string Language_English = "en";

        public const string Theme_Mode_Auto = "auto";
        public const string Theme_Mode_Fixed = "fixed";

        public const string DatabaseFileName = "SQLiteVeganLife.db3";
        public const SQLite.SQLiteOpenFlags SQLiteFlags =
            // open database in read/write mdoe
            SQLite.SQLiteOpenFlags.ReadWrite |
            // create database if doesnt exist
            SQLite.SQLiteOpenFlags.Create |
            // multi-thread database access
            SQLite.SQLiteOpenFlags.SharedCache;
        public static string DatabasePath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DatabaseFileName);

        public static class RssFeedNews
        {
            public const string Google_News_VeganFoods = "https://news.google.com/rss/search?q=m%C3%B3n%20chay&hl=vi&gl=VN&ceid=VN%3Avi";
            public const string Google_News_VeganHealthy = "https://news.google.com/rss/search?q=s%C6%B0c%20kh%E1%BB%8Fe%20thu%E1%BA%A7n%20chay&hl=vi&gl=VN&ceid=VN%3Avi";
            public const string Google_News_Religion = "https://news.google.com/rss/search?q=%C4%91%E1%BA%A1o%20ph%E1%BA%ADt&hl=vi&gl=VN&ceid=VN%3Avi";
            public const string Google_News_LiveStrong = "https://news.google.com/rss/search?q=t%E1%BA%ADp%20luy%E1%BB%87n%20s%E1%BB%91ng%20kh%E1%BB%8Fe&hl=vi&gl=VN&ceid=VN%3Avi";
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

        public static class APIConstants
        {
            public const string OpenAIUrl = "https://api.openai.com/";
            public const string OpenAIToken = "M-A-8-2-c-w-u-9-a-5-8-8-E-L-u-g-R-S-n-1-T-3-B-l-b-k-F-J-I-K-8-q-F-n-i-N-R-9-B-K-Q-y-t-9-T-S-i-f";
            public const string OpenAIEndpoint_Completions = "v1/completions";
        }
    }
}
