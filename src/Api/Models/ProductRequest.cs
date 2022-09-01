using Domain.Models;

namespace Api.Models;

public class ProductRequest
{
    public string Name { get; set; }
    public DateTime ExpirationDate { get; set; }

    public static implicit operator Product(ProductRequest request)
    {
        if (request == null)
            return null;

        return new Product
        {
            ProductId = Guid.NewGuid(),
            Name = request.Name,
            ExpirationDate = request.ExpirationDate
        };
    }
}
