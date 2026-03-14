using EventMessaging.Interfaces;
using System.Collections.Concurrent;
using System.Threading.Channels;

namespace EventMessaging.InMemory
{
    public class InMemoryEventBroker : IEventPublisher, IEventSubscriber
    {
        // topic -> subscription -> channel
        private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, Channel<IMessageEnvelope>>> _topics = new();

        public async Task PublishAsync(string topicName, IMessageEnvelope message, CancellationToken ct = default)
        {

            var subs = _topics.GetOrAdd(topicName, _ => new());
            foreach (var kvp in subs)
            {
                var channel = kvp.Value;
                if (!channel.Writer.TryWrite(message))
                {
                    channel.Writer.WriteAsync(message, ct).AsTask().Wait(ct);
                }
            }
            await Task.CompletedTask;
        }

        public async Task SubscribeAsync(string topicName, string subscriptionName, Func<IMessageEnvelope, Task> handler, CancellationToken ct = default)
        {

            var subs = _topics.GetOrAdd(topicName, _ => new());
            var channel = subs.GetOrAdd(subscriptionName, _ =>
            {
                var c = Channel.CreateUnbounded<IMessageEnvelope>(new UnboundedChannelOptions
                {
                    SingleReader = false,
                    SingleWriter = false
                });

                // Start background reader:
                Task.Run(async () =>
                    {
                        await foreach (var msg in c.Reader.ReadAllAsync(ct))
                        {
                            await handler(msg);
                        }
                    }, ct);

                return c;
            });

            await Task.CompletedTask;
        }
    }
}
