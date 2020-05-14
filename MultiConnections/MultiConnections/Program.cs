using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Threading;
using System.Net.Sockets;


// Course:          CPSC-24500-002
// Instructor:      Natalie Adams
// Student:         Klead Fusha
// Assignment:      Hw 4 - Chat Room



namespace MultiConnections
{
    class Program
    {
        static Dictionary<string, Socket> sockets = new Dictionary<string, Socket>();
        static void Main(string[] args)
        {
            TcpListener server = new TcpListener(IPAddress.Loopback, 5000);
            server.Start();
            Console.WriteLine("Waiting for a connection...");
            while (true)
            {
                Thread t = new Thread(HandleConnection);
                Socket client = server.AcceptSocket();
                t.Start(client);
            }
            server.Stop();

        }

        static void HandleConnection(object socket)
        {
            Socket client = (Socket)socket;
            Console.WriteLine($"Connection accepted from {client.RemoteEndPoint}");
            byte[] buffer = new byte[1024];
            int k = client.Receive(buffer);
            string userName = Encoding.ASCII.GetString(buffer, 0, k);
            sockets[userName] = client;
            while (true)
            {
                try
                {
                    k = client.Receive(buffer);
                    foreach (KeyValuePair<string, Socket> otherClient in sockets)
                    {
                        if (otherClient.Value != client)
                        {
                            string clientData = $"[{userName}] - " + Encoding.ASCII.GetString(buffer, 0, k);
                            otherClient.Value.Send(Encoding.ASCII.GetBytes(clientData));
                        }
                           
                    }
                    Console.WriteLine("Data received from client: {0}", Encoding.ASCII.GetString(buffer, 0, k));
                }
                catch (Exception e)
                {
                    sockets.Remove(userName);
                    break;
                }
            }
            client.Close();
        }
    }
}
