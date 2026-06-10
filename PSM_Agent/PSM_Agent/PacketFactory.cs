using System.IO;
using System.Text;

namespace PSM_Agent
{
    public static class PacketFactory
    {
        const int HeaderSize = 258;
        const short HeaderMarker = 1281;
        public static byte[] Create(string service, string command)
        {
            var header = BuildHeader(service);
            var cmdBytes = Encoding.UTF8.GetBytes(command);
            using (var ms = new MemoryStream())
            {
                using (var bw = new BinaryWriter(ms, Encoding.UTF8, true))
                {
                    bw.Write((short)(sizeof(short) + header.Length + sizeof(short) + cmdBytes.Length));
                    bw.Write(header);
                    bw.Write((short)cmdBytes.Length);
                    bw.Write(cmdBytes);
                }
                return ms.ToArray();
            }
        }
        static byte[] BuildHeader(string service)
        {
            var buffer = new byte[HeaderSize];
            using (var ms = new MemoryStream(buffer))
            {
                using (var bw = new BinaryWriter(ms, Encoding.UTF8, true))
                {
                    bw.Write(HeaderMarker);
                    bw.Write(Encoding.UTF8.GetBytes(service));
                }
                return buffer;
            }
        }
    }
}