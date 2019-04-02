using System;

using HSGomoku.Network;
using HSGomoku.Network.Messages;

namespace HSGomoku.Server
{
    public class Program
    {
        private static void Main(String[] args)
        {
            NetworkServer server2 = new NetworkServer();
            server2.Start();
            GameMessage msg2;

            while (true)
            {
                var line = Console.ReadLine();
                if (line == "send")
                {
                    msg2 = server2.CreateGameMessage<HelloMessage>();
                    server2.SendMessage(msg2);
                }
                if (line == "exit")
                {
                    break;
                }
            }
            server2.Shutdown();
        }
    }
}