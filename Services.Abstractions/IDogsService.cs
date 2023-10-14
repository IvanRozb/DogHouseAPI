using Domain.Entities;

namespace Services.Abstractions;

public interface IDogsService
{
    public Task<IEnumerable<Dog>> GetAllAsync(
        string? attribute,
        string? order,
        int? pageNumber,
        int? pageSize,
        CancellationToken cancellationToken = default);
    public Task<Dog> CreateAsync(Dog dog, CancellationToken cancellationToken = default);
}