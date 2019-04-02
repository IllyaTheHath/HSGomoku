using System;
using System.Net;

using HSGomoku.Network.Messages;
using HSGomoku.Network.Utils;

using Lidgren.Network;

namespace HSGomoku.Network
{
    public class NetworkClient
    {
        private readonly NetPeerConfiguration _config;
        private readonly NetClient _client;
        private readonly NetEncryption _algo;

        public NetClient NetClient { get { return this._client; } }

        public NetworkClient()
        {
            this._config = new NetPeerConfiguration(NetworkSetting.AppIdentifier);
            //this._config.LocalAddress = IPAddress.Parse(NetworkSetting.IpAddress);

            this._config.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);

            this._client = new NetClient(this._config);

            this._algo = new NetXtea(this._client, NetworkSetting.Encryptionkey);
        }

        public void Start()
        {
            this._client.Start();
            //this._client.DiscoverLocalPeers(NetworkSetting.Port);
        }

        public void DiscoverPeers()
        {
            this._client.DiscoverKnownPeer(NetworkSetting.IpAddress, NetworkSetting.Port);
        }

        public void Connect()
        {
            NetOutgoingMessage approval = this._client.CreateMessage();
            approval.Write(NetworkSetting.Encryptionkey);
            this._client.Connect(NetworkSetting.IpAddress, NetworkSetting.Port, approval);
        }

        public void Connect(IPEndPoint remoteEndPoint)
        {
            NetOutgoingMessage approval = this._client.CreateMessage();
            approval.Write(NetworkSetting.Encryptionkey);
            this._client.Connect(remoteEndPoint, approval);
        }

        public GameMessage CreateGameMessage<T>() where T : GameMessage, new()
        {
            T t = new T();
            t.ClientId = this._client.UniqueIdentifier;
            return t;
        }

        public void ResponseDiscovery(IPEndPoint recipient)
        {
            NetOutgoingMessage response = this._client.CreateMessage();
            response.Write(NetworkSetting.ServerName);
            response.Encrypt(this._algo);

            this._client.SendDiscoveryResponse(response, recipient);
        }

        /// <summary>
        /// Send Message To Client
        /// </summary>
        /// <param name="msg">GameMessage</param>
        /// <param name="client">Client we want to send. Send to all connected client if it's null</param>
        public void SendMessage(GameMessage msg)
        {
            NetOutgoingMessage om = this._client.CreateMessage();

            var b = SerializeTools.Serialize(msg);
            om.Write(b);
            om.Encrypt(this._algo);

            this._client.Connections.ForEach((c) =>
            {
                this._client.SendMessage(om, c, NetDeliveryMethod.ReliableOrdered);
            });
        }

        public void Shutdown(String bye = null)
        {
            this._client.Disconnect(bye);
            this._client.Shutdown(bye);
        }
    }
}