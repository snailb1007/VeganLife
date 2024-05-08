// <copyright file="ConstantHelper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

// Ignore Spelling: App Rss Bmi
namespace VeganLife.Helpers.AppSetting
{
    public static class ConstantHelper
    {
        public static class OpenAIConstant
        {
            public const string OpenAIUrl = "https://api.openai.com/";
            public const string OpenAITokenVip = "***REMOVED***";

            public const string OpenAIEndpoint_Completions = "https://api.openai.com/v1/completions";

            // public const string OpenAIEndpoint_Generations = "v1/images/generations";
        }

        public const float AverageDaysInYear = 365.25f;
        public const string LanguageVietnam = "vi";
        public const string LanguageEnglish = "en";

        public const string ThemeModeAuto = "auto";
        public const string ThemeModeFixed = "fixed";

        public const string DatabaseFileName = "SQLiteVeganLife.db3";
        public const SQLite.SQLiteOpenFlags SQLiteFlags =

            // open database in read/write mode
            SQLite.SQLiteOpenFlags.ReadWrite |

            // create database if does not exist
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

        // Ignore Spelling: Proximates
        public static class UsdaFoodNutrition
        {
            public const string Proximates = "Proximates";
            public const string Water = "Water";
            public const string Energy = "Energy";
            public const string Protein = "Protein";
            public const string Fat = "fat";
            public const string Ash = "Ash";
            public const string Carbohydrate = "Carbohydrate";
            public const string Fiber = "Fiber";
            public const string Sugars = "Sugars";
            public const string Sucrose = "Sucrose";
            public const string Glucose = "Glucose";
            public const string Fructose = "Fructose";
            public const string Lactose = "Lactose";
            public const string Maltose = "Maltose";
            public const string GaLactose = "GaLactose";
            public const string Starch = "Starch";
            public const string Minerals = "Minerals";
            public const string Calcium = "Calcium";
            public const string Iron = "Iron";
            public const string Magnesium = "Magnesium";
            public const string Phosphorus = "Phosphorus";
            public const string Potassium = "Potassium";
            public const string Sodium = "Sodium";
            public const string Zinc = "Zinc";
            public const string Copper = "Copper";
        }

        public const string TAG = "undefined";
    }
}
