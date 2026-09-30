using Domain.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Cart
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }
        public async Task AddToCart(Guid userId, CartItem item)
        {
            var cart =
                await _cartRepository.GetCartAsync(userId)
                ?? new Domain.Cart.Cart
                {
                    UserID = userId
                };

            var existing =
                cart.Items.FirstOrDefault(
                    x => x.ProductID == item.ProductID);

            if (existing != null)
            {
                existing.Quanitity += item.Quanitity;
            }
            else
            {
                cart.Items.Add(item);
            }

            await _cartRepository.SaveCartAsync(cart);
        }

        public async Task<Domain.Cart.Cart?> GetCartById(Guid userId)
        {
            var cart = await _cartRepository.GetCartAsync(userId);
            return cart;
        }
    }

    public interface ICartService
    {
        Task AddToCart(Guid userId, CartItem item);
        Task<Domain.Cart.Cart?> GetCartById(Guid userId);
    }
}
