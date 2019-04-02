using System;

namespace HSGomoku.Network.Messages
{
    [Serializable]
    public sealed class ClientLeaveMessage : GameMessage
    {
        public ClientLeaveMessage()
        {
            MsgCode = MsgCode.ClientLeave;
            Content = "Client Leave";
        }
    }
}