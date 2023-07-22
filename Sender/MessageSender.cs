using RabbitChannelPool;
using RabbitMQ.Client;

namespace Sender
{
    class MessageSender
    {
        private readonly ChannelPoolLibrary _channelPool;

        public MessageSender(ChannelPoolLibrary channelPool)
        {
            _channelPool = channelPool;
        }

        public void SendMessage(string message)
        {
            using var channel = _channelPool.GetChannel();
            // Step 4: Declare the queue
            channel.QueueDeclare(queue: "my_queue",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            // Step 5: Convert the message to a byte array
            var body = System.Text.Encoding.UTF8.GetBytes(message);

            // Step 6: Publish the message to the queue
            channel.BasicPublish(exchange: "",
                                 routingKey: "my_queue",
                                 basicProperties: null,
                                 body: body);

            Console.WriteLine(" [x] Sent '{0}'", message);
        }
    }
}