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
                            Console.WriteLine(msg.SenderConnection.RemoteUniqueIdentifier + " connected!");
                        }
                        else if (status == NetConnectionStatus.Disconnected)
                        {
                            Console.WriteLine(msg.SenderConnection.RemoteUniqueIdentifier + " disconnected!");
                        }
                        break;

                    case NetIncomingMessageType.Data:
                        var data = msg.Data;
                        var message = SerializeTools.Deserialize<GameMessage>(data);
                        Console.WriteLine(message.ClientId + "-" + message.Content + "-" + (Int32)message.MsgCode);
                        break;
                }
            }

            p.Recycle(msg);
        }

        public void Start()
        {
            this._server.Start();
        }

        public GameMessage CreateGameMessage<T>() where T : GameMessage, new()
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

            if (client is null)
            {
                this._server.Connections.ForEach((c) =>
                {
                    this._server.SendMessage(om, c, NetDeliveryMethod.ReliableOrdered);
                });
            }
            else
            {
                this._server.SendMessage(om, client, NetDeliveryMethod.ReliableOrdered);
            }
        }

        public void Shutdown(String bye = null)
        {
            this._server.Shutdown(bye);
        }

        public void DecryptMessage(NetIncomingMessage msg)
        {
            msg.Decrypt(this._algo);
        }
    }
}