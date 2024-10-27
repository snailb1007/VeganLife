// <copyright file="DateTimeHelper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using VeganLife.Resources.Translations;

namespace VeganLife.Helpers
{
    public static class DateTimeHelper
    {
        public static DateTime GetDateTime(string dateString, string format = "yyyy-MM-dd")
        {
            if (string.IsNullOrEmpty(dateString))
            {
                return DateTime.MinValue;
            }

            // string parseFormat = "ddd, dd MMM yyyy HH:mm:ss 'GMT'K";
            return DateTime.ParseExact(dateString, format, CultureInfo.InvariantCulture);
        }

        public static string CalculateTimeAgo(DateTime dateTime)
        {
            const int SECOND = 1;
            const int MINUTE = 60 * SECOND;
            const int HOUR = 60 * MINUTE;
            const int DAY = 24 * HOUR;
            const int MONTH = 30 * DAY;

            var ts = new TimeSpan(DateTime.UtcNow.Ticks - dateTime.Ticks);
            double delta = Math.Abs(ts.TotalSeconds);

            if (delta < 1 * MINUTE)
            {
                return ts.Seconds == 1 ? AppResources.appSet_dateTime_1s_ago : ts.Seconds + AppResources.appSet_dateTime_manyS_ago;
            }
            else if (delta < 2 * MINUTE)
            {
                return AppResources.appSet_dateTime_1m_ago;
            }
            else if (delta < 45 * MINUTE)
            {
                return ts.Minutes + AppResources.appSet_dateTime_manyM_ago;
            }
            else if (delta < 90 * MINUTE)
            {
                return AppResources.appSet_dateTime_1h_ago;
            }
            else if (delta < 24 * HOUR)
            {
                return ts.Hours + AppResources.appSet_dateTime_manyH_ago;
            }
            else if (delta < 48 * HOUR)
            {
                return AppResources.appSet_dateTime_yesterday_ago;
            }
            else if (delta < 30 * DAY)
            {
                return ts.Days + AppResources.appSet_dateTime_days_ago;
            }
            else if (delta < 12 * MONTH)
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return months <= 1 ? AppResources.appSet_dateTime_1Month_ago : months + AppResources.appSet_dateTime_months_ago;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
