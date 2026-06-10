using System;
using System.IO;
using System.Text;

namespace PSM_Agent
{
    public static class ActivityLogger
    {
        private static readonly string LogFilePath =
            Path.Combine(@"C:\ShaiyaServer\PSM_Client", "PSM_Agent.txt");

        public static void RecordSuccess(string serviceName, string command)
        {
            WriteEntry("SUCCESS", serviceName, command, null);
        }

        public static void RecordFailure(string serviceName, string command, string errorMessage)
        {
            WriteEntry("ERROR", serviceName, command, errorMessage);
        }

        private static void WriteEntry(string status, string serviceName, string command, string errorMessage)
        {
            string currentUser = System.Security.Principal.WindowsIdentity.GetCurrent()?.Name ?? "Unknown";
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            var entry = new StringBuilder()
                .Append(timestamp).Append('|')
                .Append(status).Append('|')
                .Append(currentUser).Append('|')
                .Append(serviceName).Append('|')
                .Append(command);

            if (!string.IsNullOrEmpty(errorMessage))
            {
                entry.Append('|').Append(errorMessage);
            }

            entry.AppendLine();

            File.AppendAllText(LogFilePath, entry.ToString(), Encoding.UTF8);
        }
    }
}
