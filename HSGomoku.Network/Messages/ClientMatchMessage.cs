using System;

namespace HSGomoku.Network.Messages
{
    [Serializable]
    public sealed class ClientMatchMessage : GameMessage
    {
        public ClientMatchMessage()
        {
            MsgCode = MsgCode.ClientMatch;
            Content = "Client Matching";
        }
    }
}