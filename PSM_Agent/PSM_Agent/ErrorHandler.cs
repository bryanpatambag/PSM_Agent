using System;

namespace PSM_Agent
{
    public static class ErrorHandler
    {
        public static string Format(Exception ex, string service, string command) =>
            $"ERROR {service}: {command} | {ex.Message}";
    }
}