using ProtoBuf;

namespace HSGomoku.Network.Messages
{
    [ProtoContract]
    public class ClientLeaveMessage : GameMessage
    {
        public ClientLeaveMessage()
        {
            MsgCode = MsgCode.ClientLeave;
            Content = "Client Leave";
        }
    }
}