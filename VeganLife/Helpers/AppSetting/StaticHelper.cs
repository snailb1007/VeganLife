using VeganLife.Models.FirebaseDataModel;

namespace VeganLife.Helpers.AppSetting
{
    public static class StaticHelper
    {
        public static class ThemeInfo
        {
            public static bool IsDarkMode { get; set; }
        }

        public static class AppSetting
        {
            public static bool IsVietnameseLang { get; set; }
        }
        public static class HealthDiagnosisFirebaseDataModel
        {
            public static BMIModel BMIModel { get; set; }
        }

    }
}
