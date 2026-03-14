namespace EventMessaging.Interfaces
{
    public interface IMessageEnvelope
    {

        string Subject { get; }
        string MessageId { get; }
        string? CorrelationId { get; }
        IDictionary<string, object> Properties { get; }
        ReadOnlyMemory<byte> Body { get; }

    }
}
