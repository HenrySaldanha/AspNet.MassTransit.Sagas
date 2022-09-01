using MassTransit;
using Domain;
using Serilog;

namespace ProductWorker.Consumers;

public class AddProductsConsumer : IConsumer<AddProductsCommand>
{
    private readonly Serilog.ILogger _logger;

    public AddProductsConsumer() => _logger = Log.ForContext<AddProductsConsumer>();

    public async Task Consume(ConsumeContext<AddProductsCommand> context)
    {
        _logger.Information("Received event: {event}; Request: {@request}; CorrelationId {correlationId}",
            nameof(AddProductsCommand), @context.Message, context.CorrelationId);
       
        Thread.Sleep(1000);

        if (context.Message.Products.Any(i => i.ExpirationDate < DateTime.Now))
        {

            await context.Publish(new ProductsRejectedEvent(context.Message.CorrelationId));

            _logger.Information("Published event: {event}; CorrelationId {correlationId}",
                nameof(ProductsRejectedEvent), context.CorrelationId);
        }
        else
        {
            await context.Publish(new ProductsAddedEvent(context.Message.CorrelationId));

            _logger.Information("Published event: {event}; CorrelationId {correlationId}",
                nameof(ProductsAddedEvent), context.CorrelationId);
        }
    }
}
