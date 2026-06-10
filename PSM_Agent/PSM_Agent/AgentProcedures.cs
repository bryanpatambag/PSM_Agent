using System;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

namespace PSM_Agent
{
    public class AgentProcedures
    {
        [SqlProcedure]
        public static int RunServiceCommand(SqlString serviceName, SqlString commandInput)
        {
            try
            {
                int result = CommandExecutor.Process(serviceName.Value, commandInput.Value);

                string message = result == 0
                    ? $"OK {serviceName}: {commandInput}"
                    : $"FAIL {serviceName}: {commandInput}";

                SqlContext.Pipe.Send(message);
                return result;
            }
            catch (Exception ex)
            {
                SqlContext.Pipe.Send($"ERROR {serviceName}: {ex.Message}");
                return -1;
            }
        }
    }
}