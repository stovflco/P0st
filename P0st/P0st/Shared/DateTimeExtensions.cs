using System;

namespace Pr0mpp.Shared
{
    public static class DateTimeExtensions
    {
        public static string AsReadableString(this DateTime dt)
        {
            if (dt == DateTime.Now)
            {
                return "jetzt";
            }
            
            const int SECOND = 1;
            const int MINUTE = 60 * SECOND;
            const int HOUR = 60 * MINUTE;
            const int DAY = 24 * HOUR;
            const int MONTH = 30 * DAY;

            var ts = new TimeSpan(DateTime.UtcNow.Ticks - dt.Ticks);
            double delta = Math.Abs(ts.TotalSeconds);

            if (delta < 1 * MINUTE)
                return ts.Seconds == 1 ? "vor einer Sekunde" : $"vor {ts.Seconds} Sekunden";

            if (delta < 2 * MINUTE)
                return "vor einer Minute";

            if (delta < 45 * MINUTE)
                return $"vor {ts.Minutes} Minuten";

            if (delta < 90 * MINUTE)
                return "vor einer Stunde";

            if (delta < 24 * HOUR)
                return $"vor {ts.Hours} Stunden";

            if (delta < 48 * HOUR)
                return "gestern";

            if (delta < 30 * DAY)
                return $"vor {ts.Days} Tagen";

            if (delta < 12 * MONTH)
            {
                int months = Convert.ToInt32(Math.Floor((double) ts.Days / 30));
                return months <= 1 ? "vor einem Monat" : $"vor {months} Monaten";
            }
            else
            {
                int years = Convert.ToInt32(Math.Floor((double) ts.Days / 365));
                return years <= 1 ? "vor einem Jahr" : $"vor {years} Jahren";
            }
        }
    }
}