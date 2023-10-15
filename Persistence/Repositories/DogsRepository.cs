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

        public async Task<IEnumerable<Dog>> GetAllAsync(CancellationToken cancellationToken = default)
            => await _dbContext.Dogs.ToListAsync(cancellationToken);

        public async Task<Dog?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
            => await _dbContext.Dogs.FirstOrDefaultAsync(dog => dog.Name == name, cancellationToken);

        public async Task Insert(Dog dog) => await _dbContext.Dogs.AddAsync(dog);
    }
}
