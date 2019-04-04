using System;
using System.Net;
using System.Threading;

using HSGomoku.Network.Messages;
using HSGomoku.Network.Utils;

using Lidgren.Network;

namespace HSGomoku.Network
{
    public class NetworkServer
    {
        private readonly NetPeerConfiguration _config;
        private readonly NetServer _server;
        private readonly NetEncryption _algo;

        public event Action<GameMessage> OnGameMessage;

        public NetServer NetServer { get { return this._server; } }

        public NetworkServer()
        {
            this._config = new NetPeerConfiguration(NetworkSetting.AppIdentifier);
            this._config.LocalAddress = IPAddress.Parse(NetworkSetting.IpAddress);
            this._config.Port = NetworkSetting.Port;

            this._config.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);
            this._config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);

            this._server = new NetServer(this._config);
            // create the Synchronization Context
            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
            this._server.RegisterReceivedCallback(new SendOrPostCallback(OnMessage));

            this._algo = new NetXtea(this._server, NetworkSetting.Encryptionkey);
        }

        public void OnMessage(Object peer)
        {
            var p = peer as NetPeer;
            NetIncomingMessage msg;

            while ((msg = p.ReadMessage()) != null)
            {
                msg.Decrypt(this._algo);

                switch (msg.MessageType)
                {
                    case NetIncomingMessageType.DiscoveryRequest:
                        ResponseDiscovery(msg.SenderEndPoint);
                        break;

                    case NetIncomingMessageType.ConnectionApproval:
                        String s = msg.ReadString();
                        if (s == NetworkSetting.Encryptionkey)
                        {
                            msg.SenderConnection.Approve();
                        }
                        else
                        {
                            msg.SenderConnection.Deny();
                        }
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
                            Console.WriteLine(NetUtility.ToHexString(msg.SenderConnection.RemoteUniqueIdentifier) + " connected!");
                        }
                        else if (status == NetConnectionStatus.Disconnected)
                        {
                            Console.WriteLine(NetUtility.ToHexString(msg.SenderConnection.RemoteUniqueIdentifier) + " disconnected!");
                        }
                        break;

                    case NetIncomingMessageType.Data:
                        try
                        {
                            var data = msg.Data;
                            var message = SerializeTools.Deserialize<GameMessage>(data);
                            Console.ForegroundColor = ConsoleColor.DarkMagenta;
                            Console.WriteLine($"Receive Message From {NetUtility.ToHexString(message.ClientId)}::MsgCode:{(Int32)message.MsgCode},Content:{message.Content}");
                            Console.ResetColor();
                            OnGameMessage?.Invoke(message);
                        }
                        catch (Exception ex)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(ex);
                            Console.ResetColor();
                        }
                        break;
                }
            }

            p.Recycle(msg);
        }

        public void Start()
        {
            this._server.Start();
        }

        public T CreateGameMessage<T>() where T : GameMessage, new()
        {
            T t = new T();
            t.ClientId = this._server.UniqueIdentifier;
            return t;
        }

        public void ResponseDiscovery(IPEndPoint recipient)
        {
            NetOutgoingMessage response = this._server.CreateMessage();
            response.Write(NetworkSetting.ServerName);
            response.Encrypt(this._algo);

            this._server.SendDiscoveryResponse(response, recipient);
        }

        public NetConnection GetClient(Int64 clientId)
        {
            return this._server.Connections.Find(con => con.RemoteUniqueIdentifier == clientId);
        }

        /// <summary>
        /// Send Message To Client
        /// </summary>
        /// <param name="msg">GameMessage</param>
        /// <param name="client">Client we want to send. Send to all connected client if it's null</param>
        public void SendMessage(GameMessage msg, NetConnection client = null)
        {
            NetOutgoingMessage om = this._server.CreateMessage();

            var b = SerializeTools.Serialize(msg);
            om.Write(b);
            om.Encrypt(this._algo);

            Console.ForegroundColor = ConsoleColor.Cyan;
            var clientId = client is null ? "All" : NetUtility.ToHexString(client.RemoteUniqueIdentifier);
            Console.WriteLine($"  Send  Message  To  {clientId}::MsgCode:{(Int32)msg.MsgCode},Content:{msg.Content}");
            Console.ResetColor();
            this._server.SendMessage(om, client, NetDeliveryMethod.ReliableOrdered);
        }

        public void Shutdown(String bye = null)
        {
            var msg = CreateGameMessage<ServerShutdownMessage>();
            SendMessage(msg);
            this._server.Shutdown(bye);
        }
    }
}