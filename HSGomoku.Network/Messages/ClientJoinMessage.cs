using ProtoBuf;

namespace HSGomoku.Network.Messages
{
    [ProtoContract]
    public class ClientJoinMessage : GameMessage
    {
        public ClientJoinMessage()
        {
            MsgCode = MsgCode.ClientJoin;
            Content = "Client Join";
        }
    }
}