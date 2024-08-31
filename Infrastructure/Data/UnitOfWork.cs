using Core.Entities;
using Core.Interfaces;
using System.Collections.Concurrent;

namespace Infrastructure.Data {
    public class UnitOfWork(StoreContext context) : IUnitOfWork {
        private readonly ConcurrentDictionary<string, object> _repositories = new();
        public async Task<bool> Complete() {
            return await context.SaveChangesAsync() > 0;
        }

        public void Dispose() {
            context.Dispose();
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity {
            var type = typeof(TEntity).Name;
            //we get the repository if not found in the dictionary we add it
            return (IGenericRepository<TEntity>)_repositories.GetOrAdd(type, t => {
                //using this
                var repositoryType = typeof(GenericRepository<>).MakeGenericType(typeof(TEntity));
                return Activator.CreateInstance(repositoryType, context) ?? throw new InvalidOperationException($"Could not create repository instance for {t}");
            });
        }
    }
}
