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
                SqlContext.Pipe.Send(result == 0
                    ? $"OK {serviceName}: {commandInput}"
                    : $"FAIL {serviceName}: {commandInput}");
                return result;
            }
            catch (Exception ex)
            {
                string errorMsg = ErrorHandler.Format(ex, serviceName.Value, commandInput.Value);
                SqlContext.Pipe.Send(errorMsg);
                return -1;
            }
        }
    }
}
