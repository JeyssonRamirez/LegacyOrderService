using LegacyOrderService.Models;

namespace Data.Common.Definition
{
    public interface IUnitOfWork
    {
        Task<int> CommitInt();
        Task RollbackChanges();
        Task AttachEntity<T>(T item) where T : Entity;
        Task<bool> AddEntity<T>(T item) where T : Entity;
        Task<bool> RemoveEntity<T>(T item) where T : Entity;

    }
}
