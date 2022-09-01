using Dapper.Contrib.Extensions;
using Domain.Models;
using MassTransit;

namespace PurchaseSaga;
public class OrderState : SagaStateMachineInstance
{
    [ExplicitKey]
    public Guid CorrelationId { get; set; }
    public int CurrentState { get; set; }
    public IEnumerable<Product> Products { get; set; }
    public Customer SavedCustomer { get; set; }
}