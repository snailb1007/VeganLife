// <copyright file="BMICalculateHelper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.Helpers
{
    using VeganLife.Helpers.AppSetting;

    public static class BMICalculateHelper
    {
        /// <summary>
        /// get bmi.
        /// </summary>
        /// <param name="mass">mass to Calculate.</param>
        /// <param name="height">height to Calculate.</param>
        /// <returns>Bmi value.</returns>
        public static double Calculate(float mass, float height) => Math.Round((mass / (height * height)),2);

        // Orange: Underweight (Mild thinness)
        // Green: Normal range
        // Yellow: Overweight (Pre-obese)
        // Red: Obese

        /// <summary>
        /// Get bmi status.
        /// </summary>
        /// <param name="age">age to analysis.</param>
        /// <param name="sex">sex to analysis.</param>
        /// <param name="bmiData">bmiData to analysis.</param>
        /// <returns>Color for UI.</returns>
        public static Color GetWeightStatusCategory(short age, string sex, float bmiData)
        {
            switch (age)
            {
                case < 5:
                    return GetWSKLessThen5Age(bmiData);
                case < 19:
                    return GetWSKLessThen20Age(sex, bmiData);
                default:
                    return GetWSKForAdults(bmiData);
            }
        }

        private static Color GetWSKLessThen5Age(float bmi)
        {
            switch (bmi)
            {
                case < 14:
                    return Colors.Orange;
                case < 20:
                    return Colors.Green;
                case <= 24:
                    return Colors.Yellow;
                default: return Colors.Red;
            }
        }

        private static Color GetWSKLessThen20Age(string sex, float bmi)
        {
            if (sex.Equals(ConstantHelper.BmiData.Male))
            {
                switch (bmi)
                {
                    case < 14:
                        return Colors.Orange;
                    case < 20:
                        return Colors.Green;
                    case < 25:
                        return Colors.Yellow;
                    default: return Colors.Red;
                }
            }
            else
            {
                switch (bmi)
                {
                    case < 14:
                        return Colors.Orange;
                    case < 20:
                        return Colors.Green;
                    case < 24:
                        return Colors.Yellow;
                    default: return Colors.Red;
                }
            }
        }

        private static Color GetWSKForAdults(float bmi)
        {
            switch (bmi)
            {
                case < 16:
                    return Color.FromArgb("#ffda95"); // Underweight (Severe thinness)
                case < 17:
                    return Color.FromArgb("#ffc251"); // Underweight (Moderate thinness)
                case < 18.5f:
                    return Colors.Orange;
                case < 25:
                    return Colors.Green;
                case < 30:
                    return Colors.Yellow;
                case < 35:
                    return Colors.Red;
                case < 40:
                    return Color.FromArgb("#a50000 "); // Obese (Class II)
                default: return Color.FromArgb("#690000"); // Obese (Class III)
            }
        }
    }
}
