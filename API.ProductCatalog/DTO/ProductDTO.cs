namespace API.ProductCatalog.DTO
{

    public record CreateProductRequest (string Name, double Price);

    public class ProductCreatedEvent
    {
        public Guid ProductId { get; set; }

        public string Name { get; set; } = string.Empty;

        public double Price { get; set; }
    }
}
