using MassTransit;
using Domain;
using Serilog;

namespace PurchaseSaga;

public class OrderStateMachine : MassTransitStateMachine<OrderState>
{
    public State AddCustomer { get; private set; }
    public State AddProducts { get; private set; }
    public State RemovingCustomer { get; private set; }

    public Event<CreateOrderCommand> CreateOrder { get; private set; }
    public Event<CustomerAddedEvent> CustomerAdded { get; private set; }
    public Event<CustomerRemovedEvent> CustomerRemoved { get; private set; }
    public Event<ProductsAddedEvent> ProductsAdded { get; private set; }
    public Event<ProductsRejectedEvent> ProcuctsRejected { get; private set; }

    public OrderStateMachine()
    {
        var logger = Log.ForContext<OrderStateMachine>();

        InstanceState(x => x.CurrentState, AddCustomer, RemovingCustomer, AddProducts);

        Initially(
            When(CreateOrder)
                .Then(c => c.Saga.Products = c.Message.OrderRequest.Products)
                .Send(c => new AddCustomerCommand(c.Saga.CorrelationId, c.Message.OrderRequest.Customer))
                .TransitionTo(AddCustomer));

        During(AddCustomer,
            When(CustomerAdded)
                .Then(c => c.Saga.SavedCustomer = c.Message.Customer)
                .Send(c => new AddProductsCommand(c.Saga.CorrelationId, c.Saga.Products))
                .TransitionTo(AddProducts));

        During(AddProducts,
            When(ProductsAdded)
                .Then(context =>
                    logger.Information("Purchase Saga successfully completed Correlation Id: {id}",
                    context.CorrelationId))
                    .Finalize(),
            When(ProcuctsRejected)
                .Send(c => new RemoveCustomerCommand(c.Saga.CorrelationId, c.Saga.SavedCustomer))
                .TransitionTo(RemovingCustomer));

        During(RemovingCustomer,
            When(CustomerRemoved)
                .Then(context =>
                    logger.Warning("Purchase Saga finished with rollback, Correlation Id: {id}",
                    context.CorrelationId))
            .Finalize());

        SetCompletedWhenFinalized();
    }
}
