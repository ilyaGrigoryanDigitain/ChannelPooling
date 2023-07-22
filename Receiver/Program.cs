using RabbitChannelPool;
using Receiver;

string hostname = "localhost";
string username = "guest";
string password = "guest";
string exchangeName = "simple_message";
int channelPoolSize = 5; // Adjust the pool size as needed.

using (var channelPool = new ChannelPoolLibrary(hostname, username, password, exchangeName, channelPoolSize))
{
    // Example 2: Consuming messages
    var consumer = new MessageConsumer(channelPool);
    consumer.StartConsuming();
}