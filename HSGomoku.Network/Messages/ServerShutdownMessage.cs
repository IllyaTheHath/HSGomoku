namespace HSGomoku.Network.Messages
{
    internal class ServerShutdownMessage : GameMessage
    {
        public ServerShutdownMessage()
        {
            MsgCode = MsgCode.ServerShutdown;
            Content = "Server Shutdown";
        }
    }
}