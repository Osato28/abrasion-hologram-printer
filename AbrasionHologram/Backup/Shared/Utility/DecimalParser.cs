using System;
using System.Globalization;

namespace Utility
{
    /** Inspired by this answer on StackOverflow: https://stackoverflow.com/a/76516146/,
     * but written as a helper class because I didn't feel comfortable extending a native type just to make two minor fixes.
    */
    public class DecimalParser
    {
        public static decimal ParseToDecimal(string str)
        {
            if (String.IsNullOrEmpty(str)) throw new ArgumentNullException("str", "Tried to parse a null or empty string");
            if (decimal.TryParse(str, NumberStyles.Any, CultureInfo.CurrentUICulture, out decimal d))
                return d;
            if (decimal.TryParse(str, System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture, out d))
                return d;
            throw new Exception("Failed to parse a string value to a decimal");
        }
        public static double ParseToDouble(string str)
        {
            if (String.IsNullOrEmpty(str)) throw new ArgumentNullException("str", "Tried to parse a null or empty string");
            if (double.TryParse(str, NumberStyles.Any, CultureInfo.CurrentUICulture, out double d))
                return d;
            if (double.TryParse(str, System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture, out d))
                return d;
            throw new Exception("Failed to parse a string value to a double");
        }
    }
}
