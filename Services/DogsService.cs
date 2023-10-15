using System.Linq.Expressions;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Services.Abstractions;

namespace Services;

public class DogsService : IDogsService
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
        var query = (await _repositoryManager.DogsRepository.GetAllAsync(cancellationToken)).AsQueryable();
        
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
        
        if (!string.IsNullOrWhiteSpace(attribute) && !string.IsNullOrWhiteSpace(order))
        {
            query = ApplySorting(query, attribute, order);
        }
        
        if (pageNumber < 1)
        {
            throw new BadRequestException("pageNumber must be higher then 0");
        }
        
        if (pageSize < 1)
        {
            throw new BadRequestException("pageSize must be higher then 0");
        }

        if (pageNumber.HasValue && pageSize.HasValue)
        {
            query = ApplyPagination(query, pageNumber.Value, pageSize.Value);
        }

        return query.AsEnumerable();
    }
    
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