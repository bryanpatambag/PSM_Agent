using System.IO;
using System.Text;

namespace PSM_Agent
{
    public static class PacketFactory
    {
        public static byte[] Build(string service, string command)
        {
            byte[] header = CreateHeader(service);
            byte[] cmdBytes = Encoding.UTF8.GetBytes(command);

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
        private static byte[] CreateHeader(string service)
        {
            var buffer = new byte[ServiceConfig.HeaderSize];
            using (var ms = new MemoryStream(buffer))
            {
                using (var bw = new BinaryWriter(ms, Encoding.UTF8, true))
                {
                    bw.Write(ServiceConfig.HeaderMarker);
                    bw.Write(Encoding.UTF8.GetBytes(service));
                }
                return buffer;
            }
        }
    }
}