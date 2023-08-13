// <copyright file="ConstantHelper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.Helpers.AppSetting
{
    public static class ConstantHelper
    {
        public const float AverageDaysInYear = 365.25f;
        public const string LanguageVietnam = "vi";
        public const string LanguageEnglish = "en";

        public const string ThemeModeAuto = "auto";
        public const string ThemeModeFixed = "fixed";

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
            public const string GoogleNewsVeganFoods = "https://news.google.com/rss/search?q=m%C3%B3n%20chay&hl=vi&gl=VN&ceid=VN%3Avi";
            public const string GoogleNewsVeganHealthy = "https://news.google.com/rss/search?q=s%C6%B0c%20kh%E1%BB%8Fe%20thu%E1%BA%A7n%20chay&hl=vi&gl=VN&ceid=VN%3Avi";
            public const string GoogleNewsReligion = "https://news.google.com/rss/search?q=%C4%91%E1%BA%A1o%20ph%E1%BA%ADt&hl=vi&gl=VN&ceid=VN%3Avi";
            public const string GoogleNewsLiveStrong = "https://news.google.com/rss/search?q=t%E1%BA%ADp%20luy%E1%BB%87n%20s%E1%BB%91ng%20kh%E1%BB%8Fe&hl=vi&gl=VN&ceid=VN%3Avi";
        }

        public static class BmiData
        {
            public const string Male = "Male";
            public const string Female = "Female";
        }
    }
}
