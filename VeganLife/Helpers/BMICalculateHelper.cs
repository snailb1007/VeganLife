// <copyright file="BMICalculateHelper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using VeganLife.Resources.Translations;
using static VeganLife.Helpers.AppSetting.StaticHelper;

namespace VeganLife.Helpers
{
    public static class BMICalculateHelper
    {
        /// <summary>
        /// get bmi.
        /// </summary>
        /// <param name="mass">mass to Calculate.</param>
        /// <param name="height">height to Calculate.</param>
        /// <returns>Bmi value.</returns>
        public static double Calculate(float mass, float height) => Math.Round(mass / (height * height), 2);

        // Orange: Underweight (Mild thinness)
        // Green: Normal range
        // Yellow: Overweight (Pre-obese)
        // Red: Obese

        /// <summary>
        /// Get bmi status.
        /// </summary>
        /// <param name="age">age to analysis.</param>
        /// <param name="isMale">sex to analysis.</param>
        /// <param name="bmiData">bmiData to analysis.</param>
        /// <returns>Color for UI.</returns>
        public static HealthDiagnosisModel GetWeightStatusCategory(short age, bool isMale, float bmiData)
        {
            switch (age)
            {
                case < 5:
                    return GetWSKLessThen5Age(bmiData);
                case < 19:
                    return GetWSKLessThen20Age(isMale, bmiData);
                default:
                    return GetWSKForAdults(bmiData);
            }
        }

        private static HealthDiagnosisModel GetWSKLessThen5Age(float bmi)
        {
            var result = new HealthDiagnosisModel();
            result.Note = HealthDiagnosisFirebaseDataModel.BMIModel?.LessThan5Age?.Note ?? string.Empty;
            result.Classify = AppResources.lessThan5_notFound_toolFlyout;
            result.Documents = HealthDiagnosisFirebaseDataModel.BMIModel?.LessThan5Age?.Documents ?? string.Empty;
            switch (bmi)
            {
                case < 14:
                    result.StatusColor = Colors.Orange;
                    break;
                case < 20:
                    result.StatusColor = Colors.Green;
                    break;
                case <= 24:
                    result.StatusColor = Colors.Yellow;
                    break;
                default:
                    result.StatusColor = Colors.Red;
                    break;
            }

            return result;
        }

        private static HealthDiagnosisModel GetWSKLessThen20Age(bool isMale, float bmi)
        {
            var result = new HealthDiagnosisModel();
            result.Note = HealthDiagnosisFirebaseDataModel.BMIModel?.LessThan20Age?.Note ?? string.Empty;
            result.Documents = HealthDiagnosisFirebaseDataModel.BMIModel?.LessThan20Age?.Documents ?? string.Empty;
            result.Classify = AppResources.lessThan20_notFound_toolFlyout;
            if (isMale)
            {
                result.ChartLink = HealthDiagnosisFirebaseDataModel.BMIModel?.LessThan20Age?.ChartLink?.Boy ?? string.Empty;
                switch (bmi)
                {
                    case < 14:
                        result.StatusColor = Colors.Orange;
                        break;
                    case < 20:
                        result.StatusColor = Colors.Green;
                        break;
                    case < 25:
                        result.StatusColor = Colors.Yellow;
                        break;
                    default:
                        result.StatusColor = Colors.Red;
                        break;
                }
            }
            else
            {
                result.ChartLink = HealthDiagnosisFirebaseDataModel.BMIModel?.LessThan20Age?.ChartLink?.Girl ?? string.Empty;
                switch (bmi)
                {
                    case < 14:
                        result.StatusColor = Colors.Orange;
                        break;
                    case < 20:
                        result.StatusColor = Colors.Green;
                        break;
                    case < 24:
                        result.StatusColor = Colors.Yellow;
                        break;
                    default:
                        result.StatusColor = Colors.Red;
                        break;
                }
            }

            return result;
        }

        // public const byte Underweight3Threshold = 16;
        // public const byte Underweight2Threshold = 17;
        // public const float Underweight1Threshold = 18.5f;
        // public const byte NormalThreshold = 25;
        // public const byte ObeseThreshold = 30;
        // public const float Obese1Threshold = 35;
        // public const float obsese2Threshold = 40;
        public const float NormalAVG = 21.75f;

        private static HealthDiagnosisModel GetWSKForAdults(float bmi)
        {
            var result = new HealthDiagnosisModel();
            result.Note = HealthDiagnosisFirebaseDataModel.BMIModel?.Adult?.Note ?? string.Empty;
            result.Documents = HealthDiagnosisFirebaseDataModel.BMIModel?.Adult?.Documents ?? string.Empty;
            switch (bmi)
            {
                case < 16:
                    result.StatusColor = Color.FromArgb("#ffda95"); // Underweight 3 (Severe thinness)
                    result.Classify = AppResources.severeThinness_classify_bmi;
                    break;
                case < 17:
                    result.StatusColor = Color.FromArgb("#ffc251"); // Underweight 2 (Moderate thinness)
                    result.Classify = AppResources.moderateThinness_classify_bmi;
                    break;
                case < 18.5f:
                    result.StatusColor = Colors.Orange; // Underweight 1 (thinness)
                    result.Classify = AppResources.Thinness_classify_bmi;
                    break;
                case < 25:
                    result.StatusColor = Colors.Green;
                    result.Classify = AppResources.normal_classify_bmi;
                    break;
                case < 30:
                    result.StatusColor = Colors.Yellow;
                    result.Classify = AppResources.obese_classify_bmi;
                    break;
                case < 35:
                    result.StatusColor = Colors.Red;
                    result.Classify = AppResources.obese1_classify_bmi; // Obese (Class I)
                    break;
                case < 40:
                    result.StatusColor = Color.FromArgb("#a50000 "); // Obese (Class II)
                    result.Classify = AppResources.obese2_classify_bmi;
                    break;
                default:
                    result.StatusColor = Color.FromArgb("#690000"); // Obese (Class III)
                    result.Classify = AppResources.obese3_classify_bmi;
                    break;
            }

            return result;
        }
    }
}
