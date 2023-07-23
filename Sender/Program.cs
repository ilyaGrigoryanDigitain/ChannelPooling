using RabbitChannelPool;
using Sender;
using System.Threading.Tasks;

string hostname = "localhost";
string username = "guest";
string password = "guest";
string exchangeName = "simple_message";
int channelPoolMaxSize = 5; // Adjust the pool size as needed.
int channelPoolMinSize = 3; // Adjust the pool size as needed.
var parallelList = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 };

using (var channelPool = new ChannelPoolLibrary(hostname, username, password, exchangeName, channelPoolMaxSize, channelPoolMinSize))
{
    Parallel.ForEach(parallelList, number =>
    {
        // Example 1: Sending messages
        var sender = new MessageSender(channelPool);
        sender.SendMessage("Hello, RabbitMQ!");
    });

    Console.WriteLine("press enter to exit.");
    Console.ReadLine();
}