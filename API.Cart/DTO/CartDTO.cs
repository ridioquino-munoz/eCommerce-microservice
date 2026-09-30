namespace API.Cart.DTO
{
    public record CreateCartRequest(
        List<CartItemRequest> Items
    );

    public record CartItemRequest(
        Guid ProductID, double ProductPrice, int Quantity
    );
}
