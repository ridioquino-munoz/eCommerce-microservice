using API.Cart.Client;
using API.Cart.DTO;
using Business.Cart;
using Domain.Cart;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Cart.Controllers
{
    [Route("api/carts")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly IProductClient _productClient;
        private readonly IInventoryClient _inventoryClient;
        public CartsController(ICartService cartService, IProductClient productClient, IInventoryClient inventoryClient)
        {
            _cartService = cartService;
            _productClient = productClient; 
            _inventoryClient = inventoryClient;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddCart(CreateCartRequest request)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            foreach (var item in request.Items) { 
                var product = await _productClient.GetProductAsync(item.ProductID);

                if (product == null)
                    return BadRequest(new Exception( "Product does not exist."));

                var inventory = await _inventoryClient.GetProductInventory(item.ProductID);

                if (inventory == null) return BadRequest(new Exception("Product has no inventory."));

                if(inventory.Quantity < item.Quantity) return BadRequest(new Exception("quantity exceed than current available"));

                CartItem cartItem = new CartItem();
                cartItem.ProductID = item.ProductID;
                cartItem.ProductPrice = item.ProductPrice;
                cartItem.Quanitity = item.Quantity;

                await _cartService.AddToCart(userId, cartItem);
            }
            return Ok();
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get() {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var cart = await _cartService.GetCartById(userId);
            return Ok(cart);
        }
    }
}
