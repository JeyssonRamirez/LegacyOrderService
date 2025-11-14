using Core.GlobalRepository;
using Data.Common.Definition;
using DataAccess.Provider.SQLite.LocalKioskDBContext;
using LegacyOrderService.Models;

namespace DataAccess.Provider.SQLite
{
    public class OrderRepository  : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(LocalKioskDbContext context) : base(context, true)
        {
        }

    }
}
