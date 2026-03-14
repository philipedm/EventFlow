namespace EventMessaging.Interfaces
{
    public interface IEventSubscriber
    {

        // Subscribe to a topic with a logical subscription name (like Azure SB)
        Task SubscribeAsync(string topicName, string subscriptionName, Func<IMessageEnvelope, Task> handler, CancellationToken ct = default);

    }
}
