using System;

namespace PSM_Agent
{
    public static class CommandRules
    {
        private const int MaxCommandLength = 128;

        private static readonly string[] Permitted =
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

            if (input.Length > MaxCommandLength)
                throw new ArgumentException($"Command exceeds maximum length of {MaxCommandLength} characters.");

            if (!input.StartsWith("/"))
                throw new ArgumentException("Command must start with '/'.");

            bool isAllowed = Array.Exists(Permitted,
                cmd => input.StartsWith(cmd, StringComparison.OrdinalIgnoreCase));

            if (!isAllowed)
                throw new ArgumentException("Command not recognized or not permitted.");
        }
    }
}
