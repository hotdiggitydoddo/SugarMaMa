using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NodaTime;
using NodaTime.TimeZones;

namespace SugarMaMa.API.Helpers
{
    public static class DateTimeExtensions
    {
        static readonly IDateTimeZoneProvider tzSource = new DateTimeZoneCache(TzdbDateTimeZoneSource.Default);
        public static DateTime ConvertToLocalTime(this DateTime utcDateTime, string timeZoneId)
        {
            DateTime dUtc;
            switch (utcDateTime.Kind)
            {
                case DateTimeKind.Utc:
                    dUtc = utcDateTime;
                    break;
                case DateTimeKind.Local:
                    dUtc = utcDateTime.ToUniversalTime();
                    break;
                default: //DateTimeKind.Unspecified
                    dUtc = DateTime.SpecifyKind(utcDateTime, DateTimeKind.Utc);
                    break;
            }

            var timeZone = tzSource.GetZoneOrNull(timeZoneId);

            var instant = Instant.FromDateTimeUtc(dUtc);
            var zoned = new ZonedDateTime(instant, timeZone);
            return new DateTime(
                zoned.Year,
                zoned.Month,
                zoned.Day,
                zoned.Hour,
                zoned.Minute,
                zoned.Second,
                zoned.Millisecond,
                DateTimeKind.Unspecified);
        }
    }











    public interface ITimeZoneHelper
    {
        DateTime ConvertToLocalTime(DateTime utcDateTime, string timeZoneId);

        DateTime ConvertToUtc(
            DateTime localDateTime,
            string timeZoneId,
            ZoneLocalMappingResolver resolver = null
        );

        IReadOnlyCollection<string> GetTimeZoneList();
    }

    public class TimeZoneHelper : ITimeZoneHelper
    {
        public TimeZoneHelper(
            IDateTimeZoneProvider timeZoneProvider,
            ILogger<TimeZoneHelper> logger = null
            )
        {
            tzSource = timeZoneProvider;
            log = logger;
        }

        private IDateTimeZoneProvider tzSource;
        private ILogger log;

        public DateTime ConvertToLocalTime(DateTime utcDateTime, string timeZoneId)
        {
            DateTime dUtc;
            switch (utcDateTime.Kind)
            {
                case DateTimeKind.Utc:
                    dUtc = utcDateTime;
                    break;
                case DateTimeKind.Local:
                    dUtc = utcDateTime.ToUniversalTime();
                    break;
                default: //DateTimeKind.Unspecified
                    dUtc = DateTime.SpecifyKind(utcDateTime, DateTimeKind.Utc);
                    break;
            }

            var timeZone = tzSource.GetZoneOrNull(timeZoneId);
            if (timeZone == null)
            {
                if (log != null)
                {
                    log.LogWarning("failed to find timezone for " + timeZoneId);
                }

                return utcDateTime;
            }

            var instant = Instant.FromDateTimeUtc(dUtc);
            var zoned = new ZonedDateTime(instant, timeZone);
            return new DateTime(
                zoned.Year,
                zoned.Month,
                zoned.Day,
                zoned.Hour,
                zoned.Minute,
                zoned.Second,
                zoned.Millisecond,
                DateTimeKind.Unspecified);
        }

        public DateTime ConvertToUtc(
            DateTime localDateTime,
            string timeZoneId,
            ZoneLocalMappingResolver resolver = null
            )
        {
            if (localDateTime.Kind == DateTimeKind.Utc) return localDateTime;

            if (resolver == null) resolver = Resolvers.LenientResolver;
            var timeZone = tzSource.GetZoneOrNull(timeZoneId);
            if (timeZone == null)
            {
                if (log != null)
                {
                    log.LogWarning("failed to find timezone for " + timeZoneId);
                }
                return localDateTime;
            }

            var local = LocalDateTime.FromDateTime(localDateTime);
            var zoned = timeZone.ResolveLocal(local, resolver);
            return zoned.ToDateTimeUtc();
        }

        public IReadOnlyCollection<string> GetTimeZoneList()
        {
            return tzSource.Ids;
        }
    }
}
