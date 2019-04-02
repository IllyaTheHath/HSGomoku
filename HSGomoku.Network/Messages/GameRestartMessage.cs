using System;

namespace HSGomoku.Network.Messages
{
    [Serializable]
    public sealed class GameRestartMessage : GameMessage
    {
        public GameRestartMessage()
        {
            MsgCode = MsgCode.GameRestart;
            Content = "Game Restart";
        }
    }
}