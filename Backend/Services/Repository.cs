using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Backend.Services
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly StoreDbContext _storeDbContext;
        private readonly ILogger<Repository<T>> _logger;

        public Repository(StoreDbContext context, ILogger<Repository<T>> logger)
        {
            _storeDbContext = context;
            _logger = logger;
        }

        public async Task<IList<T>> GetAll()
        {
            _logger.LogInformation($"Fetching entity list of type {typeof(T)} from the database.");
            var query = _storeDbContext.Set<T>().ToListAsync();

            return await query;
        }

        public async Task Add(T entity)
        {
            _logger.LogInformation($"Adding object of type {entity.GetType()}");
            await _storeDbContext.AddAsync(entity);
        }

        public void Remove(T entity)
        {
            _logger.LogInformation($"Deleting object of type { entity.GetType()}");
            _storeDbContext.Remove(entity);
        }

        public void Update(T entity)
        {
            _logger.LogInformation($"Updating object of type {entity.GetType()}");
            _storeDbContext.Entry(entity).State = EntityState.Modified;
        }

        public async Task<bool> Save()
        {
            _logger.LogInformation($"Saving Changes");
            return (await _storeDbContext.SaveChangesAsync()) >= 0;
        }
    }
}