using System;

namespace HSGomoku.Network.Messages
{
    [Serializable]
    public sealed class ServerRespondJoinMessage : GameMessage
    {
        public ServerRespondJoinMessage()
        {
            MsgCode = MsgCode.ServerRespondJoin;
            Content = "Server Respond Join";
        }
    }
}