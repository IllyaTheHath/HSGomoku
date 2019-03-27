namespace HSGomoku.Network
{
    public static class NetworkFactory
    {
        public static NetworkServer CreateNetworkServer()
        {
            return new NetworkServer();
        }

        public static NetworkClient CreateNetworkClient()
        {
            return new NetworkClient();
        }
    }
}