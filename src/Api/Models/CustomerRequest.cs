using Domain.Models;

namespace Api.Models;

public class CustomerRequest
{
    public string Name { get; set; }
    public string Document { get; set; }
    public string PhoneNumber { get; set; }

    public static implicit operator Customer(CustomerRequest request)
    {
        if (request == null)
            return null;

        return new Customer
        {
            CustomerId = Guid.NewGuid(),
            Document = request.Document,
            PhoneNumber = request.PhoneNumber,
            Name = request.Name,
        };
    }
}
