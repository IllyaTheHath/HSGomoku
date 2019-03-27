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
            server.OnNewClient += (session) =>
            {
                session.GetRemoteConnectInf(out String saddress, out Int32 sport);
                Console.WriteLine("Send Hello Message To Client");
                session.Send(new GameMessage() { stateCode = 1000, content = $"Hello, Friend From {saddress}:{sport}" });
            };
            server.OnNewMessage += (message) =>
            {
                Console.WriteLine($"Recieve Message From Client:stateCode={message.stateCode},content={message.content}");
            };
            server.StartListen(address, port);
            //AsyncTCPServer server = new AsyncTCPServer();
            //server.Start();
            //Console.ReadLine();
        }
    }
}