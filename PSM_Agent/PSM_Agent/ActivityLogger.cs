using System;
using System.IO;

namespace PSM_Agent
{
    public static class ActivityLogger
    {
        public static void Success(string service, string command) =>
            Write("OK", service, command);

        public static void Failure(string service, string command, string error) =>
            Write("FAIL", service, command, error);

        private static void Write(string status, string service, string command, string error = null)
        {
            string entry = $"{Utilities.FormatTimestamp(DateTime.UtcNow)}|{status}|{service}|{command}";
            if (!Utilities.IsNullOrEmpty(error))
                entry += $"|{error}";

            File.AppendAllText(ServiceConfig.LogFilePath, entry + Environment.NewLine);
        }
    }
}