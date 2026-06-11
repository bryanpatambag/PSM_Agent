using System;

namespace PSM_Agent
{
    public static class CommandRules
    {
        public static void Validate(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new ArgumentException("Command cannot be empty.");

            if (input.Length > ServiceConfig.MaxCommandLength)
                throw new ArgumentException($"Command too long (>{ServiceConfig.MaxCommandLength}).");

            if (!input.StartsWith("/"))
                throw new ArgumentException("Command must start with '/'.");
        }
    }
}