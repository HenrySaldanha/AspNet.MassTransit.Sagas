using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MassTransit;
using Domain;
using Dependencies;
using Serilog;
using Serilog.Events;
using CustomerWorker.Consumers;

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

            EndpointConvention.Map<AddCustomerCommand>(new Uri($"queue:AddCustomer"));
            EndpointConvention.Map<RemoveCustomerCommand>(new Uri($"queue:RemoveCustomer"));

            config.AddConsumer<AddCustomerConsumer>();
            config.AddConsumer<RemoveCustomerConsumer>();
        });
    })
    .UseSerilog();

Log.Information("Starting Customer Worker");

host.Build().Run();