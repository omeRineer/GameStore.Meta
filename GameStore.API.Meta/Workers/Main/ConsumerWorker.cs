using GameStore.Meta.Business.Services;
using GameStore.Meta.Entities.Objects;
using GameStore.Meta.Models.Message;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;

namespace GameStore.API.Meta.Workers.Main
{
    public abstract class ConsumerWorker : BackgroundService
    {
        protected string Tag { get; private init; }
        protected string Queue { get; private init; }
        protected bool AutoAck { get; private init; }

        protected readonly IConnection Connection;
        protected readonly IChannel Channel;
        public ConsumerWorker(IChannel channel, IConnection connection, string queue, string tag, bool autoAck = true)
        {
            Channel = channel;
            Tag = tag;
            Queue = queue;
            AutoAck = autoAck;
            Connection = connection;
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
                         autoAck: AutoAck,
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
