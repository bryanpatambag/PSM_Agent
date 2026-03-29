using System;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

namespace PSM_Agent
{
    public class StoredProcedures
    {
        [SqlProcedure]
        public static int ExecuteCommand(SqlString Service, SqlString CommandText)
        {
            SqlPipe pipe = SqlContext.Pipe;
            try
            {
                int result = CommandHandler.Execute(Service.ToString(), CommandText.ToString());

                if (result == 0)
                {
                    pipe.Send($"Command sent to {Service}: {CommandText}");
                }
                else
                {
                    pipe.Send($"Failed to execute command for {Service}: {CommandText}");
                }

                return result;
            }
            catch (Exception ex)
            {
                pipe.Send($"Error executing command for {Service}: {ex.Message}");
                return -1;
            }
        }
    }
}
