using System;
using System.IO;
using System.Linq;
using System.Text;

namespace PSM_Agent
{
    public static class RequestThrottle
    {
        private static readonly string LogFilePath = @"C:\ShaiyaServer\PSM_Client\PSM_Agent.txt";
        private const int MinIntervalSeconds = 1;
        public static void Enforce(string serviceName)
        {
            if (!File.Exists(LogFilePath)) return;
            string lastEntry = File.ReadLines(LogFilePath, Encoding.UTF8).LastOrDefault();
            if (string.IsNullOrEmpty(lastEntry)) return;
            string[] parts = lastEntry.Split('|');
            DateTime lastTimestamp;
            if (DateTime.TryParse(parts[0], out lastTimestamp))
            {
                if ((DateTime.Now - lastTimestamp).TotalSeconds < MinIntervalSeconds)
                {
                    throw new InvalidOperationException("Request rate exceeded. Please wait before retrying.");
                }
            }
        }
    }
}