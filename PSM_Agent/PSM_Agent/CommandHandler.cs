using System;
using System.Net.Sockets;

namespace PSM_Agent
{
    public static class CommandHandler
    {
        public static int Execute(string Service, string CommandText)
        {
            try
            {
                CommandValidator.Validate(CommandText);
                RateLimiter.Check(Service);

                using (Socket sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    sender.Connect("127.0.0.1", 40900);
                    byte[] packet = PacketBuilder.Build(Service, CommandText);
                    sender.Send(packet);
                    sender.Receive(new byte[1024]);
                    sender.Shutdown(SocketShutdown.Both);
                }

                Logger.LogSuccess(Service, CommandText);
                return 0;
            }
            catch (Exception ex)
            {
                Logger.LogError(Service, CommandText, ex.Message);
                return -1;
            }
        }
    }
}
