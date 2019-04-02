using System;

namespace HSGomoku.Network.Messages
{
    [Serializable]
    public sealed class HelloMessage : GameMessage
    {
        public HelloMessage()
        {
            MsgCode = MsgCode.Hello;
            Content = "Hello";
        }
    }
}