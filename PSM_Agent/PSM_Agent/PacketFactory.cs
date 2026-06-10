using System.IO;
using System.Text;

namespace PSM_Agent
{
    public static class PacketFactory
    {
        private const int HeaderSize = 258;
        private const short HeaderMarker = 1281;

        public static byte[] Create(string serviceName, string command)
        {
            var headerData = BuildHeader(serviceName);

            return BuildPacket(headerData, command);
        }

        private static byte[] BuildHeader(string serviceName)
        {
            var buffer = new byte[HeaderSize];
            using (var stream = new MemoryStream(buffer))
            using (var writer = new BinaryWriter(stream, Encoding.UTF8, true))
            {
                writer.Write(HeaderMarker);
                writer.Write(Encoding.UTF8.GetBytes(serviceName));
                writer.Flush();
            }
            return buffer;
        }

        private static byte[] BuildPacket(byte[] headerData, string command)
        {
            using (var packetStream = new MemoryStream())
            using (var writer = new BinaryWriter(packetStream, Encoding.UTF8, true))
            {
                short totalSize = (short)(2 + headerData.Length + 2 + command.Length);
                writer.Write(totalSize);
                writer.Write(headerData);
                writer.Write((short)command.Length);
                writer.Write(Encoding.UTF8.GetBytes(command));
                writer.Flush();
                return packetStream.ToArray();
            }
        }
    }
}
