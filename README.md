Date Range Picker for [Blazor](https://blazor.net/)
=====================

[![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/BlazorDateRangePicker.svg)](https://www.nuget.org/packages/BlazorDateRangePicker/)

![https://github.com/jdtcn/BlazorDateRangePicker](https://habrastorage.org/webt/ku/ye/jt/kuyejt2khntesrw6asg9hvwiri0.png)

## [Live Demo](https://blazordaterangepicker.azurewebsites.net/)

این کامپوننت انتخاب محدود زمانی، پورت شده ای از کامپوننت بسیار متداول جاوااسکریپتی [DateRangePicker](https://github.com/dangrossman/daterangepicker/) می باشد که به زبان #C دوباره بازنویسی شده تا بعنوان یک کامپوننت Blazor بتوان از آن استفاده کرد.

این کامپوننت یک تقویم بصورت دراپ‌داون ایجاد می کند که کاربر می تواند محدوده ای از تاریخ ها را از آن انتخاب کند.

باتوجه به وجود کتابخانه بسیار کاربردی Persian Calendar از System.Globalization، هیچ وابستگی به jquery، moment یا bootstrap وجود ندارد.

مهمترین ویژگی های کامپوننت عبارتند از: محدود کردن محدوده تاریخ قابل انتخاب، متن‌ها و لیبل‌های قابل بومی سازی و قالب های تاریخ از پیش تعریف شده و همچنین حالت انتخابگر تاریخ و محدوده تاریخ از پیش تعریف شده.

بطور بسیار محدود از JS Interop برای موقعیت یابی popup  و مدیریت کلیک‌های خارج از محدوده کامپوننت استفاده شده و با توجه به امکانات نسخه های بعدی ASP.NET Core Blazor، نسخه بدون نیاز به js نیز امکان پذیر خواهد بود.

## اولین گام‌ها برای شروع

پروژه برنامه را از اینجا دانلود یا کلون کنید و به برنامه خود اضافه کنید.

خط های زیر را به فایل  _Host.cshtml (یا *index.html* برای Blazor WebAssembly)  در قسمت `<head></head>` اضافه کنید:

````html
<script src="_content/BlazorDateRangePicker/clickAndPositionHandler.js"></script>
<link rel="stylesheet" href="_content/BlazorDateRangePicker/daterangepicker.min.css" />
```` 

### و به این صورت از کامپوننت استفاده کنید:

````C#
@using BlazorDateRangePicker

<DateRangePicker/>
````
که در کد رندر شده به صورت زیر می آید:
````HTML
<input type="text"/>
````
### همچنین در صورت استفاده از Tag Attribute ها در تگ کامپوننت، به تگ Input اعمال خواهند شد:

````C#
@using BlazorDateRangePicker

<DateRangePicker class="form-control form-control-sm" placeholder="Select dates..." />
````
به این صورت اعمال می شود:
````HTML
<input type="text" class="form-control form-control-sm" placeholder="Select dates..."/>
````

### تنظیم خصوصیات:
````C#
@using BlazorDateRangePicker

<DateRangePicker MinDate="DateTimeOffset.Now.AddYears(-10)" MaxDate="DateTimeOffset.Now" />
````

### Two-way data binding:
````C#
@using BlazorDateRangePicker

<DateRangePicker @bind-StartDate="StartDate" @bind-EndDate="EndDate" />

@code {
    DateTimeOffset? StartDate { get; set; } = DateTime.Today.AddMonths(-1);
    DateTimeOffset? EndDate { get; set; } = DateTime.Today.AddDays(1).AddTicks(-1);
}
````

### Handle selection event:
````C#
@using BlazorDateRangePicker

<DateRangePicker OnRangeSelect="OnRangeSelect" />

@code {
    public void OnRangeSelect(DateRange range)
    {
        //Use range.Start and range.End here
    }
}
````

### استفاده کمی پیچیده تر:
می توانید از markup اختصاصی خودتان برای picker استفاده کنید.
````C#
@using BlazorDateRangePicker

<DateRangePicker Culture="@(System.Globalization.CultureInfo.GetCultureInfo("fa-IR"))">
    <PickerTemplate>
        <div id="@context.Id" @onclick="context.Toggle" style="background: #fff; cursor: pointer; padding: 5px 10px; width: 250px; border: 1px solid #ccc;">
            <i class="oi oi-calendar"></i>&nbsp;
            <span>@context.FormattedRange @(string.IsNullOrEmpty(context.FormattedRange) ? "انتخاب بازه زمانی ..." : "")</span>
            <i class="oi oi-chevron-bottom float-right"></i>
        </div>
    </PickerTemplate>
</DateRangePicker>
````
برای کنترل کلیک‌های خارج از محدوده کامپوننت، مقدار id="@context.Id" را تنظیم می کنیم. 

شخصی سازی دکمه ها:

````C#
<DateRangePicker @bind-StartDate="StartDate" @bind-EndDate="EndDate">
    <ButtonsTemplate>
        <button class="cancelBtn btn btn-sm btn-default" 
            @onclick="@context.ClickCancel" type="button">انصراف</button>
        <button class="cancelBtn btn btn-sm btn-default" 
            @onclick="@(e => ResetClick(e, context))" type="button">از نو</button>
        <button class="applyBtn btn btn-sm btn-primary" @onclick="@context.ClickApply"
            disabled="@(context.TStartDate == null || context.TEndDate == null)" 
            type="button">اعمال</button>
    </ButtonsTemplate>
</DateRangePicker>

@code {
    DateTimeOffset? StartDate { get; set; }
    DateTimeOffset? EndDate { get; set; }

    void ResetClick(MouseEventArgs e, DateRangePicker picker)
    {
        StartDate = null;
        EndDate = null;
        // Close the picker
        picker.Close();
        // Fire OnRangeSelectEvent
        picker.OnRangeSelect.InvokeAsync(new DateRange());
    }
}
````
برای اینکه وضعیت picker را قبل از اینکه کاربر بر روی دکمه 'اعمال' کلیک کند داشته باشید، از خصوصیت Picker.TStartDate و Picker.TEndDate استفاده کنید.  

### تنظیم متمرکز برای تمام جاهایی که کامپوننت نمایش داده می شود

````C#
#Startup.cs

using BlazorDateRangePicker;

//ConfigureServices
            builder.Services.AddDateRangePicker(config =>
            {
                config.Culture = PersianDateExtensionMethods.GetPersianCulture();
                config.ApplyLabel = "اعمال";
                config.CancelLabel = "انصراف";
                config.Opens = SideType.Left;
                config.CustomRangeLabel = "بازه دلخواه";
                config.AutoApply = true;
                config.AutoAdjustCalendars = true;
                config.ShowISOWeekNumbers = true;
                config.ShowDropdowns = true;
                config.FirstDayOfWeek = DayOfWeek.Saturday;
                config.ShowCustomRangeLabel = true;
                config.Prerender = true;
                config.Ranges = new Dictionary<string, DateRange> {
                        { "امروز", new DateRange
                            {
                                Start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                                End = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)
                            }
                        } ,
                        { "دیروز", new DateRange
                            {
                                Start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(-1),
                                End = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(-1)
                            }
                        } ,
                        { "هفت روز گذشته", new DateRange
                            {
                                Start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(-6),
                                End = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)
                            }
                        } ,
                        { "سی روز گذشته", new DateRange
                            {
                                Start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddMonths(-1),
                                End = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)
                            }
                        } ,
                        { "این ماه", new DateRange
                            {
                                Start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1),
                                End = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddTicks(-1)
                            }
                        } ,
                        { "ماه گذشته" , new DateRange
                            {
                                Start = new DateTime(DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month, 1),
                                End = new DateTime(DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month, 1).AddMonths(1).AddTicks(-1)
                            }
                        } ,
                        { "شش ماه گذشته" , new DateRange
                            {
                                Start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddMonths(-6),
                                End = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)
                            }
                        } ,
                        { "یک سال گذشته" , new DateRange
                            {
                                Start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddYears(-1),
                                End = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)
                            }
                        },
                        { "امسال" , new DateRange
                            {
                                Start = new DateTime(DateTime.Now.Year, 1, 1),
                                End = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)
                            }
                        }
                 };
            });
````
همچنین می توانید چندین تنظیم متفاوت با نام‌های متفاوت ایجاد کنید و به picker با خصوصیت "Config" اعمال کنید. 

````C#
services.AddDateRangePicker(config => ..., configName: "CustomConfig");

<DateRangePicker Config="CustomConfig" />
````

## خصوصیات

| نام | نوع | مقدار پیش‌فرض |  توضیحات |
|------|------|--------------|--------------|
|StartDate|DateTimeOffset?|null|The beginning date of the initially selected date range.|
|EndDate|DateTimeOffset?|null|The end date of the initially selected date range.|
|MinDate|DateTimeOffset?|null|The earliest date a user may select.|
|MaxDate|DateTimeOffset?|null|The latest date a user may select.|
|MinSpan|TimeSpan?|null|The minimum span between the selected start and end dates.|
|MaxSpan|TimeSpan?|null|The maximum span between the selected start and end dates.|
|ShowDropdowns|bool|true|Show year and month select boxes above calendars to jump to a specific month and year.|
|ShowWeekNumbers|bool|false|Show localized week numbers at the start of each week on the calendars.|
|ShowISOWeekNumbers|bool|false|Show ISO week numbers at the start of each week on the calendars.|
|Ranges|Dictionary<string, DateRange>|null|Set predefined date ranges the user can select from. Each key is the label for the range.|
|ShowCustomRangeLabel|bool|true|Displays "Custom Range" at the end of the list of predefined ranges, when the ranges option is used. This option will be highlighted whenever the current date range selection does not match one of the predefined ranges. Clicking it will display the calendars to select a new range.|
|AlwaysShowCalendars|bool|false|Normally, if you use the ranges option to specify pre-defined date ranges, calendars for choosing a custom date range are not shown until the user clicks "Custom Range". When this option is set to true, the calendars for choosing a custom date range are always shown instead.|
|Opens|SideType enum: Left/Right/Center|Right|Whether the picker appears aligned to the left, to the right, or centered under the HTML element it's attached to.|
|Drops|DropsType enum: Down/Up|Down|Whether the picker appears below (default) or above the HTML element it's attached to.|
|ButtonClasses|string|btn btn-sm|CSS class names that will be added to both the apply and cancel buttons.|
|ApplyButtonClasses|string|btn-primary|CSS class names that will be added only to the apply button.|
|CancelButtonClasses|string|btn-default|CSS class names that will be added only to the cancel button.|
|Culture|CultureInfo|CultureInfo.CurrentCulture|Allows you to provide localized strings for buttons and labels, customize the date format, and change the first day of week for the calendars.|
|SingleDatePicker|bool|false|Show only a single calendar to choose one date, instead of a range picker with two calendars. The start and end dates provided to your callback will be the same single date chosen.|
|AutoApply|bool|false|Hide the apply and cancel buttons, and automatically apply a new date range as soon as two dates are clicked.|
|LinkedCalendars|bool|false|When enabled, the two calendars displayed will always be for two sequential months (i.e. January and February), and both will be advanced when clicking the left or right arrows above the calendars. When disabled, the two calendars can be individually advanced and display any month/year.|
|DaysEnabledFunction|Func<DateTimeOffset, bool>|_ => true|A function that is passed each date in the two calendars before they are displayed, and may return true or false to indicate whether that date should be available for selection or not.|
|DaysEnabledFunctionAsync|Func< DateTimeOffset, Task< bool>>|_ => true|Same as DaysEnabledFunction but with async support.|
|CustomDateFunction|Func<DateTimeOffset, object>|_ => true|A function to which each date from the calendars is passed before they are displayed, may return a bool value indicates whether the string will be added to the cell, or a string with CSS class name to add to that date's calendar cell. May return string, bool, Task<string>, Task<bool>|
|CustomDateClass|string|string.Empty|String of CSS class name to apply to that custom date's calendar cell.|
|ApplyLabel|string|"Apply"|Apply button text.|
|CancelLabel|string|"Cancel"|Cancel button text.|
|CustomRangeLabel|string|"Custom range"|Custom range label at the end of the list of predefined ranges.|
|DateFormat|string|CultureInfo.DateTimeFormat.ShortDatePattern|Enforces the desired format for formatting the date, ignoring the settings of the current CultureInfo.|
|Config|string|null|Name of the named configuration to use with this picker instance.|
|ShowOnlyOneCalendar|bool|false|Show only one calendar in the picker instead of two calendars.|
|CloseOnOutsideClick|bool|true|Whether the picker should close on outside click.|
|AutoAdjustCalendars|bool|true|Whether the picker should pick the months based on selected range.|
|PickerTemplate|RenderFragment<DateRangePicker>|null|Custom input field template|
|ButtonsTemplate|RenderFragment<DateRangePicker>|null|Custom picker buttons template|
|DayTemplate|RenderFragment<CalendarItem>|null|Custom day cell template|
|Inline|bool|false|Inline mode if true.|
|ResetOnClear|bool|true|Whether the picker should set dates to null when the user clears the input.|
|TimePicker|bool|false|Adds select boxes to choose times in addition to dates.|
|TimePicker24Hour|bool|true|Use 24-hour instead of 12-hour times, removing the AM/PM selection.|
|TimePickerIncrement|int|1|Increment of the minutes selection list for times (i.e. 30 to allow only selection of times ending in 0 or 30).|
|TimePickerSeconds|bool|false|Show seconds in the timePicker.|
|InitialStartTime|TimeSpan|TimeSpan.Zero|Initial start time value to show in the picker before any date selected|
|InitialEndTime|TimeSpan|TimeSpan.FromDays(1).AddTicks(-1)|Initial end time value to show in the picker before any date selected|
|TimeEnabledFunction|Func<DateTimeOffset?, Task<TimeSettings>>|null|Returns time available for selection.|

## رخدادها

| Name | Type | Description |
|------|------|-------------|
|OnRangeSelect|DateRange|Triggered when the apply button is clicked, or when a predefined range is clicked.|
|OnOpened|void|An event that is invoked when the DatePicker is opened.|
|OnClosed|void|An event that is invoked when the DatePicker is closed.|
|OnCancel|bool|An event that is invoked when user cancels the selection (`true` if by pressing "Cancel" button, `false` if by backdrop click).|
|OnReset|void|An event that is invoked when the DatePicker is cleared.|
|OnMonthChanged|void|An event that is invoked when left or right calendar's month changed.|
|OnMonthChangedAsync|Task|An event that is invoked when left or right calendar's month changed and supports CancellationToken. Use this event handler to prepare the data for CustomDateFunction.|
|OnSelectionStart|DateTimeOffset|An event that is invoked when StartDate is selected|
|OnSelectionEnd|DateTimeOffset|An event that is invoked when EndDate is selected but before "Apply" button is clicked|

## متدها

| Name |Description |
|------|------------|
|Open|Show picker popup.|
|Close|Close picker popup.|
|Toggle|Toggle picker popup state.|
|Reset|Rest picker.|
|virtual InvokeClickOutside|A JSInvocable callback to handle outside click. When inherited can be overridden to modify outside click closing behavior.|

## تایپ‌ها

DateRange:
````C#
public class DateRange
{
    public DateTimeOffset Start { get; set; }
    public DateTimeOffset End { get; set; }
}
````

>نکته: 
>DateRange Start و End براساس منطقه زمانی محلی می‌باشد 
>
>خصوصیت Start شروع روز انتخاب شده است (dateTime.Date).
>
>خصوصیت End پایان روز انتخاب شده است (dateTime.Date.AddDays(1).AddTicks(-1)).

## Changelog
### 0.0.0

1. اعمال تغییرات اولیه در کامپوننت اصلی
2. ایجاد یک نسخه اولیه قابل استفاده

 
## License

The MIT License (MIT)

Copyright (c) 2019-2020 Sergey Zaikin

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
