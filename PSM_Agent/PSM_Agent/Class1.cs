using System;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using System.Text;
using System.IO;
using System.Net.Sockets;

public class StoredProcedures
{
    private static readonly string logPath = @"C:\ShaiyaServer\PSM_Client\PSM_Agent.txt";
    private static readonly string[] allowedCommands = { "/nt", "/ntcn", "/kickcn", "/mmake" };

    [SqlProcedure]
    public static int Command(SqlString serviceName, SqlString cmmd)
    {
        Socket sender = null;
        SqlPipe client = SqlContext.Pipe;
        string output = "0";
        DateTime now = DateTime.Now;
        string userIdentity = SqlContext.WindowsIdentity?.Name ?? "UnknownUser";

        try
        {
            string service = serviceName.ToString();
            string command = cmmd.ToString();

            // Input validation
            if (string.IsNullOrWhiteSpace(command))
                throw new ArgumentException("Empty command.");
            if (command.Length > 200)
                throw new ArgumentException("Command too long.");
            if (!command.StartsWith("/"))
                throw new ArgumentException("Invalid command format.");

            // Whitelist check
            bool valid = false;
            foreach (var cmd in allowedCommands)
            {
                if (command.StartsWith(cmd))
                {
                    valid = true;
                    break;
                }
            }
            if (!valid)
                throw new ArgumentException("Command not allowed.");

            // Rate limiting (1 second between commands)
            if (File.Exists(logPath))
            {
                string[] lines = File.ReadAllLines(logPath, Encoding.UTF8);
                if (lines.Length > 0)
                {
                    string lastLine = lines[lines.Length - 1];
                    string[] parts = lastLine.Split('|');
                    if (parts.Length > 1 && DateTime.TryParse(parts[0], out DateTime lastTime))
                    {
                        if ((now - lastTime).TotalSeconds < 1)
                            throw new InvalidOperationException("Too many requests, please wait.");
                    }
                }
            }

            // Connect to PSM_Agent
            sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sender.Connect("127.0.0.1", 40900);

            var temp = new BinaryWriter(new MemoryStream(258));
            var binary = new BinaryWriter(new MemoryStream());

            // header + service
            temp.Write((short)1281);
            temp.Write(Encoding.UTF8.GetBytes(service));

            byte[] data = new byte[258];
            var stream = (MemoryStream)temp.BaseStream;
            stream.Position = 0;
            stream.Read(data, 0, (int)stream.Length);

            // build packet
            binary.BaseStream.Position = 0;
            short size = (short)(2 + data.Length + 2 + command.Length);
            binary.Write(size);
            binary.Write(data);
            binary.Write((short)command.Length);
            binary.Write(Encoding.UTF8.GetBytes(command));

            byte[] packet = ((MemoryStream)binary.BaseStream).ToArray();
            sender.Send(packet);
            sender.Receive(new byte[1024]);
            sender.Shutdown(SocketShutdown.Both);

            output = $"Command sent to {service}: {command}";
            client.Send(output);

            // Success log
            File.AppendAllText(logPath, $"{now}|SUCCESS|{userIdentity}|{service}|{command}\r\n", Encoding.UTF8);
            return 0;
        }
        catch (Exception ex)
        {
            output = $"Error: {ex.Message}";
            client.Send(output);

            // Error log
            File.AppendAllText(logPath, $"{now}|ERROR|{userIdentity}|{serviceName}|{cmmd}|{ex.Message}\r\n", Encoding.UTF8);
            return -1;
        }
        finally
        {
            if (sender != null) sender.Close();
        }
    }
}
