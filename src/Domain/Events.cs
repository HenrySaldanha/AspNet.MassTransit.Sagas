using Domain.Models;

namespace Domain;

public record CustomerAddedEvent(Guid CorrelationId, Customer Customer);
public record ProductsAddedEvent(Guid CorrelationId);
public record ProductsRejectedEvent(Guid CorrelationId);
public record CustomerRemovedEvent(Guid CorrelationId);