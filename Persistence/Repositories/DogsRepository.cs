using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Persistence.Repositories
{
    public class DogsRepository : IDogsRepository
    {
        private readonly RepositoryDbContext _dbContext;

        public DogsRepository(RepositoryDbContext dbContext) => _dbContext = dbContext;

        public async Task<IEnumerable<Dog>> GetAllAsync(
            string? attribute,
            string? order,
            int? pageNumber,
            int? pageSize,
            CancellationToken cancellationToken = default)
        {
            IQueryable<Dog> query = _dbContext.Dogs;

            if (!string.IsNullOrWhiteSpace(attribute) && attribute is "name" or "color" or "tail_length" or "weight" && !string.IsNullOrWhiteSpace(order) && order is "asc" or "desc")
            {
                query = ApplySorting(query, attribute, order);
            }

            if (pageNumber.HasValue && pageSize.HasValue)
            {
                query = ApplyPagination(query, pageNumber.Value, pageSize.Value);
            }

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<Dog?> GetByNameAllAsync(string name, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Dogs.FirstOrDefaultAsync(dog => dog.Name == name, cancellationToken);
        }

        public void Insert(Dog dog) => _dbContext.Dogs.Add(dog);
        
        private static IQueryable<Dog> ApplySorting(IQueryable<Dog> query, string attribute, string order)
        {
            var attributeToPropertyMap = new Dictionary<string, Expression<Func<Dog, object>>>
            {
                { "name", dog => dog.Name },
                { "color", dog => dog.Color },
                { "tail_length", dog => dog.TailLength },
                { "weight", dog => dog.Weight }
            };

            if (attributeToPropertyMap.TryGetValue(attribute, out var propertyExpression))
            {
                query = order.ToLower() == "desc" ?
                    query.OrderByDescending(propertyExpression) :
                    query.OrderBy(propertyExpression);
            }

            return query;
        }

        private static IQueryable<Dog> ApplyPagination(IQueryable<Dog> query, int pageNumber, int pageSize)
        {
            var itemsToSkip = (pageNumber - 1) * pageSize;

            query = query.Skip(itemsToSkip).Take(pageSize);

            return query;
        }
    }
}
