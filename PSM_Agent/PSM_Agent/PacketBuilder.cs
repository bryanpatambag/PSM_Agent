using System.IO;
using System.Text;

namespace PSM_Agent
{
    public static class PacketFactory
    {
        public static byte[] Create(string serviceName, string command)
        {
            using (var headerStream = new MemoryStream(258))
            using (var headerWriter = new BinaryWriter(headerStream))
            {
                headerWriter.Write((short)1281);
                headerWriter.Write(Encoding.UTF8.GetBytes(serviceName));

                byte[] headerData = new byte[258];
                headerStream.Position = 0;
                headerStream.Read(headerData, 0, (int)headerStream.Length);
                using (var packetStream = new MemoryStream())
                using (var packetWriter = new BinaryWriter(packetStream))
                {
                    short totalSize = (short)(2 + headerData.Length + 2 + command.Length);
                    packetWriter.Write(totalSize);
                    packetWriter.Write(headerData);
                    packetWriter.Write((short)command.Length);
                    packetWriter.Write(Encoding.UTF8.GetBytes(command));
                    return packetStream.ToArray();
                }
            }
        }
    }
}
