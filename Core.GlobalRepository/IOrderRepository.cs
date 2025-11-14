using Data.Common.Definition;
using LegacyOrderService.Models;

namespace Core.GlobalRepository
{
    public interface IOrderRepository : IGenericRepository<Order>
    {

    }

    public interface IProductRepository : IGenericRepository<Product>
    {

    }
}
