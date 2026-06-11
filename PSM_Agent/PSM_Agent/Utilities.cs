using System;

namespace PSM_Agent
{
    public static class Utilities
    {
        public static string FormatTimestamp(DateTime dt) =>
            dt.ToString("yyyy-MM-dd HH:mm:ss");

        public static bool IsNullOrEmpty(string input) =>
            string.IsNullOrEmpty(input);

        public static bool IsNullOrWhiteSpace(string input) =>
            string.IsNullOrWhiteSpace(input);
    }
}