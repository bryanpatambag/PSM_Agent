using System;
using System.IO;
using System.Linq;
using System.Text;

namespace PSM_Agent
{
    public static class RequestThrottle
    {
        public static void Enforce(string serviceName)
        {
            if (!File.Exists(ServiceConfig.LogFilePath)) return;
            string lastEntry = File.ReadLines(ServiceConfig.LogFilePath, Encoding.UTF8).LastOrDefault();
            if (string.IsNullOrEmpty(lastEntry)) return;
            string[] parts = lastEntry.Split('|');
            if (DateTime.TryParse(parts[0], out DateTime lastTimestamp))
            {
                if ((DateTime.Now - lastTimestamp).TotalSeconds < ServiceConfig.MinIntervalSeconds)
                {
                    throw new InvalidOperationException("Request rate exceeded. Please wait before retrying.");
                }
            }
        }
    }
}