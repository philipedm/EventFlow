namespace EventMessaging.Interfaces
{
    public interface IEventPublisher
    {
        Task PublishAsync(string topicName, IMessageEnvelope message, CancellationToken ct = default);
    }
}
