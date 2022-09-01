using StackExchange.Redis;

namespace Drinktionary.Cache;

public class ConnectionHelper
{
    private static readonly Lazy<ConnectionMultiplexer> _lazyConnection = new(() => ConnectionMultiplexer.Connect(ConfigurationManager.AppSetting["RedisURL"]));

    public static ConnectionMultiplexer Connection
    {
        get
        {
            return _lazyConnection.Value;
        }
    }
}