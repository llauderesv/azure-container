using System.Linq;
using System.Threading.Tasks;
using AzureContainer.Models;

namespace AzureContainer.Interfaces
{
    public interface IProductRepository
    {
        IQueryable<Product> Products { get; }

        Task Insert(Product product);

        Task Delete(int id);

        Task Update(Product product);
    }
}