using System;

using HSGomoku.Network;
using HSGomoku.Network.Messages;
using HSGomoku.Network.Utils;

using Lidgren.Network;

namespace HSGomoku.Client
{
    public class Program
    {
        private static void Main(String[] args)
        {
            NetworkClient client = new NetworkClient();
            client.Start();
            //client.Connect();
            client.DiscoverPeers();

            NetClient netClient = client.NetClient;
            var algo = new NetXtea(netClient, NetworkSetting.Encryptionkey);

            while (!Console.KeyAvailable || Console.ReadKey().Key != ConsoleKey.Escape)
            {
                NetIncomingMessage msg;
                while ((msg = netClient.ReadMessage()) != null)
                {
                    msg.Decrypt(algo);

                    switch (msg.MessageType)
                    {
                        case NetIncomingMessageType.DiscoveryResponse:
                            Console.WriteLine("Found server at " + msg.SenderEndPoint + " name: " + msg.ReadString());
                            client.Connect(msg.SenderEndPoint);
                            break;

                        case NetIncomingMessageType.VerboseDebugMessage:
                        case NetIncomingMessageType.DebugMessage:
                        case NetIncomingMessageType.WarningMessage:
                        case NetIncomingMessageType.ErrorMessage:
                            Console.WriteLine(msg.ReadString());
                            break;

                        case NetIncomingMessageType.StatusChanged:
                            NetConnectionStatus status = (NetConnectionStatus)msg.ReadByte();
                            if (status == NetConnectionStatus.Connected)
                            {
                                Console.WriteLine(" connected to" + msg.SenderConnection.RemoteUniqueIdentifier);
                            }
                            break;

                        case NetIncomingMessageType.Data:
                            var data = msg.Data;
                            var message = SerializeTools.Deserialize<GameMessage>(data);
                            Console.WriteLine(message.ClientId + "-" + message.Content + "-" + (Int32)message.MsgCode);
                            client.SendMessage(client.CreateGameMessage<HelloMessage>());
                            break;
                    }
                    netClient.Recycle(msg);
                }
            }

            client.Shutdown();
        }
    }
}