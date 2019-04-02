using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using HSGomoku.Network.Messages;

namespace HSGomoku.Network.Old
{
    public class NetworkServer
    {
        private TcpListener _listener;

        private Thread _listenThread;
        private Boolean _threadAbort;
        private readonly NetworkClientSession[] _sessions;

        private readonly Boolean[] usedUserID;

        public event Action<NetworkClientSession> OnNewClient;

        public event Action<GameMessage> OnNewMessage;

        public NetworkServer()
        {
            this._sessions = new NetworkClientSession[NetworkSetting.MaxConnectClient];
            this.usedUserID = new Boolean[NetworkSetting.MaxConnectClient];
            this._threadAbort = false;
        }

        public void StartListen(String address, Int32 port)
        {
            IPAddress ip = IPAddress.Parse(address);

            this._listener = new TcpListener(ip, port);
            this._listener.Start();

            this._listenThread = new Thread(() =>
            {
                Console.WriteLine($"Server Started At {address}:{port}");
                try
                {
                    while (true)
                    {
                        if (this._threadAbort)
                        {
                            break;
                        }
                        var client = this._listener.AcceptTcpClient();
                        if (client.Available <= 0)
                        {
                            continue;
                        }

                        Int32 id = -1;
                        for (var i = 0; i < this.usedUserID.Length; i++)
                        {
                            if (this.usedUserID[i] == false)
                            {
                                id = i;
                                break;
                            }
                        }

                        if (id == -1)
                        {
                            Console.WriteLine("Client " + client.Client.RemoteEndPoint.ToString() + " cannot connect. ");
                            client.Close();
                            continue;
                        }

                        this.usedUserID[id] = true;
                        NetworkClientSession session = new NetworkClientSession(client, id);
                        this._sessions[id] = session;
                        session.GetRemoteConnectInf(out String saddress, out Int32 sport);
                        Console.WriteLine($"New Client Connected:{saddress}:{sport}");
                        session.MessageHandler += OnNewMessage ?? null;
                        OnNewClient?.Invoke(session);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            });
            this._listenThread.Start();
        }

        public void Send(GameMessage msg, Int32 id)
        {
            if (id == -1)
            {
                foreach (NetworkClientSession s in this._sessions)
                {
                    if (s != null && s.Connected)
                    {
                        s.Send(msg);
                    }
                }
            }
            else
            {
                foreach (NetworkClientSession s in this._sessions)
                {
                    if (s != null && s.Id == id && s.Connected)
                    {
                        s.Send(msg);
                    }
                }
            }
        }

        public void Shutdown(Boolean sendShutdownMsg)
        {
            if (sendShutdownMsg)
            {
                Send(new ServerShutdownMessage(), -1);
            }

            this._listener.Stop();
            this._threadAbort = true;
        }
    }
}