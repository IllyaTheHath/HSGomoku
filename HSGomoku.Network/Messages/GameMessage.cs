using System;
using System.Collections.Generic;

namespace HSGomoku.Network.Messages
{
    public enum MsgCode
    {
        EmptyMessage = 2,

        ClientJoin = 10,
        ClientLeave = 11,
        ClientMatch = 12,
        ClientMatchSuccess = 13,

        GameStart = 20,
        GameEnd = 21,

        PlayerPlaceChess = 30,
        PlayerSurrender = 31,

        ServerRespondJoin = 100,
        ServerShutdown = 101,
        Hello = 1000
    }

    [Serializable]
    public abstract class GameMessage
    {
        public MsgCode MsgCode { get; protected set; } = MsgCode.EmptyMessage;

        public String Content { get; set; } = String.Empty;

        public Int64 ClientId { get; set; }

        public Dictionary<String, Object> ExtraData { get; set; }
    }
}