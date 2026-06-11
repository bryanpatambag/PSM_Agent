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
            if (serviceName.IsNull || commandInput.IsNull)
            {
                SqlContext.Pipe.Send("ERROR: Missing service or command.");
                return -1;
            }

            string svc = serviceName.Value;
            string cmd = commandInput.Value;

            try
            {
                int code = CommandExecutor.Process(svc, cmd);
                SqlContext.Pipe.Send(code == 0 ? $"OK {svc}: {cmd}" : $"FAIL {svc}: {cmd}");
                return code;
            }
            catch (Exception ex)
            {
                SqlContext.Pipe.Send(ErrorHandler.Format(ex, svc, cmd));
                return -1;
            }
        }
    }
}