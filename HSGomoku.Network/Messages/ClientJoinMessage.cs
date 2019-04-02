using System;

namespace HSGomoku.Network.Messages
{
    [Serializable]
    public sealed class ClientJoinMessage : GameMessage
    {
        public ClientJoinMessage()
        {
            MsgCode = MsgCode.ClientJoin;
            Content = "Client Join";
        }
    }
}