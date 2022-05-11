using System;
using System.Globalization;
using System.Reflection;

namespace BlazorDateRangePicker
{
    internal static class PersianDateExtensionMethods
    {
        private static CultureInfo _Culture;
        public static CultureInfo GetPersianCulture()
        {
            if (_Culture == null)
            {
                _Culture = new CultureInfo("fa-IR");
                DateTimeFormatInfo formatInfo = _Culture.DateTimeFormat;
                formatInfo.AbbreviatedDayNames = new[] { "ی", "د", "س", "چ", "پ", "ج", "ش" };
                formatInfo.DayNames = new[] { "یک‌شنبه", "دوشنبه", "سه‌شنبه", "چهارشنبه", "پنج‌شنبه", "جمعه", "شنبه" };
                var monthNames = new[]
                {
                    "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن",
                    "اسفند",
                    ""
                };                
                formatInfo.ShortestDayNames = new[] { "ی", "د", "س", "چ", "پ", "ج", "ش" };

                formatInfo.AbbreviatedMonthNames =
                    formatInfo.MonthNames =
                    formatInfo.MonthGenitiveNames = formatInfo.AbbreviatedMonthGenitiveNames = monthNames;
                formatInfo.AMDesignator = "ق.ظ";
                formatInfo.PMDesignator = "ب.ظ";
                formatInfo.ShortDatePattern = "yyyy/MM/dd";
                formatInfo.LongDatePattern = "dddd, dd MMMM,yyyy";
                formatInfo.FirstDayOfWeek = DayOfWeek.Saturday;
                System.Globalization.Calendar cal = new PersianCalendar();

                FieldInfo fieldInfo = _Culture.GetType().GetField("calendar", BindingFlags.NonPublic | BindingFlags.Instance);
                if (fieldInfo != null)
                    fieldInfo.SetValue(_Culture, cal);

                FieldInfo info = formatInfo.GetType().GetField("calendar", BindingFlags.NonPublic | BindingFlags.Instance);
                if (info != null)
                    info.SetValue(formatInfo, cal);

                _Culture.NumberFormat.NumberDecimalSeparator = "/";
                _Culture.NumberFormat.DigitSubstitution = DigitShapes.NativeNational;
                _Culture.NumberFormat.NumberNegativePattern = 0;
            }
            return _Culture;
        }

        public static string ToPersianDateString(this DateTime date, string format = "yyyy/MM/dd")
        {
            return date.ToString(format, GetPersianCulture());
        }

        public static int MonthInPersianCalendar(this DateTimeOffset date)
        {
            PersianCalendar ps = new();
            return ps.GetMonth(date.DateTime);
        }
        public static int YearInPersianCalendar(this DateTimeOffset date)
        {
            PersianCalendar ps = new();
            int month = ps.GetYear(date.DateTime);
            return month;
        }
        public static int DaysInMonthInPersianCalendar(this DateTimeOffset date)
        {
            PersianCalendar ps = new();
            return ps.GetDaysInMonth(ps.GetYear(date.DateTime), ps.GetMonth(date.DateTime), PersianCalendar.PersianEra);
        }
        public static int DayInPersianCalendar(this DateTimeOffset date)
        {
            PersianCalendar ps = new();
            return ps.GetDayOfMonth(date.DateTime);
        }
        public static int DaysInLastMonthInPersianCalendar(this DateTimeOffset date, int year, int month)
        {
            PersianCalendar ps = new();
            return ps.GetDaysInMonth(year, month, PersianCalendar.PersianEra);
        }

        public static DateTimeOffset FirstDayOfMonthInPersianCalendar(this DateTimeOffset date)
        {
            PersianCalendar ps = new();
            return ps.ToDateTime(ps.GetYear(date.DateTime), ps.GetMonth(date.DateTime), 1, 12, 0, 0, 0, PersianCalendar.PersianEra);
        }
        public static DateTimeOffset LastDayOfMonthInPersianCalendar(this DateTimeOffset date)
        {
            PersianCalendar ps = new();
            return ps.ToDateTime(ps.GetYear(date.DateTime), ps.GetMonth(date.DateTime), date.DaysInMonthInPersianCalendar(), 12, 0, 0, 0, PersianCalendar.PersianEra);
        }
        public static DateTime ToDateTimeInPersianCalendar(this DateTimeOffset date, int year, int month, int day)
        {
            PersianCalendar ps = new();
            return ps.ToDateTime(year, month, day, 12, 0, 0, 0, PersianCalendar.PersianEra);
        }
        public static DateTimeOffset AddMonthsInPersianCalendar(this DateTimeOffset date, int count)
        {
            PersianCalendar ps = new();
            return ps.AddMonths(date.DateTime, count); 
        }
    }
}