using System;

namespace PSM_Agent
{
    public static class CommandRules
    {
        private static readonly string[] permitted =
        {
            "/nt",
            "/ntcn",
            "/kick",
            "/mmake"
        };

        public static void Check(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new ArgumentException("Command cannot be empty.");

            if (input.Length > 200)
                throw new ArgumentException("Command exceeds maximum length.");

            if (!input.StartsWith("/"))
                throw new ArgumentException("Command must start with '/'.");

            bool isAllowed = false;
            foreach (var cmd in permitted)
            {
                if (input.StartsWith(cmd, StringComparison.OrdinalIgnoreCase))
                {
                    isAllowed = true;
                    break;
                }
            }

            if (!isAllowed)
                throw new ArgumentException("Command not recognized or not permitted.");
        }
    }
}
