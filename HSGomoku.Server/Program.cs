using System;
using System.Collections.Generic;
using System.Threading;

using HSGomoku.Network;

namespace HSGomoku.Server
{
    public partial class Program
    {
        private static HashSet<Int64> connected;
        private static HashSet<Int64> matching;
        private static Dictionary<Int64, Int64> matched;

        private static NetworkServer server;

        private static Boolean _quitRequested = false;
        private static readonly Object _syncLock = new Object();
        private static readonly AutoResetEvent _waitHandle = new AutoResetEvent(false);

        private static void Main(String[] args)
        {
            connected = new HashSet<Int64>();
            matching = new HashSet<Int64>();
            matched = new Dictionary<Int64, Int64>();

            server = new NetworkServer();
            server.Start();
            server.OnGameMessage += OnMessage;
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"Server Started at {NetworkSetting.LocalIpAddress}:{NetworkSetting.Port}");
            Console.ResetColor();
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(CurrentDomain_ProcessExit);
            // start the message pumping thread
            Thread msgThread = new Thread(MessagePump);
            msgThread.Start();
            // read input to detect "quit" command
            String command = String.Empty;
            do
            {
                command = Console.ReadLine();
            } while (!"quit".Equals(command, StringComparison.InvariantCultureIgnoreCase));
            // signal that we want to quit
            SetQuitRequested();
            // wait until the message pump says it's done
            _waitHandle.WaitOne();
            // perform any additional cleanup, logging or whatever
        }

        private static void SetQuitRequested()
        {
            lock (_syncLock)
            {
                _quitRequested = true;
            }
        }

        private static void MessagePump()
        {
            do
            {
                // act on messages
            } while (!_quitRequested);
            _waitHandle.Set();
        }

        private static void CurrentDomain_ProcessExit(Object sender, EventArgs e)
        {
            Console.WriteLine("exiting...");
            server.Shutdown();
        }
    }
}