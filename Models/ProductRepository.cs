using System;
using System.Linq;
using System.Threading.Tasks;
using AzureContainer.Exceptions;
using AzureContainer.Interfaces;

namespace AzureContainer.Models
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductDbContext _context;

        public ProductRepository(ProductDbContext context)
        {
            _context = context;
        }

        public IQueryable<Product> Products => _context.Products;

        public async Task Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) throw new RepositoryException("Can't find Product.");

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task Insert(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Product product)
        {
            if (!IsExists(product.Id)) throw new RepositoryException("Can't find Product.");

            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        private bool IsExists(int id)
        {
            var product = Products.Where(p => p.Id == id).FirstOrDefault();
            if (product == null) return false;

            return true;
        }

    }
}