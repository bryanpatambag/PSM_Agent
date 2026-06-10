using System.Net.Sockets;

namespace PSM_Agent
{
    public static class SocketHelper
    {
        public static void Execute(string host, int port, byte[] data, byte[] buffer)
        {
            using (var client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                client.Connect(host, port);
                client.Send(data);
                client.Receive(buffer);
            }
        }
    }
}