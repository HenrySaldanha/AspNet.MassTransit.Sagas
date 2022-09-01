using MassTransit;
using Domain;
using Serilog;

namespace CustomerWorker.Consumers;

public class AddCustomerConsumer : IConsumer<AddCustomerCommand>
{
    private readonly ILogger _logger;

    public AddCustomerConsumer() => _logger = Log.ForContext<AddCustomerConsumer>();

    public async Task Consume(ConsumeContext<AddCustomerCommand> context)
    {
        _logger.Information("Received event: {event}; Request: {@request}; CorrelationId {correlationId}",
            nameof(AddCustomerCommand), @context.Message, context.CorrelationId);

        Thread.Sleep(1000);

        await context.Publish(new CustomerAddedEvent(context.Message.CorrelationId, context.Message.Customer));

        _logger.Information("Published event: {event}; CorrelationId {correlationId}",
            nameof(CustomerAddedEvent), context.CorrelationId);
    }
}
