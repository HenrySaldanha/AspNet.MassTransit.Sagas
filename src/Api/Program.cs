using MassTransit;
using Domain;
using Dependencies;
using Serilog.Events;
using Serilog;

Log.Logger = new LoggerConfiguration()
          .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
          .Enrich.FromLogContext()
          .WriteTo.Console()
          .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

var services = builder.Services;
services.AddControllers();
services.AddEndpointsApiExplorer()
    .AddSwaggerGen();

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
});

var app = builder.Build();

app.UseSwagger()
    .UseSwaggerUI()
    .UseAuthorization();

app.MapControllers();

Log.Information("Starting Api");

app.Run();