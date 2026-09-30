using API.Cart.DTO;
using Domain.Product;

namespace API.Cart.Client
{
    public class InventoryClient : IInventoryClient
    {
        private readonly HttpClient _httpClient;
        public InventoryClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<ProductInventoryResponse?> GetProductInventory(Guid productID)
        {
            var response =
                    await _httpClient.GetAsync(
                        $"api/inventory/{productID}");

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content


            .ReadFromJsonAsync<ProductInventoryResponse>();
        }
    }

    public interface IInventoryClient
    {
        Task<ProductInventoryResponse?> GetProductInventory(Guid productID);
    }
}
