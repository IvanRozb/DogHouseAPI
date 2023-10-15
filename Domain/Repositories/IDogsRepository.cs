using Domain.Entities;

namespace Domain.Repositories;

public interface IDogsRepository
{
    public Task<IEnumerable<Dog>> GetAllAsync(CancellationToken cancellationToken = default);
    
    public Task<Dog?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    
    public Task Insert(Dog dog);
    
}