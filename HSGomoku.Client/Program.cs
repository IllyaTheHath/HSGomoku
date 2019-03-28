using System;

using HSGomoku.Network;
using HSGomoku.Network.Messages;

namespace HSGomoku.Client
{
    public class Program
    {
        private static void Main(String[] args)
        {
            String address = "127.0.0.1";
            Int32 port = 13459;
            NetworkClient client = new NetworkClient();
            client.MessageHandler += (message) =>
            {
                if (message.MsgCode != MsgCode.ServerShutdown)
                {
                    Console.WriteLine($"Server Said:msgCode={message.MsgCode},content={message.Content}");
                    Console.WriteLine("Send Hello Back To Server");
                    client.Send(new GameMessage() { MsgCode = MsgCode.Hello, Content = "Hello Back From Client" });
                }
                else
                {
                    client.Close(false);
                }
            };
            client.Connect(address, port);
            Console.ReadLine();
            client.Close(true);
        }
    }
}