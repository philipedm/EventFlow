using Contract;
using EventMessaging.Interfaces;

namespace Consumer1
{
    public class Worker1 : BackgroundService
    {

        private readonly ILogger<Worker1> _logger;
        private readonly IEventSubscriber _subscriber;
        private readonly string _subscriptionName;

        public Worker1(ILogger<Worker1> logger, IEventSubscriber subscriber, IConfiguration cfg)
        {
            _logger = logger;
            _subscriber = subscriber;
            _subscriptionName = cfg["ServiceBus:SubscriptionName"] ?? "worker-1";
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _subscriber.SubscribeAsync("orders", _subscriptionName, async envelope =>
            {
                var evt = System.Text.Json.JsonSerializer.Deserialize<OrderCreated>(envelope.Body.Span);
                
                _logger.LogInformation("Worker {Sub} got OrderCreated: {OrderId} for Customer {CustomerId}",
                    _subscriptionName, evt!.Id, evt.User);

                await Task.Delay(200, stoppingToken); // simulate work
            }, stoppingToken);

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }

    }
}
