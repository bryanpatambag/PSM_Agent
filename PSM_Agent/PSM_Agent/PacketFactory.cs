using System.IO;
using System.Text;

namespace PSM_Agent
{
    public static class PacketFactory
    {
        private const int HeaderSize = 258;
        private const short HeaderMarker = 1281;

        public static byte[] Create(string service, string command)
        {
            byte[] header = BuildHeader(service);

            using (var ms = new MemoryStream())
            using (var bw = new BinaryWriter(ms, Encoding.UTF8, true))
            {
                short size = (short)(2 + header.Length + 2 + command.Length);
                bw.Write(size);
                bw.Write(header);
                bw.Write((short)command.Length);
                bw.Write(Encoding.UTF8.GetBytes(command));
                return ms.ToArray();
            }
        }

        private static byte[] BuildHeader(string service)
        {
            var buffer = new byte[HeaderSize];
            using (var ms = new MemoryStream(buffer))
            using (var bw = new BinaryWriter(ms, Encoding.UTF8, true))
            {
                bw.Write(HeaderMarker);
                bw.Write(Encoding.UTF8.GetBytes(service));
                return buffer;
            }
        }
    }
}
