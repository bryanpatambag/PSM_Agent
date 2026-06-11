using System;
using System.IO;
using System.Linq;
using System.Text;

namespace PSM_Agent
{
    public static class RequestThrottle
    {
        public static void Check(string service)
        {
            if (!File.Exists(ServiceConfig.LogFilePath)) return;

            string last = File.ReadLines(ServiceConfig.LogFilePath, Encoding.UTF8).LastOrDefault();
            if (string.IsNullOrEmpty(last)) return;

            string[] parts = last.Split('|');
            if (DateTime.TryParse(parts[0], out DateTime lastTime))
            {
                if ((DateTime.Now - lastTime).TotalSeconds < ServiceConfig.MinIntervalSeconds)
                    throw new InvalidOperationException("Too many requests. Please wait.");
            }
        }
    }
}