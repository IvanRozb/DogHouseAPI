using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using Persistence;
using Persistence.Repositories;

namespace DogHouseAPI.Tests.Repositories;

public class DogsRepositoryTests
{
    private readonly Mock<RepositoryDbContext> _mockContext;
    private readonly IEnumerable<Dog> _fakedDogs;
    private readonly IDogsRepository _dogsRepository;
    
    public DogsRepositoryTests()
    {
        _mockContext = new Mock<RepositoryDbContext>();
        _fakedDogs = TestDataHelper.GetFakeDogsList();
        _mockContext.Setup<DbSet<Dog>>(x => x.Dogs)
            .ReturnsDbSet(_fakedDogs);
        _dogsRepository = new DogsRepository(_mockContext.Object);
    }
    
    [Fact]
    public async Task GetAllAsync_WithoutParameters_ReturnsAllDogs()
    {
        // Act
        var result = await _dogsRepository.GetAllAsync(null, null, null, null);

        // Assert
        Assert.Equal(_fakedDogs, result);
    }
    
    [Theory]
    [InlineData("name", "asc")]
    [InlineData("name", "desc")]
    [InlineData("color", "asc")]
    [InlineData("color", "desc")]
    [InlineData("tail_length", "asc")]
    [InlineData("tail_length", "desc")]
    [InlineData("weight", "asc")]
    [InlineData("weight", "desc")]
    public async Task GetAllAsync_WithSorting_ReturnsDogsInCorrectOrder(string attribute, string order)
    {
        // Act
        IOrderedEnumerable<Dog> expectedResult;
    
        if (order == "asc")
        {
            expectedResult = attribute switch
            {
                "name" => _fakedDogs.OrderBy(s => s.Name),
                "color" => _fakedDogs.OrderBy(s => s.Color),
                "tail_length" => _fakedDogs.OrderBy(s => s.TailLength),
                "weight" => _fakedDogs.OrderBy(s => s.Weight),
            };
        }
        else
        {
            expectedResult = attribute switch
            {
                "name" => _fakedDogs.OrderByDescending(s => s.Name),
                "color" => _fakedDogs.OrderByDescending(s => s.Color),
                "tail_length" => _fakedDogs.OrderByDescending(s => s.TailLength),
                "weight" => _fakedDogs.OrderByDescending(s => s.Weight),
            };
        }
    
        var result = await _dogsRepository.GetAllAsync(attribute, order, null, null);

        // Assert
        Assert.Equal(expectedResult, result);
    }

    
    [Theory]
    [InlineData("name")]
    [InlineData("color")]
    [InlineData("tail_length")]
    [InlineData("weight")]
    public async Task GetAllAsync_WithInvalidSortingOrder_ReturnsUnsortedDogs(string attribute)
    {
        // Act
        var result = await _dogsRepository.GetAllAsync(attribute, "invalidOrder", null, null);

        // Assert
        Assert.Equal(_fakedDogs, result);
    }
    
    [Theory]
    [InlineData("asc")]
    [InlineData("desc")]
    public async Task GetAllAsync_WithInvalidSortingAttribute_ReturnsUnsortedDogs(string order)
    {
        // Act
        var result = await _dogsRepository.GetAllAsync("invalidAttribute", order, null, null);

        // Assert
        Assert.Equal(_fakedDogs, result);
    }
    
    [Theory]
    [InlineData(1,0)]
    [InlineData(1,1)]
    [InlineData(1,2)]
    [InlineData(1,3)]
    [InlineData(2,2)]
    [InlineData(3,0)]
    [InlineData(3,1)]
    [InlineData(100, 1)]
    [InlineData(1, 100)]
    [InlineData(0, 0)]
    public async Task GetAllAsync_WithPagination_ReturnsCorrectPageOfDogs(int pageNumber, int pageSize)
    {
        // Act
        var itemsToSkip = (pageNumber - 1) * pageSize;
        var expectedResult = _fakedDogs.AsQueryable().Skip(itemsToSkip).Take(pageSize);
        var result = await _dogsRepository.GetAllAsync(null, null, pageNumber, pageSize);

        // Assert
        Assert.Equal(expectedResult, result);
    }
    
    [Theory]
    [InlineData(-1,0)]
    [InlineData(0,-1)]
    public async Task GetAllAsync_WithInvalidPagination_ReturnsEmptyList(int pageNumber, int pageSize)
    {
        // Act
        var result = await _dogsRepository.GetAllAsync(null, null, pageNumber, pageSize);

        // Assert
        Assert.Empty(result);
    }
    
    [Fact]
    public async Task GetAllAsync_WithSortingAndPagination_ReturnsSortedAndPaginatedDogs()
    {
        // Arrange
        var expectedResult = _fakedDogs.OrderBy(d => d.Name).Skip(1).Take(1);

        // Act
        var result = await _dogsRepository.GetAllAsync("name", "asc", 2, 1);

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public async Task GetAllAsync_WithSortingAndPagination_InvalidSortingAttribute_ReturnsSortedDogs()
    {
        // Act
        var expectedResult = _fakedDogs.ElementAt(1);
        var result = await _dogsRepository.GetAllAsync("InvalidAttribute", "asc", 2, 1);

        // Assert
        Assert.Equal(expectedResult, result.Single());
    }
    
    [Fact]
    public async Task GetAllAsync_WithSortingAndPagination_InvalidSortingOrder_ReturnsPaginatedDogs()
    {
        // Act
        var result = await _dogsRepository.GetAllAsync("Name", "InvalidOrder", 2, 1);

        // Assert
        Assert.NotEqual(_fakedDogs, result);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task GetAllAsync_WithSortingAndPagination_ReturnsSortedAndPaginatedDogs_WithCancellationToken(bool tokenExists)
    {
        // Arrange
        var expectedResult = _fakedDogs.OrderBy(d => d.Name).Skip(1).Take(1);
        var cancellationToken = tokenExists ? CancellationToken.None : new CancellationToken();

        // Act
        var result = await _dogsRepository.GetAllAsync("name", "asc", 2, 1, cancellationToken);

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public async Task GetByNameAllAsync_ExistingName_ReturnsDog()
    {
        // Arrange
        const string dogName = "Dog1";
        var expectedDog = _fakedDogs.FirstOrDefault(f => f.Name == dogName);

        // Act
        var result = await _dogsRepository.GetByNameAsync(dogName);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedDog, result);
    }
    
    [Fact]
    public async Task GetByNameAllAsync_NonExistingName_ReturnsNull()
    {
        // Act
        var result = await _dogsRepository.GetByNameAsync("NonExistingName");

        // Assert
        Assert.Null(result);
    }
    
    [Fact]
    public async Task Insert_AddsDogToDbContext()
    {
        // Arrange

        var dog = new Dog
        {
            Name = "Dog4",
            Color = "red",
            TailLength = 1,
            Weight = 10
        };

        // Act
        await _dogsRepository.Insert(dog);

        // Assert
        _mockContext.Verify(d => d.Dogs.AddAsync(dog, It.IsAny<CancellationToken>()), Times.Once);
    }
}