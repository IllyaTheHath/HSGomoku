using System;

using HSGomoku.Network;

namespace HSGomoku.Server
{
    public class Program
    {
        private static void Main(String[] args)
        {
            String address = "127.0.0.1";
            Int32 port = 13459;
            NetworkServer server = NetworkFactory.CreateNetworkServer();
            server.StartListen(address, port);
            //AsyncTCPServer server = new AsyncTCPServer();
            //server.Start();
            //Console.ReadLine();
        }
    }
}