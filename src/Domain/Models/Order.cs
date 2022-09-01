namespace Domain.Models;

public class Order
{
    public Guid OrderId { get; set; }
    public Customer Customer { get; set; }
    public IEnumerable<Product> Products { get; set; }
}
