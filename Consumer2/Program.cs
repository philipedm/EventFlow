using Consumer2;
using EventMessaging.InMemory;
using EventMessaging.Interfaces;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<InMemoryEventBroker>();
builder.Services.AddSingleton<IEventPublisher>(sp => sp.GetRequiredService<InMemoryEventBroker>());
builder.Services.AddSingleton<IEventSubscriber>(sp => sp.GetRequiredService<InMemoryEventBroker>());

builder.Services.AddHostedService<Worker2>();

var host = builder.Build();
await host.RunAsync();
