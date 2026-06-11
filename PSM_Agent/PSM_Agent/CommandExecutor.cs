using System;

namespace PSM_Agent
{
    public static class CommandExecutor
    {
        public static int Process(string service, string command)
        {
            try
            {
                CommandRules.Check(command);
                RequestThrottle.Enforce(service);
                byte[] buffer = new byte[ServiceConfig.BufferSize];
                byte[] packet = PacketFactory.Create(service, command);
                SocketHelper.Execute(ServiceConfig.Host, ServiceConfig.Port, packet, buffer);
                ActivityLogger.RecordSuccess(service, command);
                return 0;
            }
            catch (Exception ex)
            {
                string errorMsg = ErrorHandler.Format(ex, service, command);
                ActivityLogger.RecordFailure(service, command, errorMsg);
                return -1;
            }
        }
    }
}
