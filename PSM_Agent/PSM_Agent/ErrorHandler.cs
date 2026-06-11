using System;

namespace PSM_Agent
{
    public static class ErrorHandler
    {
        public static string Format(Exception ex, string service, string command)
        {
            return $"ERROR {service}: {command} | {ex.Message}";
        }
    }
}