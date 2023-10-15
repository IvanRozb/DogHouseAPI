using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Services.Abstractions;

namespace Services;

internal sealed class DogsService : IDogsService
{
    private readonly IRepositoryManager _repositoryManager;
    public DogsService(IRepositoryManager repositoryManager) => _repositoryManager = repositoryManager;

    public async Task<Dog> CreateAsync(Dog dog, CancellationToken cancellationToken = default)
    {
        var existingDog = await _repositoryManager.DogsRepository.GetByNameAsync(dog.Name, cancellationToken);
        if (existingDog is not null)
        {
            throw new ConflictException($"The dog with name: '{dog.Name}' is already exist!");
        }
        
        await _repositoryManager.DogsRepository.Insert(dog);
        await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
        
        return dog;
    }

    public async Task<IEnumerable<Dog>> GetAllAsync(string? attribute, string? order, int? pageNumber, int? pageSize,
        CancellationToken cancellationToken = default)
    {
        if (attribute is not null)
        {
            var availableAttributes = new List<string> { "name", "color", "tail_length", "weight" };
            if (!availableAttributes.Contains(attribute))
            {
                throw new BadRequestException("attribute must be name, color, tail_length, or weight");
            }
        }
        
        if (order is not null)
        {
            var availableOrders = new List<string> { "asc", "desc"};
            if (!availableOrders.Contains(order))
            {
                throw new BadRequestException("order must be asc or desc");
            }
        }
        
        if (pageNumber < 1)
        {
            throw new BadRequestException("pageNumber must be higher then 0");
        }
        
        if (pageSize < 1)
        {
            throw new BadRequestException("pageSize must be higher then 0");
        }

        return await _repositoryManager.DogsRepository.GetAllAsync(attribute, order, pageNumber, pageSize,
            cancellationToken);
    }
}