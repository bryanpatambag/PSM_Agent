using System;
using System.IO;
using System.Text;

namespace PSM_Agent
{
    public static class RateLimiter
    {
        private static readonly string logPath = @"C:\ShaiyaServer\PSM_Client\PSM_Agent.txt";

        public static void Check(string Service)
        {
            DateTime now = DateTime.Now;

            if (File.Exists(logPath))
            {
                string[] lines = File.ReadAllLines(logPath, Encoding.UTF8);
                if (lines.Length > 0)
                {
                    string lastLine = lines[lines.Length - 1];
                    string[] parts = lastLine.Split('|');
                    if (parts.Length > 1 && DateTime.TryParse(parts[0], out DateTime lastTime))
                    {
                        if ((now - lastTime).TotalSeconds < 1)
                            throw new InvalidOperationException("Too many requests, please wait.");
                    }
                }
            }
        }
    }
}
