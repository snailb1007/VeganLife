using VeganLife.Helpers.AppSetting;

namespace VeganLife.Helpers
{
    public static class BMICalculateHelper
    {
        public static float Calculate(float mass, float height)
        {
            return mass / (height*height);
        }

        // Orange: Underweight (Mild thinness)
        // Green: Normal range
        // Yellow: Overweight (Pre-obese)
        // Red: Obese
        public static Color GetWeightStatusCategory(short age, string sex, float bmiData)
        {
            switch(age)
            {
                case < 5:
                    return GetWSKLessThen5Age(bmiData);
                case < 19:
                    return GetWSKLessThen20Age(sex, bmiData);
                default:
                    return GetWSKForAdults(bmiData);
            }
        }

        #region kid
        static Color GetWSKLessThen5Age(float bmi)
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

        static Color GetWSKLessThen20Age(string sex, float bmi)
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
        #endregion

        static Color GetWSKForAdults(float bmi)
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
