using ApiProducer1.Interfaces;
using ApiProducer1.Services;
using Consumer1;
using Consumer2;
using EventMessaging.InMemory;
using EventMessaging.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddSingleton<IOrderService, OrderService>();

// ===> Broker in-memory shared by API and Workers ===
// One Single instance of InMemoryEventBroker is registered and shared across the entire application, including both the API controllers and the background workers.
// This allows for seamless communication between the API and the workers using the same event broker instance.
builder.Services.AddSingleton<InMemoryEventBroker>();
builder.Services.AddSingleton<IEventPublisher>(sp => sp.GetRequiredService<InMemoryEventBroker>());
builder.Services.AddSingleton<IEventSubscriber>(sp => sp.GetRequiredService<InMemoryEventBroker>());

// ===> Registering "worker-1" (BackgroundService) at the SAME process ===
builder.Services.AddHostedService(sp =>
    new Worker1(
        sp.GetRequiredService<ILogger<Worker1>>(),
        sp.GetRequiredService<IEventSubscriber>(),
        sp.GetRequiredService<IConfiguration>()));

// ===> Registering "worker-2" (BackgroundService) at the SAME process ===
builder.Services.AddHostedService(sp =>
    new Worker2(
        sp.GetRequiredService<ILogger<Worker2>>(),
        sp.GetRequiredService<IEventSubscriber>(),
        sp.GetRequiredService<IConfiguration>()));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();


    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "EventBrokerDemo API v1");
    });

}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.Run();
