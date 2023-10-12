using Domain.Entities;

namespace Domain.Repositories;

public interface IDogsRepository
{
    Task<IEnumerable<Dog?>> GetAllAsync(string? attribute, string? order, int? pageNumber, int? pageSize, CancellationToken cancellationToken = default);
    void Insert(Dog? dog);
    
}