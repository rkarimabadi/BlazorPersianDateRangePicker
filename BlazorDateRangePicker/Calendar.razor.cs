﻿/**
* @author: Sergey Zaikin zaikinsr@yandex.ru
* @copyright: Copyright (c) 2019 Sergey Zaikin. All rights reserved.
* @license: Licensed under the MIT license. See http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Linq;
using System.Globalization;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace BlazorDateRangePicker
{
    public partial class Calendar : ComponentBase
    {

        [CascadingParameter] public DateRangePicker Picker { get; set; }
        [Parameter] public CalendarType CalendarData { get; set; }

        [Parameter] public SideType? Side { get; set; }
        [Parameter] public EventCallback<DateTimeOffset> OnMonthChanged { get; set; }
        [Parameter] public EventCallback<DateTimeOffset> OnClickDate { get; set; }
        [Parameter] public EventCallback<DateTimeOffset> OnHoverDate { get; set; }

        private int MinYear => Picker.MinDate?.YearInPersianCalendar() ?? 1350;
        private int MaxYear => Picker.MaxDate?.YearInPersianCalendar() ?? ((DateTimeOffset)DateTime.Now.AddYears(50)).YearInPersianCalendar();

        private int CurrentMonth
        {
            get => CalendarData.Month.MonthInPersianCalendar();
            set => MonthSelected(value);
        }

        private int CurrentYear
        {
            get => CalendarData.Month.YearInPersianCalendar();
            set => YearSelected(value);
        }

        private bool PrevBtnVisible =>
            (!Picker.MinDate.HasValue || Picker.MinDate < CalendarData.FirstDay)
            && (Picker.LinkedCalendars != true || Side == SideType.Left);
        private bool NextBtnVisible =>
            (!Picker.MaxDate.HasValue || Picker.MaxDate > CalendarData.LastDay)
            && (Picker.LinkedCalendars != true || Side == SideType.Right || Picker.SingleDatePicker == true);

        private List<string> DayNames { get; set; } = new List<string>();

        private List<string> GetDayNames()
        {
            var dayNames = Picker.Culture.DateTimeFormat.ShortestDayNames.ToList();
            var firstDayNumber = (int)Picker.FirstDayOfWeek;
            if (firstDayNumber > 0)
            {
                for (int i = 0; i < firstDayNumber; i++)
                {
                    var item = dayNames[0];
                    dayNames.Insert(dayNames.Count, item);
                    dayNames.RemoveAt(0);
                }
            }
            return dayNames;
        }

        protected override void OnInitialized()
        {
            DayNames = GetDayNames();
        }

        private Task PreviousMonth()
        {
            return OnMonthChanged.InvokeAsync(CalendarData.Month.AddMonthsInPersianCalendar(-1));
        }

        private Task NextMonth()
        {
            return OnMonthChanged.InvokeAsync(CalendarData.Month.AddMonthsInPersianCalendar(1));
        }

        private Task MonthSelected(int month)
        {
            var d = CalendarData.Month;
            return OnMonthChanged.InvokeAsync(d.ToDateTimeInPersianCalendar(d.YearInPersianCalendar(), month, 1));
        }

        private Task YearSelected(int year)
        {
            var d = CalendarData.Month;
            var newMonth = new DateTimeOffset(d.ToDateTimeInPersianCalendar(year, d.MonthInPersianCalendar(), 1), d.Offset);
            if (newMonth > Picker.MaxDate) newMonth = Picker.MaxDate.Value;
            else if (newMonth < Picker.MinDate) newMonth = Picker.MinDate.Value;
            return OnMonthChanged.InvokeAsync(newMonth);
        }

        private Task ClickDate(CalendarItem dt)
        {
            if (dt.Disabled) return Task.CompletedTask;
            return OnClickDate.InvokeAsync(dt.Day);
        }

        private Task OnMouseOverDate(DateTimeOffset date)
        {
            if (Picker.HoverDate != date)
            {
                return OnHoverDate.InvokeAsync(date);
            }
            return Task.CompletedTask;
        }

        private int GetWeekOfYear(DateTime time)
        {
            var weekRule = Picker.Culture.DateTimeFormat.CalendarWeekRule;
            var firstDayOfWeek = CalendarData.FirstDayOfWeek;
            return DateTimeFormatInfo.CurrentInfo.Calendar.GetWeekOfYear(time, weekRule, firstDayOfWeek);
        }

        private int GetIso8601WeekOfYear(DateTime time)
        {
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }
    }
}
