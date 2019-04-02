using System;

namespace HSGomoku.Network.Messages
{
    [Serializable]
    public sealed class ServerShutdownMessage : GameMessage
    {
        public ServerShutdownMessage()
        {
            MsgCode = MsgCode.ServerShutdown;
            Content = "Server Shutdown";
        }
    }
}