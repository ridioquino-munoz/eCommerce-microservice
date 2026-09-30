using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Inventory
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly InventoryDBContext _context;
        public InventoryRepository(InventoryDBContext context)
        {
            _context = context;
        }

        public async Task<ProductInventory> CreateProduct(ProductInventory product)
        {
           _context.ProductInventories.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<ProductInventory?> GetStockAsync(Guid productID)
        {
            var inventory = _context.ProductInventories.Where(i => i.ProductID == productID).FirstOrDefaultAsync();
            return await inventory;
        }

        public async Task<ProductInventory?> UpdateStock(ProductInventory product)
        {
            _context.ProductInventories.Update(product);
            await _context.SaveChangesAsync();
            return product;
        }
    }

    public interface IInventoryRepository
    { 
        Task<ProductInventory?> GetStockAsync(Guid productID);
        Task<ProductInventory> CreateProduct(ProductInventory product);

        Task<ProductInventory?> UpdateStock(ProductInventory product);
    }
}
