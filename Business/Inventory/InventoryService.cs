using Domain.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Inventory
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository _inventoryRepository;
        public InventoryService(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        public async Task<ProductInventory> CreateProduct(ProductInventory product)
        {
            var newProduct = await _inventoryRepository.CreateProduct(product);
            return newProduct;
        }

        public async Task<ProductInventory?> GetProductInventoryAsync(Guid productID)
        {
            return await _inventoryRepository.GetStockAsync(productID);
        }

        public async Task<ProductInventory?> UpdateStock(ProductInventory product)
        {
            return await _inventoryRepository.UpdateStock(product);
        }
    }

    public interface IInventoryService
    {
        Task<ProductInventory?> GetProductInventoryAsync(Guid productID);
        Task<ProductInventory> CreateProduct(ProductInventory product);
        Task<ProductInventory?> UpdateStock(ProductInventory product);
    }
}
