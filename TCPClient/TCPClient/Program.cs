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
// Assignment:      Hw 4 - ChatRoom

namespace TCPClient
{
    class Program
    {

        // This function will create a byte array to make sure that client is not overloaded,
        // and will get the stream from the client. Read the messagge that was received from other clients.
        static void ReadData(object socket)
        {
            TcpClient client = (TcpClient)socket;
            while(true)
            {
                byte[] buffer = new byte[1024];
                int k = client.GetStream().Read(buffer, 0, buffer.Length);
                Console.WriteLine("Messagge from {0}: ", Encoding.ASCII.GetString(buffer, 0, k));
            }
        }
        static void Main(string[] args)
        {
            TcpClient client = new TcpClient();
            client.Connect(IPAddress.Loopback, 5000);
            Thread t = new Thread(ReadData);
            t.Start(client);
            Console.WriteLine("Enter your username: ");
            string input = Console.ReadLine();

            // If the user doesn't enter any username, create a random username by concatenating:
            // the abbreviation of the current month, hour, minute seconds
            // a random cartoon character
            // and a random number from 0 to 100.
            if(input == "")
            {
                DateTime now = DateTime.Now;

                string[] cartoonCharacters = { "Simba", "Popey", "BugsBunny", "Goku", "MickeyMouse",
                    "HomerSimpson", "FredFlinstone", "Scooby-Doo", "WinnieThePooh", "Tom",
                    "StewieGriffin", "Jerry", "SpongeBob" };
                Random rnd = new Random();

                int rndValue = rnd.Next(0, cartoonCharacters.Length);
                int rndValue2 = rnd.Next(0, 100);

                input = now.ToString("MMMHHmmss") + cartoonCharacters[rndValue] + rndValue2;

            }


            byte[] message = Encoding.ASCII.GetBytes(input);
            client.GetStream().Write(message, 0, message.Length);

            // This will send the messagge to the server and it will be received by other clients in the 't' thread (ReadData)
            while (true)
            {
                Console.WriteLine("Press 'Enter' to send the following message: ");
                input = Console.ReadLine();
                message = Encoding.ASCII.GetBytes(input);         
                client.GetStream().Write(message, 0, message.Length);                
            }
        }
    }
}
