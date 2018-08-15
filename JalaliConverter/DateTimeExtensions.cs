using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JalaliConverter
{
    public static class DateTimeExtensions
    {
        public static string TimeStamp(this DateTime dateTime)
        {
            int ms = DateTime.Now.Millisecond;
            string str = DateTime.Now.Year.ToString();
            str += DateTime.Now.Month < 10 ? "0" + DateTime.Now.Month.ToString() : DateTime.Now.Month.ToString();
            str += DateTime.Now.Day < 10 ? "0" + DateTime.Now.Day.ToString() : DateTime.Now.Day.ToString();
            str += DateTime.Now.TimeOfDay.Hours < 10 ? "0" + DateTime.Now.TimeOfDay.Hours.ToString() : DateTime.Now.TimeOfDay.Hours.ToString();
            str += DateTime.Now.TimeOfDay.Minutes < 10 ? "0" + DateTime.Now.TimeOfDay.Minutes.ToString() : DateTime.Now.TimeOfDay.Minutes.ToString();
            str += DateTime.Now.TimeOfDay.Seconds < 10 ? "0" + DateTime.Now.TimeOfDay.Seconds.ToString() : DateTime.Now.TimeOfDay.Seconds.ToString();

            if (ms < 10)
                str += "000" + ms.ToString();
            else if (ms < 100)
                str += "00" + ms.ToString();
            else if (ms < 1000)
                str += "0" + ms.ToString();
            else
                str += ms.ToString();

            return str;
        }

        public static string GetTime(this DateTime dateTime)
        {
            int secound = dateTime.TimeOfDay.Seconds;
            int minute = dateTime.TimeOfDay.Minutes;
            int hour = dateTime.TimeOfDay.Hours;

            string s = secound < 10 ? "0" + secound.ToString() : secound.ToString();
            string m = minute < 10 ? "0" + minute.ToString() : minute.ToString();
            string h = hour < 10 ? "0" + hour.ToString() : hour.ToString();
            return string.Format("{0}:{1}:{2}", h, m, s);
        }

        public static string GetDateTime(this DateTime dateTime)
            => dateTime.ToString().Substring(0, 19).Replace(' ', 'T');

        public static DateTime ToGregorianDate(this string persianDate)
            => new PersianDate(persianDate).ToChristianDate();
    }
}