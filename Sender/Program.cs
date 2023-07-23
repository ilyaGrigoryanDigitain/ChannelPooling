using RabbitChannelPool;
using Sender;

string hostname = "localhost";
string username = "guest";
string password = "guest";
string exchangeName = "simple_message";
int channelPoolMaxSize = 5; // Adjust the pool size as needed.
int channelPoolMinSize = 3; // Adjust the pool size as needed.

using (var channelPool = new ChannelPoolLibrary(hostname, username, password, exchangeName, channelPoolMaxSize, channelPoolMinSize))
{
    // Example 1: Sending messages
    var sender = new MessageSender(channelPool);
    sender.SendMessage("Hello, RabbitMQ!");

    Console.WriteLine("press enter to exit.");
    Console.ReadLine();
}