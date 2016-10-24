using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SugarMaMa.API.Helpers
{
    public static class DateTimeExtensions
    {
        private const string FormalDateFormat = "MMMM dd, yyyy";
        private const string DateFormat = "MMM dd, yyyy";
        private const string TimeFormat = "h:mm tt";
        /// <summary>
        /// 5:42 PM (PST) on Oct 14, 2014
        /// </summary>
        public static string ToTimeAndShortDate(this DateTime date, TimeZoneInfo timeZone = null)
        {
            var currentDate = DateTime.UtcNow.ConvertFromUtc(timeZone);
            var localDate = date.ConvertFromUtc(timeZone);
            var str = localDate.ToString(string.Format("{0} () on {1}", TimeFormat, DateFormat));
            var localDateWithTz = str.Insert(str.IndexOf("(") + 1, GetTzAbbreviation(currentDate.IsDaylightSavingTime()
                ? timeZone.DaylightName
                : timeZone.StandardName));

            return localDateWithTz;
        }

        /// <summary>
        /// Tuesday, December 1, 2015 at 11:15 AM
        /// </summary>
        public static string ToFormalDateAndTime(this DateTime date, TimeZoneInfo timeZone = null)
        {
            var localDate = date.ConvertFromUtc(timeZone);
            var str = localDate.ToString(string.Format("dddd, {0} - {1}", FormalDateFormat, TimeFormat));
            str = str.Replace("-", "at");
            return str;
        }

        /// <summary>
        /// Oct 14, 2014 5:42 PM (PST)
        /// <param name="timeZone">Passing an empty timezone defaults to PST</param>
        /// </summary>
        public static string ToShortDateAndTime(this DateTime date, TimeZoneInfo timeZone = null)
        {
            var currentDate = DateTime.UtcNow.ConvertFromUtc(timeZone);
            var localDate = date.ConvertFromUtc(timeZone);
            var str = localDate.ToString(string.Format("{0} {1}", DateFormat, TimeFormat));
            var localDateWithTz = string.Format("{0} ({1})", str, GetTzAbbreviation(currentDate.IsDaylightSavingTime() ? timeZone.DaylightName : timeZone.StandardName));

            return localDateWithTz;
        }

        /// <summary>
        /// Oct 14, 2014 5:42 PM (PST)
        /// <param name="timeZone">Passing an empty timezone defaults to PST</param>
        /// </summary>
        public static string ToShortDateAndTime(this DateTime? date, TimeZoneInfo timeZone = null)
        {
            return !date.HasValue ? string.Empty : date.Value.ToShortDateAndTime(timeZone);
        }

        /// <summary>
        /// Oct 14, 2014
        /// <param name="timeZone">Passing an empty timezone defaults to PST</param>
        /// </summary>
        public static string ToShortDate(this DateTime date, TimeZoneInfo timeZone = null)
        {
            return date.ConvertFromUtc(timeZone).ToString(DateFormat);
        }

        public static string ToPartialDate(this DateTime date, TimeZoneInfo timeZone = null)
        {
            return date.ConvertFromUtc(timeZone).ToString("MMM d");
        }

        public static string ToTimeOnly(this DateTime date, TimeZoneInfo timeZone = null)
        {
            return date.ConvertFromUtc(timeZone).ToString("t");
        }

        public static DateTime FromUTC(this DateTime date, TimeZoneInfo timeZone = null)
        {
            var d = date.ConvertFromUtc(timeZone);
            return d;
        }

        /// <summary>
        /// 5:42 PM
        /// </summary>
        /// <param name="timeZone">Passing an empty timezone defaults to PST</param>
        public static string ToTime(this DateTime date, TimeZoneInfo timeZone = null)
        {
            var currentDate = DateTime.UtcNow.ConvertFromUtc(timeZone);
            var localDate = date.ConvertFromUtc(timeZone);
            var str = localDate.ToString(string.Format("{0}", TimeFormat));
            var localDateWithTz = string.Format("{0} ({1})", str, GetTzAbbreviation(currentDate.IsDaylightSavingTime() ? timeZone.DaylightName : timeZone.StandardName));


            return localDateWithTz;
        }

        private static TimeZoneInfo PacificTimeZone()
        {
            return TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
        }

        public static DateTime ToPacificTime(this DateTime date)
        {
            if (date.Kind != DateTimeKind.Utc)
                date = DateTime.SpecifyKind(date, DateTimeKind.Utc);

            return TimeZoneInfo.ConvertTime(date, TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time"));
        }


        private static DateTime ConvertFromUtc(this DateTime dt, TimeZoneInfo timeZone)
        {
            // is dayliahgt savings time go to tz abbrev and pass in daylight name or std name
            if (dt.Kind == DateTimeKind.Unspecified)
                DateTime.SpecifyKind(dt, DateTimeKind.Utc);
            //Finally default to pacific time zone
            return TimeZoneInfo.ConvertTime(dt, timeZone);
        }

      

        static string GetTzAbbreviation(string timeZoneName)
        {

            //where you are converting the date and using the method
            string output = string.Empty;

            string[] timeZoneWords = timeZoneName.Split(' ');
            foreach (string timeZoneWord in timeZoneWords)
            {
                if (timeZoneWord[0] != '(')
                {
                    output += timeZoneWord[0];
                }
                else
                {
                    output += timeZoneWord;
                }
            }
            return output;
        }

        public static DateTime UnixTimeToDateTime(long unixtime)
        {
            var sTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return sTime.AddSeconds(unixtime);
        }
    }
}
