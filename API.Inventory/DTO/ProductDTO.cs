namespace API.Inventory.DTO
{
    public class ProductCreatedEvent
    {
        public Guid ProductId { get; set; }

        public string Name { get; set; } = string.Empty;

        public double Price { get; set; }
    }

    public record UpdateStockRequest(Guid ProductID, int Quantity);
}
