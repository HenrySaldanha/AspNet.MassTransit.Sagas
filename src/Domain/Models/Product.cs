namespace Domain.Models;

public class Product
{
    public Guid ProductId { get; set; }
    public string Name { get; set; }
    public DateTime ExpirationDate { get; set; }
}
