using System;
using System.Linq;

namespace PSM_Agent
{
    public static class CommandRules
    {
        private const int MaxLength = 128;
        private static readonly string[] Allowed = { "/nt", "/ntcn", "/kick", "/mmake" };
        public static void Check(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new ArgumentException("Empty command.");
            if (input.Length > MaxLength)
                throw new ArgumentException($"Too long (>{MaxLength}).");
            if (!input.StartsWith("/"))
                throw new ArgumentException("Must start with '/'.");
            if (!Allowed.Any(cmd => input.StartsWith(cmd, StringComparison.OrdinalIgnoreCase)))
                throw new ArgumentException("Not permitted.");
        }
    }
}