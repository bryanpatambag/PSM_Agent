using System.IO;
using System.Text;

namespace PSM_Agent
{
    public static class PacketBuilder
    {
        public static byte[] Build(string Service, string CommandText)
        {
            var temp = new BinaryWriter(new MemoryStream(258));
            var binary = new BinaryWriter(new MemoryStream());

            temp.Write((short)1281);
            temp.Write(Encoding.UTF8.GetBytes(Service));

            byte[] data = new byte[258];
            var stream = (MemoryStream)temp.BaseStream;
            stream.Position = 0;
            stream.Read(data, 0, (int)stream.Length);

            binary.BaseStream.Position = 0;
            short size = (short)(2 + data.Length + 2 + CommandText.Length);
            binary.Write(size);
            binary.Write(data);
            binary.Write((short)CommandText.Length);
            binary.Write(Encoding.UTF8.GetBytes(CommandText));

            return ((MemoryStream)binary.BaseStream).ToArray();
        }
    }
}
