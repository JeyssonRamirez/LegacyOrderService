using Core.GlobalRepository;
using Data.Common.Definition;
using DataAccess.Provider.SQLite.LocalKioskDBContext;
using LegacyOrderService.Models;

namespace DataAccess.Provider.SQLite.Repositories
{
    public class OrderRepository  : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(LocalDbContext context) : base(context, true)
        {
        }

    }
}
