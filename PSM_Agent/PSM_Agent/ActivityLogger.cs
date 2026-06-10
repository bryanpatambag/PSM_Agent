using System;
using System.IO;

namespace PSM_Agent
{
    public static class ActivityLogger
    {
        private static readonly string LogFilePath =
            Path.Combine(@"C:\ShaiyaServer\PSM_Client", "PSM_Agent.txt");

        public static void RecordSuccess(string serviceName, string command) =>
            WriteEntry("OK", serviceName, command);

        public static void RecordFailure(string serviceName, string command, string errorMessage) =>
            WriteEntry("FAIL", serviceName, command, errorMessage);

        private static void WriteEntry(string status, string serviceName, string command, string errorMessage = null)
        {
            var line = $"{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}|{status}|{serviceName}|{command}";
            if (!string.IsNullOrEmpty(errorMessage))
                line += $"|{errorMessage}";

            File.AppendAllText(LogFilePath, line + Environment.NewLine);
        }
    }
}