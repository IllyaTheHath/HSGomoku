using System;

using HSGomoku.Network.Messages;

using ProtoBuf;

namespace HSGomoku.Network
{
    [ProtoContract]
    [ProtoInclude(3, typeof(GamePlayerMessage))]
    [ProtoInclude(4, typeof(ClientJoinMessage))]
    [ProtoInclude(5, typeof(ClientLeaveMessage))]
    public class GameMessage
    {
        [ProtoMember(1)]
        public Int32 stateCode;

        [ProtoMember(2)]
        public String content;
    }
}