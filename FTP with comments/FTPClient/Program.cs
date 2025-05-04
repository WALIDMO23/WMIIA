using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class FTPClient
{
    static void Main()
    {
        Console.Write("Enter FTP Server IP: ");
        string serverIp = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(serverIp))
        {
            serverIp = "127.0.0.1";
            Console.WriteLine($"Using default IP: {serverIp}");
        }

        try
        {
            using (TcpClient client = new TcpClient(serverIp, 21))
            using (NetworkStream controlStream = client.GetStream())
            using (StreamReader reader = new StreamReader(controlStream, Encoding.ASCII))
            using (StreamWriter writer = new StreamWriter(controlStream, Encoding.ASCII) { AutoFlush = true })
            {
                Console.WriteLine(reader.ReadLine());

                Console.Write("Username: ");
                string username = Console.ReadLine();
                writer.WriteLine($"USER {username}");
                Console.WriteLine(reader.ReadLine());

                Console.Write("Password: ");
                string password = Console.ReadLine();
                writer.WriteLine($"PASS {password}");
                Console.WriteLine(reader.ReadLine());

                while (true)
                {
                    Console.Write("ftp> ");
                    string command = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(command)) continue;
                    if (command.ToLower() == "exit") break;

                    string[] parts = command.Split(' ', 2);
                    string cmd = parts[0].ToUpper();
                    string argument = parts.Length > 1 ? parts[1] : null;

                    switch (cmd)
                    {
                        case "LIST":
                        case "RETR":
                        case "STOR":
                            writer.WriteLine("PASV");
                            string pasvResponse = reader.ReadLine();
                            Console.WriteLine(pasvResponse);

                            if (!TryParsePassiveInfo(pasvResponse, out string dataIp, out int dataPort))
                            {
                                Console.WriteLine("Invalid PASV response.");
                                continue;
                            }

                            writer.WriteLine(command);
                            string response = reader.ReadLine();
                            Console.WriteLine(response);

                            if (response.StartsWith("150"))
                            {
                                switch (cmd)
                                {
                                    case "LIST":
                                        using (TcpClient dataClient = new TcpClient(dataIp, dataPort))
                                        using (StreamReader dataReader = new StreamReader(dataClient.GetStream()))
                                        {
                                            string line;
                                            while ((line = dataReader.ReadLine()) != null)
                                                Console.WriteLine(line);
                                        }
                                        break;

                                    case "RETR":
                                        if (argument != null)
                                            ReceiveFile(dataIp, dataPort, argument);
                                        break;

                                    case "STOR":
                                        if (argument != null && File.Exists(argument))
                                            SendFile(dataIp, dataPort, argument);
                                        break;
                                }

                                Console.WriteLine(reader.ReadLine()); // Read 226
                            }
                            break;

                        case "CWD":
                            if (argument != null)
                            {
                                writer.WriteLine(command);
                                Console.WriteLine(reader.ReadLine());
                            }
                            break;

                        default:
                            writer.WriteLine(command);
                            Console.WriteLine(reader.ReadLine());
                            break;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    static void ReceiveFile(string ip, int port, string filename)
    {
        Console.Write("Enter path to save the file (or press Enter for default): ");
        string savePath = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(savePath)) savePath = filename;

        try
        {
            using (TcpClient dataClient = new TcpClient(ip, port))
            using (NetworkStream dataStream = dataClient.GetStream())
            using (FileStream fileStream = new FileStream(savePath, FileMode.Create))
            {
                byte[] buffer = new byte[8192];
                int bytesRead;
                while ((bytesRead = dataStream.Read(buffer, 0, buffer.Length)) > 0)
                    fileStream.Write(buffer, 0, bytesRead);
                Console.WriteLine($"Download complete: {savePath}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error receiving file: {ex.Message}");
        }
    }

    static void SendFile(string ip, int port, string filename)
    {
        try
        {
            using (TcpClient dataClient = new TcpClient(ip, port))
            using (NetworkStream dataStream = dataClient.GetStream())
            using (FileStream fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                byte[] buffer = new byte[8192];
                int bytesRead;
                while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                    dataStream.Write(buffer, 0, bytesRead);
                Console.WriteLine($"Upload complete: {filename}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending file: {ex.Message}");
        }
    }

    static bool TryParsePassiveInfo(string response, out string ip, out int port)
    {
        ip = null; port = 0;
        int start = response.IndexOf('(');
        int end = response.IndexOf(')');
        if (start < 0 || end < 0 || end <= start) return false;

        string[] parts = response.Substring(start + 1, end - start - 1).Split(',');
        if (parts.Length != 6) return false;

        ip = string.Join(".", parts[0], parts[1], parts[2], parts[3]);
        port = (int.Parse(parts[4]) << 8) + int.Parse(parts[5]);
        return true;
    }
}
