using System;

namespace HSGomoku.Network.Messages
{
    [Serializable]
    public sealed class ClientMatchSuccessMessage : GameMessage
    {
        public ClientMatchSuccessMessage()
        {
            MsgCode = MsgCode.ClientMatchSuccess;
            Content = "Client Match Success";
        }
    }
}