using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace Domain.Cart
{
    
    public class CartRepository : ICartRepository
    {
        private readonly IDatabase _database;

        public CartRepository(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }

        public async Task<Cart?> GetCartAsync(Guid userId)
        {
            var data = await _database.StringGetAsync($"cart:{userId}");

            if (data.IsNullOrEmpty)
                return null;

            return JsonSerializer.Deserialize<Cart>(data!);
        }

        public async Task SaveCartAsync(Cart cart)
        {
            await _database.StringSetAsync(
                $"cart:{cart.UserID}",
                JsonSerializer.Serialize(cart),
                TimeSpan.FromDays(1));
        }

        public async Task DeleteCartAsync(Guid userId)
        {
            await _database.KeyDeleteAsync($"cart:{userId.ToString()}");
        }
    }

    public interface ICartRepository
    {
        Task<Cart?> GetCartAsync(Guid userId);
        Task SaveCartAsync(Cart cart);
        Task DeleteCartAsync(Guid userId);
    }
}
