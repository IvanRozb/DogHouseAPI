using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Services.Abstractions;

namespace Services;

internal sealed class DogsService : IDogsService
{
    private readonly IRepositoryManager _repositoryManager;
    public DogsService(IRepositoryManager repositoryManager) => _repositoryManager = repositoryManager;

    public async Task CreateAsync(Dog dog, CancellationToken cancellationToken = default)
    {
        var existingDog = await _repositoryManager.DogsRepository.GetByNameAllAsync(dog.Name, cancellationToken);
        if (existingDog is not null)
        {
            throw new ConflictException($"The dog with name: '{dog.Name}' is already exist!");
        }
        
        _repositoryManager.DogsRepository.Insert(dog);
        await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<Dog>> GetAllAsync(string? attribute, string? order, int? pageNumber, int? pageSize,
        CancellationToken cancellationToken = default)
    {
        return await _repositoryManager.DogsRepository.GetAllAsync(attribute, order, pageNumber, pageSize,
            cancellationToken);
    }
}