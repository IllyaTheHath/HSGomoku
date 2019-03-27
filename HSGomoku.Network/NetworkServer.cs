using System;
using System.Net;
using System.Net.Sockets;

using System.Threading;

namespace HSGomoku.Network
{
    public class NetworkServer
    {
        private TcpListener _listener;
        private Thread _listenThread;

        public event Action<NetworkClientSession> OnNewClient;

        public event Action<GameMessage> OnNewMessage;

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
                        var client = this._listener.AcceptTcpClient();
                        NetworkClientSession session = new NetworkClientSession(client);
                        session.GetRemoteConnectInf(out String saddress, out Int32 sport);
                        Console.WriteLine($"New Client Connected:{saddress}:{sport}");
                        session.MessageHandler += OnNewMessage ?? null;
                        OnNewClient?.Invoke(session);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            });
            this._listenThread.Start();
        }

        public void Shutdown()
        {
            this._listener.Stop();
            this._listenThread.Abort();
        }
    }
}