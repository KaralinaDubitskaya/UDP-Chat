using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UDP_Chat__Server_
{
    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server();
            server.Listen();

            // Close if ESC key is pressed
            while (Console.ReadKey().Key != ConsoleKey.Escape) { }
        }
    }
}
