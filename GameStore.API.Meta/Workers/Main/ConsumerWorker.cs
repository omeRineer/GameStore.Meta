using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;

namespace GameStore.API.Meta.Workers.Main
{
    public abstract class ConsumerWorker : BackgroundService
    {
        private string Tag { get; init; }
        private string Queue { get; init; }

        protected readonly IChannel Channel;
        public ConsumerWorker(IChannel channel, string queue, string tag)
        {
            Channel = channel;
            Tag = tag;
            Queue = queue;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Channel.QueueDeclareAsync(queue: Queue,
                                            durable: true,
                                            exclusive: false,
                                            autoDelete: false,
                                            arguments: null);

            var consumer = new AsyncEventingBasicConsumer(Channel);

            consumer.ReceivedAsync += HandleAsync;

            await Channel.BasicConsumeAsync(queue: Queue,
                         autoAck: false,
                         consumer: consumer,
                         consumerTag: Tag,
                         exclusive: false,
                         noLocal: false,
                         arguments: null);
        }
        protected abstract Task HandleAsync(object model, BasicDeliverEventArgs args);

        protected TMessage? GetMessage<TMessage>(BasicDeliverEventArgs args)
        {
            var body = Encoding.UTF8.GetString(args.Body.ToArray());
            var deserializedMessage = JsonSerializer.Deserialize<TMessage>(body);

            return deserializedMessage;
        }
    }
}
