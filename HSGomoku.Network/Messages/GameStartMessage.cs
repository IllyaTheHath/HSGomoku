using System;

namespace HSGomoku.Network.Messages
{
    [Serializable]
    public sealed class GameStartMessage : GameMessage
    {
        public GameStartMessage()
        {
            MsgCode = MsgCode.GameStart;
            Content = "Game Start";
        }
    }
}