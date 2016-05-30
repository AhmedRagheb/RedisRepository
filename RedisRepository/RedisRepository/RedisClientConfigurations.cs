


namespace RedisRepository
{
    public static class RedisClientConfigurations
    {
        private static string _url = ConfigurationHelper.Get("RedisServer", "127.0.0.1");
        public static string Url
        {
            get { return _url; }
            set { _url = value; }
        }

        private static int _port = 6379;
        public static int Port
        {
            get { return _port; }
            set { _port = value; }
        }

        private static int _connectTimeout = 10000;
        public static int ConnectTimeout
        {
            get { return _connectTimeout; }
            set { _connectTimeout = value; }
        }

        private static int _connectRetry = 3;
        public static int ConnectRetry
        {
            get { return _connectRetry; }
            set { _connectRetry = value; }
        }

        private static int _defaultDatabase = Configurations.Get("RedisDataBase", 0);
        public static int DefaultDatabase
        {
            get { return _defaultDatabase; }
            set { _defaultDatabase = value; }
        }

        private static bool _preserveAsyncOrder = false;
        public static bool PreserveAsyncOrder
        {
            get { return _preserveAsyncOrder; }
            set { _preserveAsyncOrder = value; }
        }
    }
}
