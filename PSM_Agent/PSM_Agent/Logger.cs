using System;
using System.IO;
using System.Text;

namespace PSM_Agent
{
    public static class Logger
    {
        private static readonly string logPath = @"C:\ShaiyaServer\PSM_Client\PSM_Agent.txt";

        // Log success
        public static void LogSuccess(string Service, string CommandText)
        {
            AppendLog("SUCCESS", Service, CommandText, null);
        }

        // Log error
        public static void LogError(string Service, string CommandText, string Error)
        {
            AppendLog("ERROR", Service, CommandText, Error);
        }

        // Append entry to text file
        private static void AppendLog(string Status, string Service, string CommandText, string Error)
        {
            DateTime now = DateTime.Now;
            string userIdentity = System.Security.Principal.WindowsIdentity.GetCurrent()?.Name ?? "UnknownUser";

            string line = $"{now}|{Status}|{userIdentity}|{Service}|{CommandText}";
            if (!string.IsNullOrEmpty(Error))
                line += $"|{Error}";

            line += Environment.NewLine;

            File.AppendAllText(logPath, line, Encoding.UTF8);
        }
    }
}
