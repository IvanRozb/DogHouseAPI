using Domain.Entities;

namespace Services.Abstractions;

public interface IDogsService
{
    public Task<IEnumerable<Dog?>> GetAllAsync(
        string? attribute,
        string? order,
        int? pageNumber,
        int? pageSize,
        CancellationToken cancellationToken = default);
    Task CreateAsync(Dog? dog, CancellationToken cancellationToken = default);
}