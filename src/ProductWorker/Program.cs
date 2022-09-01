using MassTransit;
using Domain;
using Dependencies;
using Serilog.Events;
using Serilog;
using ProductWorker.Consumers;

Log.Logger = new LoggerConfiguration()
          .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
          .Enrich.FromLogContext()
          .WriteTo.Console()
          .CreateLogger();

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();

        services.AddMassTransit(config =>
        {
            config.UsingRabbitMq((context, rabbitConfig) =>
            {
                rabbitConfig.Host(RabbitMqConnection.HOST, "/", h =>
                {
                    h.Username(RabbitMqConnection.USERNAME);
                    h.Password(RabbitMqConnection.PASSWORD);
                });

                rabbitConfig.ConfigureEndpoints(context);
            });
            EndpointConvention.Map<AddProductsCommand>(new Uri($"queue:AddProducts"));
            config.AddConsumer<AddProductsConsumer>();
        });
    })
    .UseSerilog();

Log.Information("Starting Product Worker");

host.Build().Run();