using System;

using ProtoBuf;

namespace HSGomoku.Network.Messages
{
    [ProtoContract]
    public class GamePlayerMessage : GameMessage
    {
        [ProtoMember(1)]
        public GamePlayer player;

        public GamePlayerMessage()
        {
        }

        public GamePlayerMessage(Int32 code, GamePlayer player)
        {
            this.stateCode = code;
            this.player = player;
        }
    }
}