using Domain.Models;

namespace Api.Models;

public class OrderRequest
{
    public CustomerRequest Customer { get; set; }
    public IEnumerable<ProductRequest> Products { get; set; }

    public static implicit operator Order(OrderRequest request)
    {
        if (request == null)
            return null;

        return new Order
        {
            Customer = request.Customer,
            OrderId = Guid.NewGuid(),
            Products = request.Products.Select(c => (Product)c)
        };
    }
}
