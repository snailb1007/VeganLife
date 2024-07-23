using static VeganLife.Helpers.AppSetting.ConstantHelper.CalculateHelper;

namespace VeganLife.Helpers
{
    public static class TDEEHelper
    {
        public static float GetActivityLevel(ActivityLevel activityLevel)
        {
            switch (activityLevel)
            {
                case ActivityLevel.Sedentary:
                    return SedentaryValue;
                case ActivityLevel.LightlyActive:
                    return LightlyActiveValue;
                case ActivityLevel.ModeratelyActive:
                    return ModeratelyActiveValue;
                case ActivityLevel.VeryActive:
                    return VeryActiveValue;
                case ActivityLevel.SuperActive:
                    return SedentaryValue;
                default:
                    return SedentaryValue;
            }
        }

        public static float CalculateTDEE(float bmr, ActivityLevel activityLevel)
        {
            return bmr * GetActivityLevel(activityLevel);
        }

        public static float CalculateBMR(float weight, float height, int age, bool isMale)
        {
            return isMale
                ? 88.362f + (13.397f * weight) + (4.799f * height) - (5.677f * age)
                : 447.593f + (9.247f * weight) + (3.098f * height) - (4.330f * age);
        }
    }
}
