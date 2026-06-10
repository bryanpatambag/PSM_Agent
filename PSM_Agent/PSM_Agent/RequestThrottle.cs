using System;
using System.IO;
using System.Text;

namespace PSM_Agent
{
    public static class RequestThrottle
    {
        private static readonly string LogFilePath = @"C:\ShaiyaServer\PSM_Client\PSM_Agent.txt";
        private const double MinIntervalSeconds = 1.0;

        public static void Enforce(string serviceName)
        {
            if (!File.Exists(LogFilePath))
                return;

            string[] entries = File.ReadAllLines(LogFilePath, Encoding.UTF8);
            if (entries.Length == 0)
                return;

            string lastEntry = entries[entries.Length - 1];
            string[] parts = lastEntry.Split('|');

            if (parts.Length > 0 && DateTime.TryParse(parts[0], out DateTime lastTimestamp))
            {
                double elapsedSeconds = (DateTime.Now - lastTimestamp).TotalSeconds;
                if (elapsedSeconds < MinIntervalSeconds)
                {
                    throw new InvalidOperationException("Request rate exceeded. Please wait before retrying.");
                }
            }
        }
    }
}
