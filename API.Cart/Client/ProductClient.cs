using API.Cart.DTO;
using System.Net.Http.Headers;

namespace API.Cart.Client
{
    public class ProductClient : IProductClient
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ProductResponse?> GetProductAsync(Guid productId)
        {
            var token = _httpContextAccessor
            .HttpContext?
            .Request
            .Headers["Authorization"]
            .ToString();

            _httpClient.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue(
            "Bearer",
            token?.Replace("Bearer ", "")
            );
            var response =
                    await _httpClient.GetAsync(
                        $"api/product/{productId}");

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content


            .ReadFromJsonAsync<ProductResponse>();

        }
    }

    public interface IProductClient
    {
        Task<ProductResponse?> GetProductAsync(Guid productId);
    }
}
