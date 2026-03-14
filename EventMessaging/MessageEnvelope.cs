using EventMessaging.Interfaces;

namespace EventMessaging
{
    public class MessageEnvelope : IMessageEnvelope
    {

        public string Subject { get; init; } = default!;
        public string MessageId { get; init; } = Guid.NewGuid().ToString("N");
        public string? CorrelationId { get; init; }

        public IDictionary<string, object> Properties { get; init; } = new Dictionary<string, object>();
        public ReadOnlyMemory<byte> Body { get; init; }



        public static MessageEnvelope FromJson<T>(T payload, string subject, string? messageId = null, string? correlationId = null, IDictionary<string, object>? props = null)
        {
            var bytes = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(payload);
            return new MessageEnvelope
            {
                Subject = subject,
                MessageId = messageId ?? Guid.NewGuid().ToString("N"),
                CorrelationId = correlationId,
                Properties = props ?? new Dictionary<string, object>(),
                Body = bytes
            };
        }

        public T ReadJson<T>() => System.Text.Json.JsonSerializer.Deserialize<T>(Body.Span)!;


    }
}
