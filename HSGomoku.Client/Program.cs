﻿using System;

using HSGomoku.Network;

namespace HSGomoku.Client
{
    public class Program
    {
        private static void Main(String[] args)
        {
            String address = "127.0.0.1";
            Int32 port = 13459;
            NetworkClient client = NetworkFactory.CreateNetworkClient();
            client.Connect(address, port);
            //AsyncTCPClient client = new AsyncTCPClient();
            //client.AsynConnect();
            //Console.ReadLine();
        }
    }
}