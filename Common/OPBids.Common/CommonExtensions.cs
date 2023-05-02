using System;
using System.Collections.Generic;
using System.Linq;

namespace OPBids.Common
{
    public static class CommonExtensions
    {
        public static string AppendTimeStamp(this string val)
        {
            return string.Concat(DateTime.Now.ToUniversalTime().Ticks.ToString("X"), "_", val);
        }
        public static bool IsNullOrEmpty(this string obj)
        {
            return obj.ToSafeString() == "";
        }
        public static int GetPageCount(this int obj)
        {
            var pageItemCount = Constant.AppSettings.PageItemCount.ToSafeDecimal();
            var rslts = obj.ToSafeDecimal() / pageItemCount;
            if (obj.ToSafeDecimal() % pageItemCount != 0 || rslts == 0)
            {
                rslts+=1;
            }
            return (int)rslts;
        }
        public static decimal ToSafeDecimal(this object obj)
        {
            if (obj == null)
            {
                return 0;
            }
            decimal results = 0;
            decimal.TryParse(obj.ToSafeString().Replace(",", ""), out results);
            return results;
        }
        public static int ToSafeInt(this object obj)
        {
            if (obj == null)
            {
                return 0;
            }
            int results = 0;
            int.TryParse(obj.ToSafeString().Split(new char[] { '.' })[0], out results);
            return results;
        }
        public static DateTime ToDate(this object obj)
        {
            DateTime results;
            var isSuccess = DateTime.TryParse(obj.ToSafeString(), out results);
            if (isSuccess == true)
            {
                return results;
            }
            else
            {
                return DateTime.Now;
            }
        }
        public static DateTime? ToSafeDate(this object obj)
        {
            if (obj == null)
            {
                return null;
            }
            DateTime results;
            var isSuccess = DateTime.TryParse(obj.ToSafeString(), out results);
            if (isSuccess == true)
            {
                return results;
            }
            else
            {
                return null;
            }
        }
        public static bool Has<T>(this List<T> obj, params T[] valToCheck)
        {
            if (obj == null || valToCheck.IsArrayNullOrZero())
            {
                return false;
            }
            return valToCheck.Any(a => obj.Contains(a));
        }
        public static bool Has<T>(this T obj, params T[] valToCheck)
        {
            if (obj == null || valToCheck.IsArrayNullOrZero())
            {
                return false;
            }
            return valToCheck.ToList().Any(a => a.Equals(obj));
        }
        public static bool Has(this string obj, string valToCheck)
        {
            if (obj == null)
            {
                return false;
            }
            return obj.ToSafeString().ToUpper().IndexOf(valToCheck.ToUpper().Trim()) > 0 || obj.ToSafeString().ToUpper().Trim() == valToCheck.ToUpper().Trim();
        }
        public static bool Has(this string obj, params string[] valToCheck)
        {
            if (obj == null || valToCheck.IsArrayNullOrZero())
            {
                return false;
            }
            return valToCheck.ToList().Any(a => a.ToUpper().Trim() == obj.Trim().ToUpper());
        }
        public static bool IsArrayNullOrZero<T>(this T[] obj, params T[] valToCheck)
        {
            return (obj == null || obj.Length <= 0);
        }
        public static bool IsArrayNullOrZero(this object[] obj, params object[] valToCheck)
        {
            return !(obj == null || obj.Length <= 0);
        }
        public static bool IsArrayNullOrZero(this List<object> obj, params object[] valToCheck)
        {
            return !(obj == null || obj.Count <= 0);
        }
        public static Int32? ToSafeInt32(this object obj)
        {
            if (obj == null)
            {
                return 0;
            }
            Int32 results = 0;
            Int32.TryParse(obj.ToString(), out results);
            return results;
        }
        public static string ToSafeString(this object obj)
        {
            if (obj == null)
            {
                return "";
            }
            return obj.ToString().Trim(); ;
        }
        public static bool ToSafeBool(this object obj)
        {
            if (obj == null)
            {
                return false;
            }
            bool isRes = false;
            obj = obj.ToSafeString();
            if (obj.ToSafeString().Has("1,YES,AGREE".Split(',')))
            {
                return true;
            }
            else if (obj.ToSafeString().Has("0,NO,DISAGREE".Split(',')))
            {
                return false;
            }
            bool.TryParse(obj.ToSafeString(), out isRes);
            return isRes;
        }

        public static DateTime CurrentWeekStartDate(this DateTime dt)
        {
            //get current day
            int day = (int)dt.Date.DayOfWeek;
            if (day == 0) { day = 7; }
            
            //days from Monday
            int dayDiff = 1 - day;
            
            return dt.Date.AddDays(dayDiff);
        }
    }
}
