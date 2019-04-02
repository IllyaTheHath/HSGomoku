using System;

namespace HSGomoku.Network.Messages
{
    [Serializable]
    public sealed class PlayerSurrenderMessage : GameMessage
    {
        public PlayerSurrenderMessage()
        {
            MsgCode = MsgCode.PlayerSurrender;
            Content = "Player Surrender";
        }
    }
}