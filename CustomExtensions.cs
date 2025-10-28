using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Data;

namespace CS_CommonBusinessLayer
{
    public static class StringExtensions
    {
        public static string TrimAndReduce(this string value)
        {
            return ConvertWhitespacesToSingleSpaces(value).Trim();
        }

        public static string ConvertWhitespacesToSingleSpaces(this string value)
        {
            return Regex.Replace(value, @"\s+", " ");
        }

        public static string Left(this string value, int chars)
        {
            chars = Math.Abs(chars);
            if (string.IsNullOrEmpty(value))
                return string.Empty;
            else
            {
                if (chars >= value.Length)
                    return value;
                else
                    return value.Substring(0, chars);
            }
        }

        public static string LeftBack(this string value, int chars)
        {
            chars = Math.Abs(chars);
            if (string.IsNullOrEmpty(value))
                return string.Empty;
            else
            {
                if (chars >= value.Length)
                    return value;
                else
                    return value.Substring(0, value.Length - chars);
            }
        }

        public static string Right(this string value, int chars)
        {
            chars = Math.Abs(chars);
            if (string.IsNullOrEmpty(value))
                return value;
            else
            {
                if (chars >= value.Length)
                    return value;
                else
                    return value.Substring(value.Length - chars, chars);
            }
        }

        public static string RightBack(this string value, int chars)
        {
            chars = Math.Abs(chars);
            if (string.IsNullOrEmpty(value))
                return string.Empty;
            else
            {
                if (chars >= value.Length)
                    return value;
                else
                    return value.Substring(chars - 1, value.Length - chars);
            }
        }

        public static string LeftChar(this string value, char c, bool includeChar = false)
        {
            int index = value.IndexOf(c);
            return index > 0 ? (includeChar ? value.Left(index + 1) : value.Left(index)) : value;
        }

        public static string RightChar(this string value, char c, bool includeChar = false)
        {
            int index = value.IndexOf(c);
            if (includeChar)
                return value.Substring(index);
            else
                return value.Substring(index + 1);
        }

        public static int? ParseInt(this string value, int? setDefault = null)
        {
            int result;

            if (int.TryParse(value, out result))
                return result;

            return setDefault;
        }

        public static long? ParseLong(this string value, long? setDefault = null)
        {
            long result;

            if (long.TryParse(value, out result))
                return result;

            return setDefault;
        }

        public static decimal? ParseDecimal(this string value, decimal? setDefault = null)
        {
            decimal result;

            if (decimal.TryParse(value, out result))
                return result;

            return setDefault;
        }

        public static DateTime? ParseDate(this string value)
        {
            DateTime result;
            
            if (DateTime.TryParse(value, out result))
                return result;

            return null;
        }
        public static Guid? ParseGuid(this string value)
        {
            Guid result;

            if (Guid.TryParse(value, out result))
                return result;

            return null;
        }
        public static string GetQueryStringParamValue(this string value, string paramName)
        {
            string result;

            if (string.IsNullOrWhiteSpace(value) || string.IsNullOrWhiteSpace(paramName))
                return string.Empty;

            string query = value.RightChar('?');

            if (!query.Contains("="))
                return string.Empty;

            Dictionary<string,string> queryValues = query.Split('&').Select(piQ => piQ.Split('=')).ToDictionary(piKey => piKey[0].ToLower().Trim(), piValue => piValue[1]);
            
            bool found = queryValues.TryGetValue(paramName.ToLower().Trim(), out result);

            return found ? result : string.Empty;
        }

        public static bool IsEmail(this string value)
        {
            try
            {
                MailAddress m = new MailAddress(value);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        
        public static string FormatPhone(this string value, bool requireAreaCode = false)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;

            string numbers = Regex.Replace(value, @"[^0-9Xx]+", "").ToLower();
            string number = numbers.LeftChar('x');

            string ext = numbers.IndexOf('x') > 0 ? numbers.RightChar('x') : "";

            switch(number.Length)
            {
                case 7:
                    if (requireAreaCode)
                        return "(000)" + number.Left(3) + "-" + number.Right(4) + (ext.Length > 0 ? " x" + ext : "");
                    else
                        return number.Left(3) + "-" + number.Right(4) + (ext.Length > 0 ? " x" + ext : "");
                case 10:
                    return "(" + number.Left(3) + ")" + number.Substring(3,3) + "-" + number.Right(4) + (ext.Length > 0 ? " x" + ext : "");
                case 11:
                    return number.Left(1) + "(" + number.Substring(1, 3) + ")" + number.Substring(4, 3) + "-" + number.Right(4) + (ext.Length > 0 ? " x" + ext : "");
                default:
                    return string.Empty;
            }
        }

        public static string Reverse(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            char[] chars = value.ToCharArray();

            Array.Reverse(chars);

            return new String(chars);
        }
    }

    public static class DataRowExtensions
    {
        public static T GetValue<T>(this DataRow row, string columnName) where T : struct
        {
            try
            {
                return (T)(row.Field<dynamic>(columnName) ?? default(T));
            }
            catch { return default(T); }
        }

        public static int GetIntValue(this DataRow row, string columnName, int setDefault = 0)
        {
            try
            {
                return (Int32)(row.Field<dynamic>(columnName) ?? setDefault);
            }
            catch { return setDefault; }
        }

        public static long GetLongValue(this DataRow row, string columnName, long setDefault = 0)
        {
            try
            {
                return (Int64)(row.Field<dynamic>(columnName) ?? setDefault);
            }
            catch { return setDefault; }
        }

        public static double GetDoubleValue(this DataRow row, string columnName, double setDefault = 0)
        {
            try
            {
                return (double)(row.Field<dynamic>(columnName) ?? setDefault);
            }
            catch { return setDefault; }
        }

        public static string GetText(this DataRow row, string columnName, string setDefault = "")
        {
            try
            {
                return (string)(row.Field<dynamic>(columnName) ?? setDefault);
            }
            catch { return setDefault; }
        }

        public static char GetChar(this DataRow row, string columnName, char setDefault = ' ')
        {
            try
            {
                return (char)(row.Field<dynamic>(columnName)[0] ?? setDefault);
            }
            catch { return setDefault; }
        }

        public static bool GetBooleanValue(this DataRow row, string columnName, bool setDefault = false)
        {
            try
            {
                return (bool)(row.Field<dynamic>(columnName) ?? setDefault);
            }
            catch { return setDefault; }
        }

        public static Guid GetGuidValue(this DataRow row, string columnName, Guid setDefault = default(Guid))
        {
            try
            {
                return (Guid)(row.Field<dynamic>(columnName) ?? setDefault);
            }
            catch { return setDefault; }
        }

        public static DateTime? GetDateTimeValue(this DataRow row, string columnName, DateTime? setDefault = null)
        {
            try
            {
                return (DateTime?)(row.Field<dynamic>(columnName) ?? setDefault);
            }
            catch { return setDefault; }
        }

        public static byte[] GetByteArrayValue(this DataRow row, string columnName)
        {
            try
            {
                return (byte[])(row.Field<dynamic>(columnName) ?? null);
            }
            catch { return null; }
        }

        public static decimal GetDecimalValue(this DataRow row, string columnName, decimal setDefault = 0)
        {
            try
            {
                return (decimal)(row.Field<dynamic>(columnName) ?? setDefault);
            }
            catch { return setDefault; }
        }
    }

    public static class ListExtensions
    {
        public static List<T> Sort<T>(this List<T> data, string sortExpression, string sortDirection)
        {

            List<T> data_sorted = new List<T>();

            if (sortDirection == "ASC")
            {
                data_sorted = (from n in data
                               orderby n.GetDynamicSortProperty(sortExpression) ascending
                               select n).ToList();
            }
            else
            {
                data_sorted = (from n in data
                               orderby n.GetDynamicSortProperty(sortExpression) descending
                               select n).ToList();

            }

            return data_sorted;

        }
        private static object GetDynamicSortProperty(this object item, string propName)
        {
            //Use reflection to get order type
            //return item.GetType().GetProperty(propName).GetValue(item, null);

            try
            {
                string[] prop = propName.Split('.');

                //Use reflection to get order type                   
                int i = 0;
                while (i < prop.Count())
                {
                    item = item.GetType().GetProperty(prop[i]).GetValue(item, null);
                    i++;
                }

                return item;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }

    

    public static class LinqExtensions
    {
        public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> items, Func<T, TKey> property)
        {
            GenericPropertyComparer<T, TKey> comparer = new GenericPropertyComparer<T, TKey>(property);
            return items.Distinct(comparer);
        }
    }

    public class GenericPropertyComparer<T, TKey> : IEqualityComparer<T>
    {
        private Func<T, TKey> expr { get; set; }
        public GenericPropertyComparer(Func<T, TKey> expr)
        {
            this.expr = expr;
        }
        public bool Equals(T left, T right)
        {
            var leftProp = expr.Invoke(left);
            var rightProp = expr.Invoke(right);
            if (leftProp == null && rightProp == null)
                return true;
            else if (leftProp == null ^ rightProp == null)
                return false;
            else
                return leftProp.Equals(rightProp);
        }
        public int GetHashCode(T obj)
        {
            var prop = expr.Invoke(obj);
            return (prop == null) ? 0 : prop.GetHashCode();
        }
    }
}
