using Newtonsoft.Json;
using StackExchange.Redis;

namespace Drinktionary.Cache;

public class CacheService : ICacheService
{
    private readonly IDatabase _database;

    public CacheService()
    {
        _database = ConnectionHelper.Connection.GetDatabase();
    }

    public T? GetData<T>(string key)
    {
        var value = _database.StringGet(key);
        if (!string.IsNullOrEmpty(value))
        {
            return JsonConvert.DeserializeObject<T>(value);
        }

        return default;
    }

    public bool SetData<T>(string key, T value, DateTimeOffset expirationTime)
    {
        TimeSpan expiryTime = expirationTime.DateTime.Subtract(DateTime.Now);
        return _database.StringSet(key, JsonConvert.SerializeObject(value), expiryTime);
    }

    public object RemoveData(string key)
    {
        bool _isKeyExist = _database.KeyExists(key);
        return _isKeyExist ? _database.KeyDelete(key) : (object)false;
    }
}