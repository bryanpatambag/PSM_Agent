using System;

namespace PSM_Agent
{
    public static class CommandExecutor
    {
        public static int Process(string service, string command)
        {
            try
            {
                CommandRules.Validate(command);
                RequestThrottle.Check(service);

                byte[] buffer = new byte[ServiceConfig.BufferSize];
                byte[] packet = PacketFactory.Build(service, command);

                SocketHelper.Send(ServiceConfig.Host, ServiceConfig.Port, packet, buffer);
                ActivityLogger.Success(service, command);

                return 0;
            }
            catch (Exception ex)
            {
                ActivityLogger.Failure(service, command, ErrorHandler.Format(ex, service, command));
                return -1;
            }
        }
    }
}