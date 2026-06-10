using System;
using System.IO;
using System.Text;

namespace PSM_Agent
{
    public static class ActivityLogger
    {
        private static readonly string filePath = @"C:\ShaiyaServer\PSM_Client\PSM_Agent.txt";
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
            var builder = new StringBuilder();
            builder.Append($"{timestamp}|{status}|{currentUser}|{serviceName}|{command}");
            if (!string.IsNullOrEmpty(errorMessage))
            {
                builder.Append($"|{errorMessage}");
            }
            builder.AppendLine();
            File.AppendAllText(filePath, builder.ToString(), Encoding.UTF8);
        }
    }
}
