using System.Linq;
using AzureContainer.Models;

namespace AzureContainer.Interfaces
{
    public interface IRepository
    {
        IQueryable<Product> Products { get; }
    }
}