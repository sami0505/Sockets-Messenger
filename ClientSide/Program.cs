using System;
using System.IO;
using System.Text;
using System.Net.Sockets;

namespace ClientSide
{
    class Program
    {
        static void Main(string[] args)
        {
            const string HOST_ADDRESS = "192.168.1.245";

            while (true)
            {
                try
                {   
                    // Establishes TCP client connection, and gets payload.

                    TcpClient client = new TcpClient(HOST_ADDRESS , 6778);
                    Console.WriteLine("Type in the message you would like to send.");
                    string messageToSend = Console.ReadLine();

                    int byteCount = Encoding.ASCII.GetByteCount(messageToSend + 1);
                    byte[] buffer = new byte[byteCount];
                    buffer = Encoding.ASCII.GetBytes(messageToSend);

                    NetworkStream stream = client.GetStream();
                    stream.Write(buffer, 0 , buffer.Length);
                    Console.WriteLine("Sent. Awaiting response from Server...");

                    StreamReader sr = new StreamReader(stream);
                    string response = sr.ReadLine();
                    clearLine();
                    Console.WriteLine($"Response: \"{response}\"");

                    stream.Close();
                    client.Close();
                }
                
                catch (Exception)
                {
                    Console.WriteLine($"Connection Failed...");
                }


                Console.WriteLine("Do you wish to try again? true/false: ");
                bool choice = Convert.ToBoolean(Console.ReadLine().ToLower());
                
                if (choice == false)
                {
                    break;
                }
                clearLine();
                clearLine();
                clearLine();
            }
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
