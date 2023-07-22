using System.Collections.Concurrent;
using RabbitMQ.Client;

namespace RabbitChannelPool;

public class ChannelPoolLibrary : IDisposable
{
    private readonly IConnectionFactory _connectionFactory;
    private readonly string _exchangeName;
    private readonly int _channelPoolMaxSize;
    private readonly int _channelPoolMinSize;
    private readonly ConcurrentBag<IModel> _channelPool;
    private readonly object _poolLock = new();

    public ChannelPoolLibrary(string hostname, string username, string password, string exchangeName, int channelPoolMaxSize, int channelPoolMinSize)
    {
        _connectionFactory = new ConnectionFactory
        {
            HostName = hostname,
            UserName = username,
            Password = password
        };

        _exchangeName = exchangeName;
        _channelPoolMaxSize = channelPoolMaxSize;
        _channelPoolMinSize = channelPoolMinSize;
        _channelPool = new();

        InitializeChannelPool();
    }
    private void InitializeChannelPool()
    {
        lock (_poolLock)
        {
            for (int i = 0; i < _channelPoolMinSize; i++)
            {
                _channelPool.Add(CreateChannel());
            }
        }
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
        if (_channelPool.TryTake(out var channel))
        {
            return channel;
        }

        // If the pool is empty, create a new channel and return it.
        return CreateChannel();
    }

    public void ReleaseChannel(IModel channel)
    {
        if (_channelPool.Count < _channelPoolMaxSize)
        {
            _channelPool.Add(channel);
        }
        else
        {
            channel?.Dispose();
        }
    }

    public void Dispose()
    {
        foreach (var channel in _channelPool)
        {
            channel?.Dispose();
        }
        _channelPool.Clear();
    }
}
