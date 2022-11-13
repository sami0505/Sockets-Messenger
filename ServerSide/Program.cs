using System;
using System.IO;
using System.Text;
using System.Net.Sockets;

namespace ServerSide
{
    class Program
    {
        static void Main(string[] args)
        {
            // Creates TCP listener, and opens port 6778 for communication.

            TcpListener listener = new TcpListener(System.Net.IPAddress.Any, 6778);
            listener.Start();

            while (true)
            {
                // Waits for and accepts client, then establishes bi-directional r/w stream.

                Console.WriteLine("\nWaiting for Connection...");
                TcpClient client = listener.AcceptTcpClient();
                clearLine();
                Console.WriteLine("Client Accepted.");

                NetworkStream stream = client.GetStream();
                StreamWriter sw = new StreamWriter(client.GetStream());

                // Receives, decodes and displays message. Allows for response, then logs.

                try
                {
                    byte[] buffer = new byte[1024];
                    stream.Read(buffer, 0, buffer.Length);
                    string message = decode(buffer);

                    clearLine();
                    Console.WriteLine($"Message Received: \"{message}\"\nResponse: ");
                    string response = Console.ReadLine();
                    sw.WriteLine(response);
                    sw.Flush();

                    string IPAddress = client.Client.RemoteEndPoint.ToString();
                    log(IPAddress, message, response);
                }

                // If error occurs, it makes the client and server aware.

                catch(Exception e)
                {
                    Console.WriteLine("Something Went Wrong..");
                    sw.WriteLine(e.ToString());
                }
            }
        }

        // Takes in ASCII characters encoded in byte array, and returns original decoded string. 

        static string decode(byte[] buffer)
        {
            int len = 0;
            
            foreach(byte b in buffer)
            {
                if(b != 0)
                {
                    len++;
                }
            }
            return Encoding.UTF8.GetString(buffer, 0, len);
        }

        // Defines current dateTime, path of logfile, and content to write on log.

        static void log(string IPAdd, string message, string response)
        {
            DateTime dt = DateTime.Now;
            string logPath = $"{dt.ToString("dd-MM-yyyy")}_log.txt";
            string line = $"Interaction at {dt.ToShortTimeString()}:\n\tOrigin: {IPAdd}\n\tMessage: \"{message}\"\n\tResponse: \"{response}\" \n\n";
            
            File.AppendAllText(logPath,line);
        }
        
        // Deletes last line and next output will be typed on the same line.
        
        static void clearLine()
        {
            string empty = new String(' ',Console.BufferWidth);
            Console.SetCursorPosition(0, Console.CursorTop-1);
            Console.WriteLine(empty);
            Console.SetCursorPosition(0, Console.CursorTop-1);
        }
    }
}
