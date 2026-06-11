using System;

namespace PSM_Agent
{
    public static class ErrorHandler
    {
        public static string Format(Exception ex, string serviceName, string command)
        {
            return $"ERROR {serviceName}: {command} | {ex.Message}";
        }
    }
}