using Application.Definition.IServices;
using Core.DataTransferObject;
using Core.DataTransferObject.Order;
using Core.GlobalRepository;
using LegacyOrderService.Models;
using Microsoft.Extensions.Logging;

namespace Application.Implementation.Services
{
    public class OrderAppService : IOrderAppService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly ILogger<OrderAppService> _logger;
        public OrderAppService(IOrderRepository orderRepository, ILogger<OrderAppService> logger, IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _logger = logger;
            _productRepository = productRepository;
        }

        public async Task<RecordApiResult<Product>> GetProductByName(string productName, CancellationToken cancellationToken = default)
        {
            var r = new RecordApiResult<Product>();

            var result = _productRepository.GetOne(filter: s => s.Name == productName);
            if (result == null)
            {
                r.Success = false;
                r.MessageCode = MessageCodeType.NotRecordFound;
            }
            else
            {
                r.Success = true;
                r.Data = result;
                r.MessageCode = MessageCodeType.OK;
            }

            return await Task.FromResult(r);
        }

        public async Task<RecordIdApiResult> ModifyOrder(ModifyOrderDTO model, CancellationToken cancellationToken = default)
        {

            var r = new RecordIdApiResult();

            switch (model.ActionType)
            {
                case ActionType.Undefined:
                    r.MessageCode = MessageCodeType.BadRequest;
                    break;
                case ActionType.Add:
                    var newEntity = MapperHelper.Mapper(model, new Order
                    {

                    });
                    newEntity = _orderRepository.AddItem(newEntity);
                    r.Data = newEntity.Id;
                    r.Success = true;
                    r.MessageCode = MessageCodeType.OK;
                    break;
                case ActionType.Update:
                    var old = _orderRepository.GetOne(s => s.Id == model.Id);
                    if (old == null)
                    {
                        r.Success = false;
                        r.MessageCode = MessageCodeType.NotRecordFound;
                        return r;
                    }

                    var oldDetails = old;

                    var entity = MapperHelper.Mapper(model, new Order
                    {
                        Id = old.Id,
                    });
                    entity = _orderRepository.Update(s => s.Id == model.Id, entity);
                    r.Data = entity.Id;
                    r.Success = true;
                    r.MessageCode = MessageCodeType.OK;
                    break;
                default:
                    break;
            }

            return r;
        }
    }
}
