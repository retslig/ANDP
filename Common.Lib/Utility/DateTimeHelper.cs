using System;
using Common.Lib.Common.CustomDateTime;
using Common.Lib.Extensions;

namespace Common.Lib.Utility
{
    public static class DateTimeHelper
    {
        public static DateTimeOffset ConvertLocalTimeToClientLocalTime(this DateTimeOffset localDateTime, TimeSpan offset)
        {
            //Get to UTC time then wipe out old offset and replace with new one.
            DateTimeOffset dtf = new DateTimeOffset(DateTime.SpecifyKind(localDateTime.DateTime, DateTimeKind.Unspecified), offset);
            return dtf;
        }

        public static DateTimeOffset UpdateTimeofDay(this DateTimeOffset localDateTime, TimeSpan offset)
        {
            //Get to UTC time then wipe out old offset and replace with new one.
            DateTimeOffset dtf = new DateTimeOffset(DateTime.SpecifyKind(localDateTime.DateTime, DateTimeKind.Unspecified), offset);
            return dtf;
        }

        public static DateRange RetrieveWeekStartEnd(DateTime dt, DayOfWeek dayOfWeek)
        {
            var dateRange = new DateRange();

            dateRange.StartDate = dt.StartOfWeek(dayOfWeek);
            dateRange.EndDate = dateRange.StartDate.AddDays(6);

            return dateRange;
        }
    }
}
