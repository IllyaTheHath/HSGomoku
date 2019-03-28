using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

using System.Threading;

using HSGomoku.Network.Messages;
using HSGomoku.Network.Utils;

namespace HSGomoku.Network
{
    public class NetworkClient
    {
        private readonly Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private BinaryReader reader;
        private BinaryWriter writer;

        public event Action<GameMessage> MessageHandler;

        private Thread _recieveThread;
        private Boolean _threadAbort;

        public NetworkClient()
        {
            this._threadAbort = false;
        }

        public Boolean Connected
        {
            get
            {
                return this.clientSocket.Connected;
            }
        }

        public void Connect(String address, Int32 port)
        {
            IPAddress ip = IPAddress.Parse(address);

            this.clientSocket.Connect(new IPEndPoint(ip, port));

            this.reader = new BinaryReader(new NetworkStream(this.clientSocket));
            this.writer = new BinaryWriter(new NetworkStream(this.clientSocket));

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
                        MessageHandler?.Invoke(msg);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            });
            this._recieveThread.Start();
            Send(new ClientJoinMessage());
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

        public void Close(Boolean sendLeaveMsg)
        {
            if (sendLeaveMsg)
            {
                Send(new ClientLeaveMessage());
            }

            this.clientSocket.Close();
            this._threadAbort = true;
        }
    }
}