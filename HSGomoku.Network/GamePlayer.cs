using System;

using ProtoBuf;

namespace HSGomoku.Network
{
    [ProtoContract]
    public class GamePlayer
    {
        [ProtoMember(1)]
        public String id;

        [ProtoMember(2)]
        public Int32 chessType;

        public GamePlayer Clone()
        {
            return base.MemberwiseClone() as GamePlayer;
        }
    }
}