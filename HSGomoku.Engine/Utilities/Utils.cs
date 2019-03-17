using System;

namespace HSGomoku.Engine.Utilities
{
    internal static class Utils
    {
        public static String NumberToAlphabet(Int32 number, Boolean isCaps = true)
        {
            if (number < 1 || number > 36)
            {
                return isCaps ? "A" : "a";
            }

            Char c = (Char)((isCaps ? 65 : 97) + number);
            return c.ToString();
        }
    }
}