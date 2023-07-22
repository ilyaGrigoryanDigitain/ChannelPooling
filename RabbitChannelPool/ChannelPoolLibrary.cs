using RabbitMQ.Client;

namespace RabbitChannelPool;

public class ChannelPoolLibrary : IDisposable
{
    private readonly IConnectionFactory _connectionFactory;
    private readonly string _exchangeName;
    private readonly int _channelPoolSize;
    private readonly List<IModel> _channelPool;
    private object _poolLock = new object();

    public ChannelPoolLibrary(string hostname, string username, string password, string exchangeName, int channelPoolSize)
    {
        _connectionFactory = new ConnectionFactory
        {
            HostName = hostname,
            UserName = username,
            Password = password
        };

        _exchangeName = exchangeName;
        _channelPoolSize = channelPoolSize;
        _channelPool = new List<IModel>();
    }
    private IModel CreateChannel()
    {
        var connection = _connectionFactory.CreateConnection();
        var channel = connection.CreateModel();
        channel.ExchangeDeclare(_exchangeName, ExchangeType.Direct);
        return channel;
    }

    public IModel GetChannel()
    {
        lock (_poolLock)
        {
            if (_channelPool.Count < _channelPoolSize)
            {
                var channel = CreateChannel();
                _channelPool.Add(channel);
            }

            var channelToUse = _channelPool[^1];
            _channelPool.RemoveAt(_channelPool.Count - 1);
            return channelToUse;
        }
    }

    public void ReleaseChannel(IModel channel)
    {
        lock (_poolLock)
        {
            _channelPool.Add(channel);
        }
    }

    public void Dispose()
    {
    }
}
