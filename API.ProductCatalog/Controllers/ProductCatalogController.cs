using API.ProductCatalog.DTO;
using Business;
using Domain;
using Domain.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Policy;

namespace API.ProductCatalog.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductCatalogController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMessagePublisher _messagePublisher;
        public ProductCatalogController( IProductService productService, IMessagePublisher messagePublisher)
        {
            _productService = productService;
            _messagePublisher = messagePublisher;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        { 
            var products = await _productService.GetAll();
            return Ok(products);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Add(CreateProductRequest request)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var product = new Product
            {
                Name = request.Name,
                Price = request.Price
            };
            product.CreatedByUser = userId;

            var productAdded = await _productService.Create(product);

            await _messagePublisher.PublishAsync(
                        "product-created",
                    new ProductCreatedEvent
                    {
                        ProductId = product.ID,
                        Name = product.Name,
                        Price = product.Price
                    });

            return Ok(productAdded);
        }

        [Authorize]
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetByID(Guid id)
        {
            var product = await _productService.GetById(id);
            return Ok(product);
        }
    }
}
