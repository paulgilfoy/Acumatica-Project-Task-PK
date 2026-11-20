using System;

namespace ProjectTask
{
    /// <summary>
    /// T-32: due date of a task copied from a project template or a common task (PKProjectTemplateExt calls GetDueDate).
    /// Pure date logic: NO Acumatica references, so tools\Test-TemplateDates.ps1 compiles and runs this file on its own.
    ///
    /// Rule in use (docs/template-copy.md, option C of docs/template-flow.md section 6): template due dates are statutory
    /// calendar dates (e.g. 3/15, 4/15, 5/15), so keep the template date's MONTH and DAY and return its first occurrence
    /// ON OR AFTER the project's start date. The template's year is ignored.
    ///  - Project start date missing: use the fallback date (the caller passes the business date); if that is missing too,
    ///    copy the template date as is.
    ///  - Template date missing: null (nothing is copied).
    ///  - Template date 29 Feb: 29 Feb in a leap year, 28 Feb (last day of February) otherwise; never 1 Mar.
    ///  - Time of day is dropped (the field is a date).
    /// To switch rules change ActiveRule (one line); see docs/template-copy.md "Switching the due-date rule".
    /// </summary>
    public static class PKTemplateDueDate
    {
        public enum DueDateRule
        {
            /// <summary>Copy the template / common task date unchanged (option A).</summary>
            AsIs = 0,
            /// <summary>Same month/day, first occurrence on or after the project start (option C, default).</summary>
            RollForward = 1
        }

        /// <summary>The rule PKProjectTemplateExt uses.</summary>
        public const DueDateRule ActiveRule = DueDateRule.RollForward;

        /// <summary>Due date for a new task, using <see cref="ActiveRule"/>.</summary>
        /// <param name="templateDate">UsrPGDueDate of the template task / common task.</param>
        /// <param name="projectStart">Start date of the project the task is added to (PMProject.StartDate).</param>
        /// <param name="fallbackStart">Used when <paramref name="projectStart"/> is null (the business date).</param>
        public static DateTime? GetDueDate(DateTime? templateDate, DateTime? projectStart, DateTime? fallbackStart)
        {
            return GetDueDate(ActiveRule, templateDate, projectStart, fallbackStart);
        }

        /// <summary>Due date for a new task under an explicit rule (used by the test harness).</summary>
        public static DateTime? GetDueDate(DueDateRule rule, DateTime? templateDate, DateTime? projectStart, DateTime? fallbackStart)
        {
            if (templateDate == null) return null;
            if (rule == DueDateRule.AsIs) return templateDate.Value.Date;

            DateTime? anchor = projectStart ?? fallbackStart;
            if (anchor == null) return templateDate.Value.Date;
            return RollForward(templateDate.Value, anchor.Value);
        }

        /// <summary>
        /// First date on or after <paramref name="anchor"/> that has the month and day of <paramref name="templateDate"/>
        /// (29 Feb becomes 28 Feb in a non-leap year).
        /// </summary>
        public static DateTime RollForward(DateTime templateDate, DateTime anchor)
        {
            DateTime start = anchor.Date;
            DateTime candidate = OnMonthDay(start.Year, templateDate.Month, templateDate.Day);
            if (candidate >= start) return candidate;
            if (start.Year >= DateTime.MaxValue.Year) return templateDate.Date;   // no next year to roll into
            return OnMonthDay(start.Year + 1, templateDate.Month, templateDate.Day);
        }

        private static DateTime OnMonthDay(int year, int month, int day)
        {
            return new DateTime(year, month, Math.Min(day, DateTime.DaysInMonth(year, month)));
        }
    }
}
