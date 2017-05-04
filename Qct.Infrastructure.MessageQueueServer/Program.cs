using Qct.Infrastructure.MessageServer.Implementations;
using System;

namespace Qct.Infrastructure.MessageServer
{
    class Program
    {
        static void Main(string[] args)
        {
            MQMServer.InitServer();

            Console.WriteLine("Press ENTER to exit the server.");
            Console.ReadLine();
        }
    }
}
