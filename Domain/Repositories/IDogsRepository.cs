using Domain.Entities;

namespace Domain.Repositories;

public interface IDogsRepository
{
    public Task<IEnumerable<Dog>> GetAllAsync(
        string? attribute,
        string? order,
        int? pageNumber,
        int? pageSize,
        CancellationToken cancellationToken = default);
    
    public Task<Dog?> GetByNameAllAsync(string name, CancellationToken cancellationToken = default);
    
    void Insert(Dog dog);
    
}