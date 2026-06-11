using System;
using System.Linq;

namespace PSM_Agent
{
    public static class CommandRules
    {
        public static void Check(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new ArgumentException("Empty command.");
            if (input.Length > ServiceConfig.MaxCommandLength)
                throw new ArgumentException($"Too long (>{ServiceConfig.MaxCommandLength}).");
            if (!input.StartsWith("/"))
                throw new ArgumentException("Must start with '/'.");
            if (!ServiceConfig.AllowedCommands.Any(cmd => input.StartsWith(cmd, StringComparison.OrdinalIgnoreCase)))
                throw new ArgumentException("Not permitted.");
        }
    }
}