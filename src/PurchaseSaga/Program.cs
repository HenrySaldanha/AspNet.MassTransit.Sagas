using MassTransit;
using Domain;
using Dependencies;
using PurchaseSaga;
using Serilog.Events;
using Serilog;

Log.Logger = new LoggerConfiguration()
          .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
          .Enrich.FromLogContext()
          .WriteTo.Console()
          .CreateLogger();

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
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

            EndpointConvention.Map<CreateOrderCommand>(new Uri($"queue:CreateOrder"));
            EndpointConvention.Map<AddCustomerCommand>(new Uri($"queue:AddCustomer"));
            EndpointConvention.Map<RemoveCustomerCommand>(new Uri($"queue:RemoveCustomer"));
            EndpointConvention.Map<AddProductsCommand>(new Uri($"queue:AddProducts"));

            GlobalTopology.Send.UseCorrelationId<CreateOrderCommand>(i => i.OrderRequest.OrderId);

            config.AddSagaStateMachine<OrderStateMachine, OrderState>()
                        .InMemoryRepository();
        });

        services.AddHostedService<Worker>();
    })
    .UseSerilog();

Log.Information("Starting Purchase Saga");

host.Build().Run();
