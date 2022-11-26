using System;

namespace Misc
{
    public class Format
    {
        public static String FormatSeconds(int seconds)
        {
            int minutes = seconds / 60;
            int restSeconds = seconds - minutes * 60;
            return DoubleDigit(minutes) + ":" + DoubleDigit(restSeconds);
        }

        private static String DoubleDigit(int numeric)
        {
            if (numeric < 10) return "0" + numeric;
            return numeric.ToString();
        }
    }
}