using System;
using System.Net.Sockets;

namespace PSM_Agent
{
    public static class CommandExecutor
    {
        private const string Host = "127.0.0.1";
        private const int Port = 40900;
        private const int BufferSize = 1024;

        public static int Process(string targetService, string commandInput)
        {
            try
            {
                CommandRules.Check(commandInput);
                RequestThrottle.Enforce(targetService);

                byte[] responseBuffer = new byte[BufferSize];

                using (Socket tcpClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    tcpClient.Connect(Host, Port);

                    byte[] packetData = PacketFactory.Create(targetService, commandInput);
                    tcpClient.Send(packetData);

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
