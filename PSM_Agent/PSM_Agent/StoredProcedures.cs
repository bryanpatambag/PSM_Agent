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
            SqlPipe pipe = SqlContext.Pipe;
            try
            {
                int outcome = CommandExecutor.Process(serviceName.ToString(), commandInput.ToString());

                if (outcome == 0)
                {
                    pipe.Send($"Successfully dispatched command to {serviceName}: {commandInput}");
                }
                else
                {
                    pipe.Send($"Command failed for {serviceName}: {commandInput}");
                }
                return outcome;
            }
            catch (Exception ex)
            {
                pipe.Send($"Execution error for {serviceName}: {ex.Message}");
                return -1;
            }
        }
    }
}
