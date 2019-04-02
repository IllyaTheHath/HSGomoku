using System;

namespace HSGomoku.Network.Messages
{
    [Serializable]
    public sealed class GameEndMessage : GameMessage
    {
        public GameEndMessage()
        {
            MsgCode = MsgCode.GameEnd;
            Content = "Game End";
        }
    }
}