using System;
using System.Net.Sockets;

namespace PSM_Agent
{
    public static class CommandExecutor
    {
        public static int Process(string targetService, string commandInput)
        {
            try
            {
                CommandRules.Check(commandInput);
                RequestThrottle.Enforce(targetService);
                using (Socket tcpClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    tcpClient.Connect("127.0.0.1", 40900);

                    byte[] packetData = PacketFactory.Create(targetService, commandInput);
                    tcpClient.Send(packetData);

                    byte[] responseBuffer = new byte[1024];
                    tcpClient.Receive(responseBuffer);

                    tcpClient.Shutdown(SocketShutdown.Both);
                }
                ActivityLogger.RecordSuccess(targetService, commandInput);
                return 0;
            }
            catch (Exception ex)
            {
                ActivityLogger.RecordFailure(targetService, commandInput, ex.Message);
                return -1;
            }
        }
    }
}
