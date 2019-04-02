using System;

using HSGomoku.Network;
using HSGomoku.Network.Messages;

namespace HSGomoku.Engine.Network
{
    public class Client
    {
        private readonly NetworkClient _client;

        public event Action<GameMessage> OnMessage;

        public Client()
        {
            this._client = new NetworkClient();
            this._client.MessageHandler += OnMessage;
        }

        public void Connect()
        {
            if (!this._client?.Connected ?? false)
            {
                this._client?.Connect(NetworkSetting.IpAddress, NetworkSetting.Port);
            }
        }

        public void SendMessage(GameMessage msg)
        {
            this._client?.Send(msg);
        }

        public void Shutdown()
        {
            this._client?.Send(new ClientLeaveMessage());
            this._client?.Close(true);
        }
    }
}