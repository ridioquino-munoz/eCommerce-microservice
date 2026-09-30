using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Product;

namespace Business
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepsitory;
        public ProductService(IProductRepository productRepository)
        {
            _productRepsitory = productRepository;
        }
        public async Task<Product> Create(Product product)
        {
            return await _productRepsitory.Create(product);
        }

        public async Task<bool> DeleteById(Guid id)
        {
            return await _productRepsitory.DeleteById(id);
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            return await _productRepsitory.GetAll();
        }

        public async Task<Product?> GetById(Guid id)
        {
            return await _productRepsitory.GetById(id);
        }

        public async Task<Product?> GetByName(string name)
        {
            return await _productRepsitory.GetByName(name);
        }

        public async Task<Product> Update(Product product)
        {
          return await _productRepsitory.Update(product);
        }
    }

    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAll();
        Task<Product?> GetById(Guid id);

        Task<Product?> GetByName(string name);

        Task<Product> Create(Product product);
        Task<Product> Update(Product product);
        Task<bool> DeleteById(Guid id);
    }
}
