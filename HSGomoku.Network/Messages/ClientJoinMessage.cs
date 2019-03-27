using System;

using ProtoBuf;

namespace HSGomoku.Network.Messages
{
    [ProtoContract]
    public class ClientJoinMessage : GameMessage
    {
        public ClientJoinMessage()
        {
            this.stateCode = MsgCode.ClientJoin;
        }

        [ProtoMember(1)]
        public String id;
    }
}