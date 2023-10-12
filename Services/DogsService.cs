using Domain.Entities;
using Domain.Repositories;
using Services.Abstractions;

namespace Services;

internal sealed class DogsService : IDogsService
{
    private readonly IRepositoryManager _repositoryManager;
    public DogsService(IRepositoryManager repositoryManager) => _repositoryManager = repositoryManager;

    public async Task CreateAsync(Dog? dog, CancellationToken cancellationToken = default)
    {
        _repositoryManager.DogsRepository.Insert(dog);
        await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<Dog?>> GetAllAsync(string? attribute, string? order, int? pageNumber, int? pageSize,
        CancellationToken cancellationToken = default)
    {
        return await _repositoryManager.DogsRepository.GetAllAsync(attribute, order, pageNumber, pageSize,
            cancellationToken);
    }
}