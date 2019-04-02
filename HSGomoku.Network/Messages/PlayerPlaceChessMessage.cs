using System;

namespace HSGomoku.Network.Messages
{
    [Serializable]
    public sealed class PlayerPlaceChessMessage : GameMessage
    {
        public PlayerPlaceChessMessage()
        {
            MsgCode = MsgCode.PlayerPlaceChess;
            Content = "Player Place Chess";
        }
    }
}