using System;

namespace HSGomoku.Network
{
    public static class NetworkSetting
    {
        public static String AppIdentifier => "HSGomoku.Network";

        public static String ServerName => "HSGomoku Server";

        public static String IpAddress => "127.0.0.1";

        public static Int32 Port => 13459;

        public static Int32 MaxConnectClient => 2;

        public static String Encryptionkey => "HSGomoku.Network.Key";
    }
}