using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace HSGomoku.Network
{
    public static class NetworkSetting
    {
        public static String AppIdentifier => "HSGomoku.Network";

        public static String ServerName => "HSGomoku Server";

        public static String ServerIpAddress => "34.80.43.9";

        public static String LocalIpAddress
        {
            get
            {
                return (from ip in Dns.GetHostAddresses(Dns.GetHostName())
                        where ip.AddressFamily == AddressFamily.InterNetwork
                        select ip.ToString()).FirstOrDefault();
            }
        }

        public static Int32 Port => 13459;

        public static String Encryptionkey => "HSGomoku.Network.Key";
    }
}