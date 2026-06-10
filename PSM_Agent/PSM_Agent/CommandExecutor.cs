using System;

namespace PSM_Agent
{
    public static class CommandExecutor
    {
        private const string Host = "127.0.0.1";
        private const int Port = 40900;
        public static int Process(string service, string command)
        {
            try
            {
                CommandRules.Check(command);
                RequestThrottle.Enforce(service);
                byte[] buffer = new byte[1024];
                byte[] packet = PacketFactory.Create(service, command);
                SocketHelper.Execute(Host, Port, packet, buffer);
                ActivityLogger.RecordSuccess(service, command);
                return 0;
            }
            catch (Exception ex)
            {
                ActivityLogger.RecordFailure(service, command, ex.Message);
                return -1;
            }
        }
    }
}