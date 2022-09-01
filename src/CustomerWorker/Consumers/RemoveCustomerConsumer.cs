using MassTransit;
using Domain;
using Serilog;

namespace CustomerWorker.Consumers;

public class RemoveCustomerConsumer : IConsumer<RemoveCustomerCommand>
{
    private readonly ILogger _logger;

    public RemoveCustomerConsumer() => _logger = Log.ForContext<RemoveCustomerConsumer>();

    public async Task Consume(ConsumeContext<RemoveCustomerCommand> context)
    {
        _logger.Information("Received event: {event}; Request: {@request}; CorrelationId {correlationId}",
            nameof(RemoveCustomerCommand), @context.Message, context.CorrelationId);

        Thread.Sleep(1000);

        await context.Publish(new CustomerRemovedEvent(context.Message.CorrelationId));

        _logger.Information("Published event: {event}; CorrelationId {correlationId}",
            nameof(CustomerRemovedEvent), context.CorrelationId);
    }
}
