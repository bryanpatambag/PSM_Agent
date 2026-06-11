using System;

namespace PSM_Agent
{
    public static class Utilities
    {
        public static string FormatTimestamp(DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd HH:mm:ss");
        }
        public static bool IsNullOrEmpty(string input)
        {
            return string.IsNullOrEmpty(input);
        }
    }
}