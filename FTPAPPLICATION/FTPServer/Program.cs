using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace FTPServer
{
    public class FTPServer
    {
        private TcpListener _listener;
        private bool _isRunning;

        public FTPServer(string ip, int port)
        {
            _listener = new TcpListener(IPAddress.Parse(ip), port);
        }

        public void Start()
        {
            _listener.Start();
            _isRunning = true;
            Console.WriteLine("FTP Server is running...");

            while (_isRunning)
            {
                TcpClient client = _listener.AcceptTcpClient();
                Console.WriteLine("Client connected.");
                HandleClient(client);
            }
        }

        private void HandleClient(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            StreamReader reader = new StreamReader(stream, Encoding.ASCII);
            StreamWriter writer = new StreamWriter(stream, Encoding.ASCII) { AutoFlush = true };

            // إرسال رسالة الترحيب
            writer.WriteLine("220 Welcome to FTP Server");

            try
            {
                while (true)
                {
                    string command = reader.ReadLine();
                    if (command == null) break;

                    string[] commandParts = command.Split(' ', 2);
                    string cmd = commandParts[0].ToUpper();
                    string argument = commandParts.Length > 1 ? commandParts[1] : null;

                    switch (cmd)
                    {
                        case "USER":
                            writer.WriteLine("331 Username okay, need password.");
                            break;

                        case "PASS":
                            writer.WriteLine("230 User logged in, proceed.");
                            break;

                        case "LIST":
                            writer.WriteLine("150 Here comes the directory listing.");
                            // Here you would send the directory listing
                            writer.WriteLine("226 Directory send OK.");
                            break;

                        case "RETR":
                            if (argument != null)
                            {
                                writer.WriteLine("150 Opening data connection for file transfer.");
                                // Send the file content here (if it's a valid file)
                                writer.WriteLine("226 Transfer complete.");
                            }
                            break;

                        case "STOR":
                            if (argument != null)
                            {
                                writer.WriteLine("150 Opening data connection for file upload.");
                                // Receive the file here
                                writer.WriteLine("226 Transfer complete.");
                            }
                            break;

                        default:
                            writer.WriteLine("502 Command not implemented.");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                client.Close();
            }
        }

        public void Stop()
        {
            _isRunning = false;
            _listener.Stop();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            FTPServer server = new FTPServer("127.0.0.1", 21);
            server.Start();
        }
    }
}
