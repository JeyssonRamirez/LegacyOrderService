using Core.DataTransferObject;
using LegacyOrderService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Reflection;

namespace Data.Common.Definition
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly DbContext MyDBContext;
        private static SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private bool _blockDbForThreads = false;
        private bool disposed = false;
        public GenericRepository(DbContext kioskDbContext, bool blockDbForThreads = false)
        {
            MyDBContext = kioskDbContext;
            _blockDbForThreads = blockDbForThreads;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    MyDBContext.Dispose();
                }
                this.disposed = true;
            }
        }
        public void CloseConnection()
        {
            var connection = MyDBContext.Database.GetDbConnection();

            if (connection != null && connection.State != System.Data.ConnectionState.Closed)
            {
                connection.Close();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual IQueryable<T> GetAll()
        {
            IQueryable<T> query = MyDBContext.Set<T>();
            query = ApplyStatusNotDeletedFilter(query);
            return query;
        }

        public virtual async Task<int> PermanentlyDeleteAsync(T entity)
        {
            _ = MyDBContext.Remove(entity);
            return await MyDBContext.SaveChangesAsync();
        }

        public virtual void PermanentlyDelete(T entity)
        {
            _ = MyDBContext.Remove(entity);
            MyDBContext.SaveChanges();
        }

        public int UpdateWhere<T>(
            Expression<Func<T, bool>> filter,
            T newValues
            ) where T : class
        {
            if (_blockDbForThreads)
            {
                _semaphore.Wait();
            }

            var dbSet = MyDBContext.Set<T>();
            var entities = dbSet.Where(filter).ToList();

            if (!entities.Any())
            {
                if (_blockDbForThreads)
                {
                    _semaphore.Wait();
                }
                return 0;
            }

            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var entity in entities)
            {
                foreach (var prop in properties)
                {
                    // Skip keys and navigation properties
                    if (!prop.CanWrite || Attribute.IsDefined(prop, typeof(KeyAttribute)))
                        continue;

                    // Skip Id or properties marked with [Key]
                    if (string.Equals(prop.Name, "Id", StringComparison.OrdinalIgnoreCase) ||
                        Attribute.IsDefined(prop, typeof(KeyAttribute)))
                        continue;

                    var newValue = prop.GetValue(newValues);
                    var defaultValue = GetDefault(prop.PropertyType);

                    // Only update non-default values
                    if (newValue != null && !Equals(newValue, defaultValue))
                    {
                        prop.SetValue(entity, newValue);
                    }
                }
            }

            if (_blockDbForThreads)
            {
                _semaphore.Release();
            }
            return MyDBContext.SaveChanges();
        }

        private object? GetDefault(Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }

        public T Update(Expression<Func<T, bool>> filter, T entity)
        {
            T exist = GetOne(filter);
            if (_blockDbForThreads)
            {
                _semaphore.Wait();
            }
            if (exist != null)
            {
                MyDBContext.Entry(exist).CurrentValues.SetValues(entity);
                MyDBContext.SaveChanges();
            }
            else
            {
                throw new InvalidOperationException("The entity was not found.");
            }
            if (_blockDbForThreads)
            {
                _semaphore.Release();
            }
            return exist;
        }

        public virtual async Task<ICollection<T>> FindByAsync(Expression<Func<T, bool>> predicate)
        {
            return await MyDBContext.Set<T>().Where(predicate).ToListAsync();
        }

        public IUnitOfWork UnitOfWork { get; set; }

        public T AddItem(T item)
        {
            if (_blockDbForThreads)
            {
                _semaphore.Wait();
            }
            MyDBContext.Set<T>().Add(item);
            MyDBContext.SaveChanges();
            if (_blockDbForThreads)
            {
                _semaphore.Release();
            }
            return item;
        }

        public IEnumerable<T> AddRangeOfItems(IEnumerable<T> items)
        {
            if (_blockDbForThreads)
            {
                _semaphore.Wait();
            }
            MyDBContext.Set<T>().AddRange(items);
            MyDBContext.SaveChanges();
            if (_blockDbForThreads)
            {
                _semaphore.Release();
            }
            return items;
        }

        public async Task<T> AddItemAsync(T item)
        {
            if (_blockDbForThreads)
            {
                _semaphore.Wait();
            }
            await MyDBContext.Set<T>().AddAsync(item);
            await MyDBContext.SaveChangesAsync();
            if (_blockDbForThreads)
            {
                _semaphore.Release();
            }
            return item;
        }

        public IEnumerable<T> GetForEdit(Expression<Func<T, bool>> filter, IEnumerable<T> entity)
        {
            throw new NotImplementedException();
        }

        public long Count(Expression<Func<T, bool>> filter = null)
        {
            return MyDBContext.Set<T>().Count(filter);
        }

        public T GetOne(Expression<Func<T, bool>> filter, List<Expression<Func<T, object>>> includes = null)
        {
            var query = MyDBContext.Set<T>().Where(filter);
            query = ApplyStatusNotDeletedFilter(query);

            // Apply includes for related entities
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            var item = query.FirstOrDefault();

            // Remove soft-deleted children from included collections
            if (item != null)
            {
                RemoveSoftDeletedFromIncludes(item);
            }
            return item;
        }

        public async Task<IQueryable<T>> GetFiltered(Expression<Func<T, bool>> filter, List<Expression<Func<T, object>>> includes = null, bool asNoTracking = false)
        {
            if (_blockDbForThreads)
            {
                _semaphore.Wait();
            }

            var query = MyDBContext.Set<T>().Where(filter);
            query = ApplyStatusNotDeletedFilter(query);

            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }

            // Apply includes for related entities
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            var list = await query.ToListAsync();
            if (list != null && list.Any())
            {
                // Remove soft-deleted children from included collections
                foreach (var item in list)
                {
                    RemoveSoftDeletedFromIncludes(item);
                }
            }

            query = list.AsQueryable();

            if (_blockDbForThreads)
            {
                _semaphore.Release();
            }
            return query;
        }

        public int Remove(Expression<Func<T, bool>> filter, bool logical = true)
        {
            var entities = GetFiltered(filter).Result.ToList();
            if (_blockDbForThreads)
            {
                _semaphore.Wait();
            }

            if (entities != null && entities.Any())
            {
                foreach (var item in entities)
                {
                    if (logical && item is Entity deleteEntity)
                    {
                        deleteEntity.Status = StatusType.Deleted;

                        var entityEntry = MyDBContext.Entry(item);
                        var entityType = entityEntry.Metadata;

                        foreach (var navigation in entityEntry.Navigations)
                        {
                            if (!navigation.IsLoaded)
                                navigation.Load();

                            var navigationValue = navigation.CurrentValue;
                            var navigationMetadata = navigation.Metadata;

                            if (navigationValue is IEnumerable<object> collection)
                            {
                                foreach (var child in collection)
                                {
                                    ProcessChildEntity(child, navigationMetadata);
                                }
                            }
                            else if (navigationValue is object child)
                            {
                                ProcessChildEntity(child, navigationMetadata);
                            }
                        }

                        entityEntry.State = EntityState.Modified;
                    }
                    else
                    {
                        MyDBContext.Remove(item);
                    }
                }

                if (_blockDbForThreads)
                {
                    _semaphore.Release();
                }

                return MyDBContext.SaveChanges();
            }

            if (_blockDbForThreads)
            {
                _semaphore.Release();
            }

            return 0;
        }

        public async Task<T> GetOneAsync(Expression<Func<T, bool>> filter, List<Expression<Func<T, object>>> includes = null)
        {
            var query = MyDBContext.Set<T>().Where(filter);

            // Apply includes for related entities
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            query = ApplyStatusNotDeletedFilter(query);
            return await query.FirstOrDefaultAsync();
        }

        public int RemoveRange(IEnumerable<T> items, bool logical = true)
        {
            var entities = items?.ToList();

            if (_blockDbForThreads)
            {
                _semaphore.Wait();
            }

            if (entities != null && entities.Any())
            {
                foreach (var item in entities)
                {
                    if (logical && item is Entity deleteEntity)
                    {
                        deleteEntity.Status = StatusType.Deleted;

                        var entityEntry = MyDBContext.Entry(item);

                        foreach (var navigation in entityEntry.Navigations)
                        {
                            if (!navigation.IsLoaded)
                                navigation.Load();

                            var navigationValue = navigation.CurrentValue;
                            var navigationMetadata = navigation.Metadata;

                            if (navigationValue is IEnumerable<object> collection)
                            {
                                foreach (var child in collection)
                                {
                                    ProcessChildEntity(child, navigationMetadata);
                                }
                            }
                            else if (navigationValue is object child)
                            {
                                ProcessChildEntity(child, navigationMetadata);
                            }
                        }

                        entityEntry.State = EntityState.Modified;
                    }
                    else
                    {
                        MyDBContext.Remove(item);
                    }
                }

                if (_blockDbForThreads)
                {
                    _semaphore.Release();
                }

                return MyDBContext.SaveChanges();
            }

            if (_blockDbForThreads)
            {
                _semaphore.Release();
            }

            return 0;
        }

        public async Task<PaginationResult<T>> GetByPage(int page, int pageSize, Expression<Func<T, bool>> filter = null,
            List<(Expression<Func<T, object>> OrderBy, bool Ascending)> orderBy = null,
            List<Expression<Func<T, object>>> includes = null,
            CancellationToken cancellationToken = default)
        {
            var query = MyDBContext.Set<T>().AsQueryable();
            query = ApplyStatusNotDeletedFilter(query);

            // Apply includes for related entities
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            IOrderedQueryable<T> queryOrdered = null;

            // Apply ordering based on multiple columns
            if (orderBy != null && orderBy.Any())
            {
                // Start with the first ordering
                var firstOrderBy = orderBy.First();

                if (firstOrderBy.Ascending)
                {
                    queryOrdered = query.OrderBy(firstOrderBy.OrderBy);
                }
                else
                {
                    queryOrdered = query.OrderByDescending(firstOrderBy.OrderBy);
                }

                // Apply any additional ordering
                foreach (var additionalOrder in orderBy.Skip(1))
                {

                    if (additionalOrder.Ascending)
                    {
                        queryOrdered = queryOrdered.ThenBy(additionalOrder.OrderBy);
                    }
                    else
                    {
                        queryOrdered = queryOrdered.ThenByDescending(additionalOrder.OrderBy);
                    }
                }

                query = queryOrdered;
            }

            // Fetch the items for the requested page
            if (filter != null)
            {
                query = query.Where(filter);
            }

            // Calculate total items 
            var totalItems = await query.CountAsync();
            // Calculate total pages
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            var result = new PaginationResult<T>()
            {
                TotalPages = totalPages,
                TotalRecords = totalItems,
            };

            // Paging
            var pagedList = await query.Skip((page - 1) * pageSize)
                                        .Take(pageSize)
                                        .ToListAsync(cancellationToken);

            // Remove soft-deleted children from included collections
            foreach (var item in pagedList)
            {
                RemoveSoftDeletedFromIncludes(item);
            }

            result.Data = pagedList.AsQueryable();
            return result;
        }

        public int ExecuteQuery(string query, List<ParameterDto> parameters, bool procedure)
        {
            var sqlParameters = new List<SqlParameter>();
            if (parameters != null)
            {
                foreach (var item in parameters)
                {
                    sqlParameters.Add(new SqlParameter
                    {
                        ParameterName = item.ParameterName,
                        Value = item.Value,
                        DbType = ((System.Data.DbType)item.DbType)
                    });
                }
                return MyDBContext.Database.ExecuteSqlRaw(query, parameters.ToArray());
            }
            return MyDBContext.Database.ExecuteSqlRaw(query);
        }

        public List<T> ExecuteQuery<T>(string query, List<ParameterDto> parameters, bool procedure)
        {
            var sqlParameters = new List<SqlParameter>();
            var counter = 0;
            if (parameters != null)
            {
                foreach (var item in parameters)
                {
                    sqlParameters.Add(new SqlParameter
                    {
                        ParameterName = item.ParameterName,
                        Value = item.Value,
                        DbType = ((System.Data.DbType)item.DbType)
                    });
                    if (counter == 0)
                    {
                        query = query + " " + item.ParameterName;
                    }
                    else
                    {
                        query = query + "," + item.ParameterName;
                    }
                    counter++;
                }
                return MyDBContext.Database.SqlQueryRaw<T>(query, sqlParameters.ToArray()).ToList();
            }
            return MyDBContext.Database.SqlQueryRaw<T>(query).ToList();
        }

        public string ApplyMigrations()
        {
            try
            {

                var pendingMigrations = MyDBContext.Database.GetPendingMigrations().ToList();

                if (pendingMigrations.Any())
                {
                    string latestMigration = pendingMigrations.Last();  // Get the last migration
                    MyDBContext.Database.Migrate();
                    // Applies all pending migrations up to the latest one
                    return $"Database updated to the latest migration: {latestMigration}";
                }
                return "Database is already up to date!";
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> TableExistsAsync()
        {
            var tableName = MyDBContext.Model.FindEntityType(typeof(T)).GetTableName();
            var connection = MyDBContext.Database.GetDbConnection();

            try
            {
                await connection.OpenAsync();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{tableName}'";
                    var result = await command.ExecuteScalarAsync();

                    if (result != null && int.TryParse(result.ToString(), out int count))
                    {
                        return count > 0;
                    }

                    return false;
                }
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        private IQueryable<T> ApplyStatusNotDeletedFilter(IQueryable<T> query)
        {
            // Check if entity has a 'Status' property
            var statusProp = typeof(T).GetProperty("Status");
            if (statusProp != null && statusProp.PropertyType == typeof(int))
            {
                var parameter = Expression.Parameter(typeof(T), "e");
                var property = Expression.Property(parameter, "Status");
                var deletedValue = Expression.Constant((int)StatusType.Deleted);
                var notEqual = Expression.NotEqual(property, deletedValue);
                var lambda = Expression.Lambda<Func<T, bool>>(notEqual, parameter);
                query = query.Where(lambda);
            }

            return query;
        }

        private static void RemoveSoftDeletedFromIncludes(T entity)
        {
            if (entity == null)
            {
                return;
            }

            var type = typeof(T);
            foreach (var navProp in type.GetProperties().Where(p => typeof(IEnumerable<object>).IsAssignableFrom(p.PropertyType) && p.PropertyType != typeof(string)))
            {
                var collection = navProp.GetValue(entity) as IEnumerable<object>;
                if (collection == null) continue;

                var filtered = collection
                    .Where(item =>
                    {
                        var statusProp = item.GetType().GetProperty("Status");
                        if (statusProp == null || statusProp.PropertyType != typeof(int))
                            return true; // no Status field

                        var statusVal = (int)statusProp.GetValue(item);
                        return statusVal != (int)StatusType.Deleted;
                    })
                    .ToList();

                // Replace the collection with the filtered one
                var targetListProp = navProp.GetValue(entity);
                if (targetListProp is IList<object> list)
                {
                    list.Clear();
                    foreach (var item in filtered)
                        list.Add(item);
                }
                else if (navProp.PropertyType.IsGenericType && navProp.PropertyType.GetGenericTypeDefinition() == typeof(ICollection<>))
                {
                    var genericListType = typeof(List<>).MakeGenericType(navProp.PropertyType.GetGenericArguments()[0]);
                    var newList = Activator.CreateInstance(genericListType);
                    foreach (var item in filtered)
                        ((IList)newList).Add(item);
                    navProp.SetValue(entity, newList);
                }
            }
        }

        private void ProcessChildEntity(object child, INavigationBase navigationMetadata)
        {
            if (child == null || navigationMetadata == null)
            {
                return;
            }

            var fk = navigationMetadata is INavigation nav ? nav.ForeignKey : (navigationMetadata as ISkipNavigation)?.ForeignKey;
            if (fk == null)
            {
                return; // Cannot process without FK details
            }
            var dependentProps = fk.Properties;
            bool isNullable = dependentProps.All(p => p.IsNullable);

            if (isNullable)
            {
                // Break the relationship by setting FK(s) to null
                foreach (var prop in dependentProps)
                {
                    var propInfo = child.GetType().GetProperty(prop.Name);
                    if (propInfo != null && propInfo.CanWrite)
                    {
                        propInfo.SetValue(child, null);
                    }
                }
            }
            else if (child is Entity deleteChild)
            {
                deleteChild.Status = StatusType.Deleted;
                MyDBContext.Entry(child).State = EntityState.Modified;
            }
        }
    }
}
