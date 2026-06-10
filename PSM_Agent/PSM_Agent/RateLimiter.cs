using System;
using System.IO;
using System.Text;

namespace PSM_Agent
{
    public static class RequestThrottle
    {
        private static readonly string logFile = @"C:\ShaiyaServer\PSM_Client\PSM_Agent.txt";
        public static void Enforce(string serviceName)
        {
            DateTime currentTime = DateTime.Now;
            if (!File.Exists(logFile))
                return;
            string[] entries = File.ReadAllLines(logFile, Encoding.UTF8);
            if (entries.Length == 0)
                return;
            string lastEntry = entries[entries.Length - 1];
            string[] parts = lastEntry.Split('|');
            if (parts.Length > 1 && DateTime.TryParse(parts[0], out DateTime lastTimestamp))
            {
                double elapsedSeconds = (currentTime - lastTimestamp).TotalSeconds;
                if (elapsedSeconds < 1)
                {
                    throw new InvalidOperationException("Request rate exceeded. Please wait before retrying.");
                }
            }
        }
    }
}
