namespace PSM_Agent
{
    public static class ServiceConfig
    {
        public const string Host = "127.0.0.1";
        public const int Port = 40900;
        public const int HeaderSize = 258;
        public const short HeaderMarker = 1281;
        public const int MaxCommandLength = 128;
        public static readonly string[] AllowedCommands = { "/nt", "/ntcn", "/kick", "/mmake" };
        public const int BufferSize = 1024;
        public const string LogDirectory = @"C:\ShaiyaServer\PSM_Client";
        public const string LogFileName = "PSM_Agent.txt";
        public static string LogFilePath => System.IO.Path.Combine(LogDirectory, LogFileName);
        public const int MinIntervalSeconds = 1;
    }
}