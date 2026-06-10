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
                int outcome = CommandExecutor.Process(serviceName.Value, commandInput.Value);

                SqlContext.Pipe.Send(
                    outcome == 0
                        ? $"Successfully dispatched command to {serviceName}: {commandInput}"
                        : $"Command failed for {serviceName}: {commandInput}"
                );

                return outcome;
            }
            catch (Exception ex)
            {
                SqlContext.Pipe.Send($"Execution error for {serviceName}: {ex.Message}");
                return -1;
            }
        }
    }
}
