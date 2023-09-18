// <copyright file="BMICalculateHelper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.Helpers
{
    using VeganLife.Helpers.AppSetting;
    using VeganLife.Resources.Translations;
    using static VeganLife.Helpers.AppSetting.StaticHelper;

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
        public static HealthDiagnosisModel GetWeightStatusCategory(short age, string sex, float bmiData)
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

        private static HealthDiagnosisModel GetWSKLessThen5Age(float bmi)
        {
            var result = new HealthDiagnosisModel();
            result.Note = HealthDiagnosisFirebaseDataModel.BMIModel?.LessThan5Age?.Note;
            result.Classify = AppResources.lessThan5_notFound_toolFlyout;
            result.Documents = HealthDiagnosisFirebaseDataModel.BMIModel?.LessThan5Age?.Documents;
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

        private static HealthDiagnosisModel GetWSKLessThen20Age(string sex, float bmi)
        {
            var result = new HealthDiagnosisModel();
            result.Note = HealthDiagnosisFirebaseDataModel.BMIModel?.LessThan20Age?.Note;
            result.Documents = HealthDiagnosisFirebaseDataModel.BMIModel?.LessThan20Age?.Documents;
            result.Classify = AppResources.lessThan20_notFound_toolFlyout;
            if (sex.Equals(ConstantHelper.BmiData.Male))
            {
                result.ChartLink = HealthDiagnosisFirebaseDataModel.BMIModel?.LessThan20Age?.ChartLink?.Boy;
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
                result.ChartLink = HealthDiagnosisFirebaseDataModel.BMIModel?.LessThan20Age?.ChartLink?.Girl;
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

        private static HealthDiagnosisModel GetWSKForAdults(float bmi)
        {
            var result = new HealthDiagnosisModel();
            result.Note = HealthDiagnosisFirebaseDataModel.BMIModel?.Adult?.Note;
            result.Note = HealthDiagnosisFirebaseDataModel.BMIModel?.Adult?.Documents;
            switch (bmi)
            {
                case < 16:
                    result.StatusColor = Color.FromArgb("#ffda95"); // Underweight (Severe thinness)
                    break;
                case < 17:
                    result.StatusColor = Color.FromArgb("#ffc251"); // Underweight (Moderate thinness)
                    break;
                case < 18.5f:
                    result.StatusColor = Colors.Orange;
                    break;
                case < 25:
                    result.StatusColor = Colors.Green;
                    break;
                case < 30:
                    result.StatusColor = Colors.Yellow;
                    break;
                case < 35:
                    result.StatusColor = Colors.Red;
                    break;
                case < 40:
                    result.StatusColor = Color.FromArgb("#a50000 "); // Obese (Class II)
                    break;
                default:
                    result.StatusColor = Color.FromArgb("#690000"); // Obese (Class III)
                    break;
            }

            return result;
        }
    }
}
