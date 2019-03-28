using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

using System.Threading;

using HSGomoku.Network.Messages;
using HSGomoku.Network.Utils;

namespace HSGomoku.Network
{
    public class NetworkClientSession
    {
        private readonly TcpClient client;
        private readonly BinaryWriter writer;
        private BinaryReader reader;

        public Int32 Id { get; }

        public event Action<GameMessage> MessageHandler;

        private readonly Thread _recieveThread;
        private Boolean _threadAbort;

        public NetworkClientSession()
        {
            this._threadAbort = false;
        }

        public Boolean Connected
        {
            get
            {
                return this.client.Connected;
            }
        }

        public NetworkClientSession(TcpClient client, Int32 id)
        {
            this.client = client;
            Id = id;
            this.writer = new BinaryWriter(client.GetStream());
            this.reader = new BinaryReader(client.GetStream());

            this._recieveThread = new Thread((o) =>
            {
                try
                {
                    while (true)
                    {
                        if (this._threadAbort)
                        {
                            break;
                        }

                        Int32 length = this.reader.ReadInt32();
                        var data = this.reader.ReadBytes(length);
                        var msg = ProtoBufTools.Deserialize<GameMessage>(data);
                        MessageHandler(msg);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            });
            this._recieveThread.Start();
        }

        public void GetRemoteConnectInf(out String address, out Int32 port)
        {
            var remoteIpEndPoint = this.client.Client.RemoteEndPoint as IPEndPoint;
            address = remoteIpEndPoint.Address.ToString();
            port = remoteIpEndPoint.Port;
        }

        public void Send(GameMessage msg)
        {
            if (msg == null)
            {
                return;
            }

            var data = ProtoBufTools.Serialize(msg);
            if (data.Length == 0)
            {
                return;
            }

            this.writer.Write(data.Length);
            this.writer.Write(data);
        }

        public void Close()
        {
            this.client.Close();
            this._threadAbort = true;
        }
    }
}