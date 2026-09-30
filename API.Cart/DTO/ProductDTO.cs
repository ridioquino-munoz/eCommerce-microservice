namespace API.Cart.DTO
{
    public record ProductResponse(
        Guid ID,
        string Name,
        double Price
    );
   
    public record ProductInventoryResponse(
        Guid ProductID,
        int Quantity
    );
    
}
