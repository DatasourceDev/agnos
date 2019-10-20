using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppFramework.Util
{
    public class NumUtil
    {
        public static decimal ParseDecimal(string s, decimal def = 0)
        {
            decimal d = 0;
            if (!string.IsNullOrEmpty(s))
            {
                try
                {
                    return Convert.ToDecimal(s.Trim().Replace(",", ""));
                }
                catch
                {
                    return d;
                }
            }
            return d;
        }

        public static int ParseInteger(string s, int def = 0)
        {
            int d = 0;
            if (!string.IsNullOrEmpty(s))
            {
                try
                {
                    return Convert.ToInt32(s.Trim().Replace(",", ""));
                }
                catch
                {
                    return d;
                }
            }
            return d;
        }

        public static int ParseInteger(double dd, int def = 0)
        {
            int d = 0;
            try
            {
                return Convert.ToInt32(dd);
            }
            catch
            {
                return d;
            }
        }

        public static bool IsNumeric(object s)
        {
            if (s != null)
                return IsNumeric2(s.ToString());

            return false;
        }


        private static bool IsNumeric2(string s)
        {
            if (!string.IsNullOrEmpty(s))
            {
                try
                {
                    var num = Convert.ToDecimal(s.Trim().Replace(",", ""));
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }


        public static string FormatCurrency(Nullable<decimal> val, bool addComma = true, int digit = 2)
        {
            try
            {
                if (val.HasValue)
                {
                    var str = "";
                    if (GetDecimalLength(val.Value) > 0)
                        str = val.Value.ToString("n" + digit.ToString());
                    else
                        str = val.Value.ToString("n0");

                    if (!addComma)
                        return str.Replace(",", "");

                    return str;
                }
                return "0";
            }
            catch
            {
                return "0";
            }
        }
        public static string FormatCurrency(Nullable<int> val,bool addComma = true, int digit = 2)
        {
            try
            {
                if (val.HasValue)
                {
                    var str = "";
                    if (GetDecimalLength(val.Value) > 0)
                        str = val.Value.ToString("n" + digit.ToString());
                    else
                        str = val.Value.ToString("n0");

                    if (!addComma)
                        return str.Replace(",", "");

                    return str;
                }
                return "0";
            }
            catch
            {
                return "0";
            }
        }


        public static int GetDecimalLength(decimal dValue)
        {

            var tempValue = dValue.ToString();
            int decimalLength = 0;
            if (tempValue.Contains('.') || tempValue.Contains(','))
            {
                char[] separator = new char[] { '.', ',' };
                string[] tempstring = tempValue.Split(separator);

                if (ParseInteger(tempstring[1]) > 0)
                    decimalLength = tempstring[1].Length;
            }
            return decimalLength;
        }

        public static int GetDecimalLength(string dValue)
        {
            var tempValue = dValue;
            int decimalLength = 0;
            if (tempValue.Contains('.') || tempValue.Contains(','))
            {
                char[] separator = new char[] { '.', ',' };
                string[] tempstring = tempValue.Split(separator);

                if (ParseInteger(tempstring[1]) > 0)
                    decimalLength = tempstring[1].Length;
            }
            return decimalLength;
        }
    }
}