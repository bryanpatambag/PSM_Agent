using System;

namespace PSM_Agent
{
    public static class CommandValidator
    {
        private static readonly string[] allowedCommands =
        {
            "/nt",
            "/ntcn",
            "/kick",
            "/mmake"
        };

        public static void Validate(string CommandText)
        {
            if (string.IsNullOrWhiteSpace(CommandText))
                throw new ArgumentException("Empty command.");

            if (CommandText.Length > 200)
                throw new ArgumentException("Command too long.");

            if (!CommandText.StartsWith("/"))
                throw new ArgumentException("Invalid command format.");

            bool valid = false;
            foreach (var cmd in allowedCommands)
            {
                if (CommandText.StartsWith(cmd, StringComparison.OrdinalIgnoreCase))
                {
                    valid = true;
                    break;
                }
            }

            if (!valid)
                throw new ArgumentException("Command not allowed.");
        }
    }
}
