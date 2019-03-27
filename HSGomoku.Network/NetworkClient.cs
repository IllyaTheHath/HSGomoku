using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

using System.Threading;

using HSGomoku.Network.Utils;

namespace HSGomoku.Network
{
    public class NetworkClient
    {
        private readonly Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private BinaryReader reader;
        private BinaryWriter writer;

        public event Action<GameMessage> MessageHandler;

        private Thread _recieveThread = null;

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
                        Int32 length = this.reader.ReadInt32();
                        var data = this.reader.ReadBytes(length);
                        var msg = ProtoBufTools.Deserialize<GameMessage>(data);
                        MessageHandler?.Invoke(msg);
                    }
                }
                catch
                {
                }
            });
            this._recieveThread.Start();
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
            this._recieveThread.Abort();
            this.clientSocket.Close();
        }
    }
}