using Domain.Models;

namespace Domain;

public record CreateOrderCommand(Order OrderRequest);
public record AddCustomerCommand(Guid CorrelationId, Customer Customer);
public record AddProductsCommand(Guid CorrelationId, IEnumerable<Product> Products);
public record RemoveCustomerCommand(Guid CorrelationId, Customer Customer);