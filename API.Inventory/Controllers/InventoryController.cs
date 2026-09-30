using API.Inventory.DTO;
using Business.Inventory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Inventory.Controllers
{
    [Route("api/inventory")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;
        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [HttpGet]
        [Route("{productId:guid}")]
        public async Task<IActionResult> GetStock(Guid productId)
        {
            var data =
                await _inventoryService
                    .GetProductInventoryAsync(productId);

            return Ok(data);
        }

        [HttpPost("restock")]
        public async Task<IActionResult> UpdateStock(UpdateStockRequest request)
        {
            var product = await _inventoryService.GetProductInventoryAsync(request.ProductID);

            if (product == null) return BadRequest(new Exception("Product does not exist"));

            product.DateLastUpdate = DateTime.UtcNow;
            product.Quantity =+ request.Quantity;

            await _inventoryService.UpdateStock(product);

            return Ok(product);
        }

        [HttpPost("reserved")]
        public async Task<IActionResult> Reserved(UpdateStockRequest request)
        {
            var product = await _inventoryService.GetProductInventoryAsync(request.ProductID);

            if (product == null) return BadRequest(new Exception("Product does not exist"));

            product.DateLastUpdate = DateTime.UtcNow;
            product.Quantity =- request.Quantity;
            product.ReservedQuantity = request.Quantity;

            await _inventoryService.UpdateStock(product);

            return Ok(product);
        }
    }
}
