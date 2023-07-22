using RabbitChannelPool;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Receiver
{
    class MessageConsumer
    {
        private readonly ChannelPoolLibrary _channelPool;

        public MessageConsumer(ChannelPoolLibrary channelPool)
        {
            _channelPool = channelPool;
        }

        public void StartConsuming()
        {
            using var channel = _channelPool.GetChannel();
            // Step 4: Declare the queue (same as in the sender)
            channel.QueueDeclare(queue: "my_queue",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            // Step 5: Create the consumer
            var consumer = new EventingBasicConsumer(channel);

            // Step 6: Handle received messages
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = System.Text.Encoding.UTF8.GetString(body.ToArray());
                Console.WriteLine(" [x] Received '{0}'", message);
            };

            // Step 7: Start consuming messages
            channel.BasicConsume(queue: "my_queue",
                                 autoAck: true,
                                 consumer: consumer);

            Console.WriteLine(" [*] Waiting for messages. Press any key to exit.");
            Console.ReadLine();
        }
    }
}