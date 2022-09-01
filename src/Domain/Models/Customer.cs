namespace Domain.Models;

public class Customer
{
    public Guid CustomerId { get; set; }
    public string Name { get; set; }
    public string Document { get; set; }
    public string PhoneNumber { get; set; }
}