using System;

namespace HSGomoku.Network
{
    public static class NetworkSetting
    {
        public static String AppIdentifier => "HSGomoku.Network";

        public static String ServerName => "HSGomoku Server";

        public static String IpAddress => "10.129.12.33";

        public static Int32 Port => 13459;

        public static Int32 MaxConnectClient => 2;

        public static String Encryptionkey => "HSGomoku.Network.Key";
    }
}