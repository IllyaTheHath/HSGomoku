using System;

using ProtoBuf;

namespace HSGomoku.Network.Messages
{
    [ProtoContract]
    public class ClientLeaveMessage : GameMessage
    {
        public ClientLeaveMessage()
        {
            this.stateCode = MsgCode.ClientLeave;
        }

        [ProtoMember(1)]
        public String id;
    }
}