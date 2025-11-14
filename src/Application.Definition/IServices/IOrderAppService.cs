using Core.DataTransferObject;
using Core.DataTransferObject.Order;
using LegacyOrderService.Models;

namespace Application.Definition.IServices
{
    public interface IOrderAppService
    {
        Task<RecordIdApiResult> ModifyOrder(ModifyOrderDTO model, CancellationToken cancellationToken = default);
        Task<RecordApiResult<Product>> GetProductByName(string productName, CancellationToken cancellationToken = default);

        Task<BaseApiResult> UpdateDatabase();
    }
}
