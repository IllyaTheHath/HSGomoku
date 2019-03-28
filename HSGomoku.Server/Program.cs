using System;

using HSGomoku.Network;
using HSGomoku.Network.Messages;

namespace HSGomoku.Server
{
    public class Program
    {
        private static void Main(String[] args)
        {
            String address = "127.0.0.1";
            Int32 port = 13459;
            NetworkServer server = new NetworkServer();
            server.OnNewClient += (session) =>
            {
                session.GetRemoteConnectInf(out String saddress, out Int32 sport);
                Console.WriteLine("Send Hello Message To Client");
                session.Send(new GameMessage() { MsgCode = MsgCode.Hello, Content = $"Hello, Friend From {saddress}:{sport}" });
            };
            server.OnNewMessage += (message) =>
            {
                Console.WriteLine($"Recieve Message From Client:msgCode={message.MsgCode},content={message.Content}");
            };
            server.StartListen(address, port);
            Console.ReadLine();
            server.Send(new GameMessage() { Content = "server" }, -1);
            Console.ReadLine();
            server.Shutdown(true);
        }
    }
}