using Core.DataTransferObject;
using LegacyOrderService.Models;
using System.Linq.Expressions;

namespace Data.Common.Definition
{
    public interface IGenericRepository<T> where T : class
    {
        T AddItem(T item);
        IEnumerable<T> AddRangeOfItems(IEnumerable<T> items);

        long Count(Expression<Func<T, bool>> filter = null);
        T GetOne(Expression<Func<T, bool>> filter, List<Expression<Func<T, object>>> includes = null);


        IQueryable<T> GetAll();
        Task<IQueryable<T>> GetFiltered(Expression<Func<T, bool>> filter, List<Expression<Func<T, object>>> includes = null, bool asNoTracking = false);
        int Remove(Expression<Func<T, bool>> filter, bool logical = true);


        int RemoveRange(IEnumerable<T> items, bool logical = true);
        T Update(Expression<Func<T, bool>> filter, T entity);

        int UpdateWhere<T>(
                Expression<Func<T, bool>> filter,
                T newValues
                ) where T : class;

        Task<PaginationResult<T>> GetByPage(int page, int pageSize, Expression<Func<T, bool>> filter = null,
                List<(Expression<Func<T, object>> OrderBy, bool Ascending)> orderBy = null,
                List<Expression<Func<T, object>>> includes = null, CancellationToken cancellationToken = default);

        int ExecuteQuery(string query, List<ParameterDto> parameters, bool procedure);
        List<T> ExecuteQuery<T>(string query, List<ParameterDto> parameters, bool procedure);
        void CloseConnection();
        string ApplyMigrations();
        Task<bool> TableExistsAsync();
    }
}
