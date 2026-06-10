using System;

namespace PSM_Agent
{
    public static class CommandRules
    {
        private const int MaxLength = 128;

        private static readonly string[] Allowed =
        {
            "/nt", "/ntcn", "/kick", "/mmake"
        };

        public static void Check(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new ArgumentException("Empty command.");

            if (input.Length > MaxLength)
                throw new ArgumentException($"Too long (>{MaxLength}).");

            if (!input.StartsWith("/"))
                throw new ArgumentException("Must start with '/'.");

            if (!IsAllowed(input))
                throw new ArgumentException("Not permitted.");
        }

        private static bool IsAllowed(string input) =>
            Array.Exists(Allowed, cmd => input.StartsWith(cmd, StringComparison.OrdinalIgnoreCase));
    }
}
