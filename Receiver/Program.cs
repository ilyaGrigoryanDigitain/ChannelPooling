using RabbitChannelPool;
using Receiver;

string hostname = "localhost";
string username = "guest";
string password = "guest";
string exchangeName = "simple_message";
int channelPoolMaxSize = 5; // Adjust the pool size as needed.
int channelPoolMinSize = 3; // Adjust the pool size as needed.

using (var channelPool = new ChannelPoolLibrary(hostname, username, password, exchangeName, channelPoolMaxSize, channelPoolMinSize))
{
    // Example 2: Consuming messages
    var consumer = new MessageConsumer(channelPool);
    consumer.StartConsuming();
}