using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JalaliConverter
{
    public enum PersainMonthName : byte
    {
        فروردین = 1,
        اردیبهشت = 2,
        خرداد = 3,
        تیر = 4,
        مرداد = 5,
        شهریور = 6,
        مهر = 7,
        آبان = 8,
        آذر = 9,
        دی = 10,
        بهمن = 11,
        اسفند = 12
    }

    public class PersianDate : IComparable<PersianDate>, IComparable
    {
        private static PersianCalendar pc = new PersianCalendar();
        private string[] dayNames = new string[] { "یکم", "دوم", "سوم", "چهارم", "پنجم", "ششم", "هفتم", "هشتم", "نهم", "دهم", "یازدهم", "دوازدهم", "سیزدهم", "چهاردهم", "پانزدهم", "شانزدهم", "هفدهم", "هجدهم", "نوزدهم", "بیستم", "بیست و یکم", "بیست و دوم", "بیست و سوم", "بیست و چهارم", "بیست و پنجم", "بیست و شمم", "بیست و هفتم", "بیست و هشتم", "بیست و نهم", "سی ام", "سی و یکم" };
        private string[] weekDayNames = new string[] { "شنبه", "یکشنبه", "دوشنبه", "سه شنبه", "چهارشنبه", "پنجشنبه", "جمعه" };

        #region constructors
        public PersianDate()
            : this(pc.GetYear(DateTime.Today), pc.GetMonth(DateTime.Today), pc.GetDayOfMonth(DateTime.Today))
        {

        }
        public PersianDate(DateTime christianDate)
            : this(pc.GetYear(christianDate), pc.GetMonth(christianDate), pc.GetDayOfMonth(christianDate))
        {
        }
        public PersianDate(DateTime? christianDate)
        {
            try
            {
                this.Year = pc.GetYear((DateTime)christianDate);
                this.Month = pc.GetMonth((DateTime)christianDate);
                this.Day = pc.GetDayOfMonth((DateTime)christianDate);
            }
            catch
            {

            }
        }
        public PersianDate(int year, int month, int day)
        {
            this.Year = year;
            this.Month = month;
            this.Day = day;
        }

        public PersianDate(string persianDate)
        {
            if (string.IsNullOrEmpty(persianDate))
                throw new Exception("Persian date string is invalid. Persian date string is empty or null.");

            if (persianDate.Length != 10)
                throw new Exception("Persian date string is invalid. Value of persian date string is invalid");

            if (persianDate[4] != persianDate[7])
                throw new Exception("Persian date string is invalid. Date segment seperator is invalid.");

            if (persianDate[4] != '/' && persianDate[4] != '-')
                throw new Exception("Persian date string is invalid. Date segment seperator most be slash or dash.");

            string[] segments = persianDate.Split(persianDate[4]);
            if (segments.Length != 3)
                throw new Exception("Persian date string is invalid. Value of persian date string is invalid");

            try
            {
                int y = Convert.ToInt32(segments[0]);
                int m = Convert.ToInt32(segments[1]);
                int d = Convert.ToInt32(segments[2]);

                if (y < 1000 || y > 9999)
                    throw new Exception("Persian date string is invalid. Value of year is invalid");

                if (m < 1 || m > 12)
                    throw new Exception("Persian date string is invalid. Value of maonth is invalid");

                if (d < 1 || (m < 7 && d > 31))
                    throw new Exception("Persian date string is invalid. Value of day is invalid");

                if (d < 1 || (m > 6 && d > 30))
                    throw new Exception("Persian date string is invalid. Value of day is invalid");

                this.Day = d;
                this.Month = m;
                this.Year = y;

            }
            catch (Exception e)
            {

                throw e;
            }
        }
        #endregion

        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }

        public string ToString(DateStringType stringType = DateStringType.Digit)
        {
            switch (stringType)
            {
                case DateStringType.SubDateString:
                    var dayStr = "";

                    if (Year > 0)
                        dayStr += $"{Year} سال";

                    if (Month > 0)
                    {
                        if (dayStr.Length > 0)
                            dayStr += $" و";
                        dayStr += $" {Month} ماه";
                    }

                    if (Day > 0)
                    {
                        if (dayStr.Length > 0)
                            dayStr += $" و";
                        dayStr += $" {Day} روز";
                    }
                    return dayStr;
                case DateStringType.String:
                    return "";
                case DateStringType.Digit:
                    string y = Year.ToString();
                    string m = Month > 9 ? Month.ToString() : "0" + Month.ToString();
                    string d = Day > 9 ? Day.ToString() : "0" + Day.ToString();
                    if (Year < 1 || Month < 1 || Day < 1)
                        return string.Empty;
                    else
                        return $"{y}/{m}/{d}";
            }
            return ToString();
        }

        public PersainMonthName MonthName
        {
            get { return (PersainMonthName)Month; }
        }
        public string DayName
        {
            get { return dayNames[Day - 1]; }
        }
        public string WeekDayName
        {
            get
            {
                int d = 0;
                if (ToChristianDate().DayOfWeek == DayOfWeek.Saturday)
                    d = 0;
                else
                {
                    d = (int)ToChristianDate().DayOfWeek;
                    d++;
                }

                return weekDayNames[d];
            }
        }

        public bool FormatIsValid(string persianDate)
        {
            if (string.IsNullOrEmpty(persianDate))
                return false;

            if (persianDate.Length != 10)
                return false;

            if (persianDate[4] != persianDate[7])
                return false;

            if (persianDate[4] != '/' && persianDate[4] != '-')
                return false;

            string[] segments = persianDate.Split(persianDate[4]);
            if (segments.Length != 3)
                return false;

            foreach (char c in persianDate)
            {
                if (!"-/0123456789".ToCharArray().Any(Ci => Ci == c))
                    return false;
            }

            int y = Convert.ToInt32(segments[0]);
            int m = Convert.ToInt32(segments[1]);
            int d = Convert.ToInt32(segments[2]);

            if (y < 1000 || y > 9999)
                return false;

            if (m < 1 || m > 12)
                return false;

            if (d < 1 || (m < 7 && d > 31))
                return false;

            if (d < 1 || (m > 6 && d > 30))
                return false;

            return true;
        }

        public DateTime ToChristianDate()
        {
            return pc.ToDateTime(Year, Month, Day, DateTime.Today.Hour, DateTime.Today.Minute, DateTime.Today.Second, DateTime.Today.Millisecond);
        }
        public PersianDate FromString(string persianDate)
        {
            if (!FormatIsValid(persianDate))
                return null;

            string[] segments = persianDate.Split(persianDate[4]);

            int y = Int32.Parse(segments[0]);
            int m = Int32.Parse(segments[1]);
            int d = Int32.Parse(segments[2]);
            if (m < 1 || m > 12)
                return null;
            if (d < 1)
                return null;
            if (m > 6 && d > 30)
                return null;
            else if (m < 7 && d > 31)
                return null;

            return new PersianDate(y, m, d);
        }

        public PersianDate Tomorrow()
        {
            return new PersianDate(ToChristianDate().AddDays(1));
        }
        public PersianDate Yesterday()
        {
            return new PersianDate(ToChristianDate().AddDays(-1));
        }
        public PersianDate NextMonth()
        {
            return new PersianDate(ToChristianDate().AddMonths(1));
        }
        public PersianDate PriorMonth()
        {
            return new PersianDate(ToChristianDate().AddMonths(-1));
        }
        public PersianDate NextYear()
        {
            return new PersianDate(ToChristianDate().AddYears(1));
        }
        public PersianDate PriorYear()
        {
            return new PersianDate(ToChristianDate().AddYears(-1));
        }
        public PersianDate AddDays(int n)
        {
            return new PersianDate(ToChristianDate().AddDays(n));
        }
        public PersianDate AddMonths(int n)
        {
            return new PersianDate(ToChristianDate().AddMonths(n));
        }
        public PersianDate AddYears(int n)
        {
            return new PersianDate(ToChristianDate().AddYears(n));
        }

        public static explicit operator PersianDate(DateTime dateTime)
            => new PersianDate(dateTime);

        public static PersianDate operator +(PersianDate endDate, PersianDate startDate)
        {
            var yearsCount = endDate.Year + startDate.Year;
            var monthsCount = endDate.Month + startDate.Month;
            var daysCount = endDate.Day + startDate.Day;

            if (daysCount > 30)
            {
                daysCount -= 30;
                monthsCount++;
            }

            if (daysCount > 0)
                daysCount++;

            if (monthsCount > 12)
            {
                monthsCount -= 12;
                yearsCount++;
            }

            if (daysCount == 30 || daysCount == 31)
            {
                daysCount = 0;
                monthsCount++;
            }

            if (monthsCount == 12)
            {
                monthsCount = 0;
                yearsCount++;
            }

            return new PersianDate(yearsCount, monthsCount, daysCount);
        }

        public static PersianDate operator -(PersianDate endDate, PersianDate startDate)
        {
            var yearsCount = endDate.Year - startDate.Year;
            var monthsCount = endDate.Month - startDate.Month;
            var daysCount = endDate.Day - startDate.Day;

            if (daysCount < 0)
            {
                daysCount += 30;
                monthsCount--;
            }

            if (daysCount > 0)
                daysCount++;

            if (monthsCount < 0)
            {
                monthsCount += 12;
                yearsCount--;
            }

            if (daysCount == 30 || daysCount == 31)
            {
                daysCount = 0;
                monthsCount++;
            }

            if (monthsCount == 12)
            {
                monthsCount = 0;
                yearsCount++;
            }

            return new PersianDate(yearsCount, monthsCount, daysCount);
        }

        #region IComparable<PersianDate> Members
        /// <summary>
        /// Compare value whit other date.
        /// </summary>
        /// <param name="date">Value to compare with control value.</param>
        /// <returns></returns>
        public int CompareTo(PersianDate other)
        {
            return DateTime.Compare(ToChristianDate(), other.ToChristianDate());
        }
        public int CompareTo(object obj)
        {
            PersianDate d = (PersianDate)obj;
            return this.CompareTo(d);
        }
        #endregion
    }
}
