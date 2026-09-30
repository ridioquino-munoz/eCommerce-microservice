using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Product
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductDBContext _dbContext;
        public ProductRepository(ProductDBContext dBContext)
        {
            _dbContext = dBContext;
        }
        public async Task<Product> Create(Product product)
        {
           _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();
            return product;
        }

        public async Task<bool> DeleteById(Guid id)
        {
            var product = _dbContext.Products.Find(id);
            if (product == null) {
                throw new Exception("user not found");
            }
             _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            return await _dbContext.Products.ToListAsync();
        }

        public async Task<Product?> GetById(Guid id)
        {
            var product = _dbContext.Products.FindAsync(id);
            return await product;
        }

        public async Task<Product?> GetByName(string name)
        {
            var product = _dbContext.Products.Where(p => p.Name == name).FirstOrDefaultAsync();
            return await product;
        }

        public async Task<Product> Update(Product product)
        {
           _dbContext.Products.Update(product);
            await _dbContext.SaveChangesAsync();
            return product;
        }
    }

    public interface IProductRepository
    { 
        Task<IEnumerable<Product>> GetAll();
        Task<Product?> GetById(Guid id);

        Task<Product?> GetByName(string name);

        Task<Product> Create(Product product);
        Task<Product> Update(Product product);
        Task<bool> DeleteById(Guid id);
    }
}
