using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class FTPServer
{
    static void Main()
    {
        TcpListener controlListener = new TcpListener(IPAddress.Any, 21);
        controlListener.Start();
        Console.WriteLine("FTP Server started on port 21...");

        while (true)
        {
            TcpClient client = controlListener.AcceptTcpClient();
            ThreadPool.QueueUserWorkItem(HandleClient, client);
        }
    }

    static void HandleClient(object clientObj)
    {
        TcpClient client = (TcpClient)clientObj;

        using (NetworkStream stream = client.GetStream())
        using (StreamReader reader = new StreamReader(stream, Encoding.ASCII))
        using (StreamWriter writer = new StreamWriter(stream, Encoding.ASCII) { AutoFlush = true })
        {
            writer.WriteLine("220 Welcome to Simple FTP Server");

            string username = null;
            string password = null;
            string currentDir = Directory.GetCurrentDirectory();
            TcpListener passiveListener = null;

            while (true)
            {
                string line = reader.ReadLine();
                if (line == null) break;

                string[] parts = line.Split(' ', 2);
                string cmd = parts[0].ToUpper();
                string arg = parts.Length > 1 ? parts[1] : null;

                switch (cmd)
                {
                    case "USER":
                        username = arg;
                        writer.WriteLine("331 Username OK, need password");
                        break;

                    case "PASS":
                        password = arg;
                        writer.WriteLine("230 Login successful");
                        break;

                    case "PASV":
                        passiveListener?.Stop();  // Close any previous listener
                        passiveListener = new TcpListener(IPAddress.Any, 0);
                        passiveListener.Start();

                        IPEndPoint localEp = (IPEndPoint)passiveListener.LocalEndpoint;
                        byte[] address = localEp.Address.GetAddressBytes();
                        int port = localEp.Port;
                        byte p1 = (byte)(port / 256);
                        byte p2 = (byte)(port % 256);
                        writer.WriteLine($"227 Entering Passive Mode ({address[0]},{address[1]},{address[2]},{address[3]},{p1},{p2})");
                        break;

                    case "LIST":
                        writer.WriteLine("150 Opening data connection for LIST");
                        using (TcpClient dataClient = passiveListener.AcceptTcpClient())
                        using (StreamWriter dataWriter = new StreamWriter(dataClient.GetStream(), Encoding.ASCII))
                        {
                            foreach (string file in Directory.GetFiles(currentDir))
                                dataWriter.WriteLine(Path.GetFileName(file));
                        }
                        writer.WriteLine("226 Transfer complete");
                        break;

                    case "RETR":
                        writer.WriteLine("150 Opening data connection for RETR");
                        using (TcpClient dataClient = passiveListener.AcceptTcpClient())
                        using (NetworkStream dataStream = dataClient.GetStream())
                        using (FileStream fileStream = new FileStream(Path.Combine(currentDir, arg), FileMode.Open, FileAccess.Read))
                        {
                            fileStream.CopyTo(dataStream);
                        }
                        writer.WriteLine("226 Transfer complete");
                        break;

                    case "STOR":
                        writer.WriteLine("150 Opening data connection for STOR");
                        using (TcpClient dataClient = passiveListener.AcceptTcpClient())
                        using (NetworkStream dataStream = dataClient.GetStream())
                        using (FileStream fileStream = new FileStream(Path.Combine(currentDir, arg), FileMode.Create))
                        {
                            dataStream.CopyTo(fileStream);
                        }
                        writer.WriteLine("226 Transfer complete");
                        break;

                    case "CWD":
                        if (Directory.Exists(arg))
                        {
                            currentDir = Path.GetFullPath(arg);
                            writer.WriteLine("250 Directory changed");
                        }
                        else
                        {
                            writer.WriteLine("550 Directory not found");
                        }
                        break;

                    default:
                        writer.WriteLine("502 Command not implemented");
                        break;
                }
            }

            passiveListener?.Stop();
        }

        client.Close();
    }
}
