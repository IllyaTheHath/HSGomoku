using System;

using ProtoBuf;

namespace HSGomoku.Network.Messages
{
    public enum MsgCode
    {
        UserJoin = 1,
        UserLeave = 2,
        NewRound = 10,
        ClientJoin = 100,
        ClientLeave = 101,
        ServerShutdown = 1000,
        Hello = 10000
    }

    [ProtoContract]
    [ProtoInclude(5, typeof(ClientJoinMessage))]
    [ProtoInclude(6, typeof(ClientLeaveMessage))]
    [ProtoInclude(10, typeof(ServerShutdownMessage))]
    public class GameMessage
    {
        [ProtoMember(1)]
        public MsgCode MsgCode { get; set; }

        [ProtoMember(2)]
        public String Content { get; set; }
    }
}