using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Moq;
using Services;

namespace DogHouseAPI.Tests.Services;

public class DogsServiceTests
{
    private readonly DogsService _dogsService;
    private readonly Mock<IRepositoryManager> _repositoryManager;
    private readonly IEnumerable<Dog> _fakeDogs;

    public DogsServiceTests()
    {
        _repositoryManager = new Mock<IRepositoryManager>();
        _dogsService = new DogsService(_repositoryManager.Object);
        _fakeDogs = TestDataHelper.GetFakeDogsList();

        var unitOfWork = new Mock<IUnitOfWork>();
        var dogsRepository = new Mock<IDogsRepository>();
        
        dogsRepository.Setup(r => r.GetAllAsync(CancellationToken.None))
            .ReturnsAsync(_fakeDogs);
        unitOfWork.Setup(u => u.SaveChangesAsync(new CancellationToken()));
        
        _repositoryManager.Setup(r => r.UnitOfWork).Returns(unitOfWork.Object);
        _repositoryManager.Setup(r => r.DogsRepository).Returns(dogsRepository.Object);
    }
    
    [Fact]
    public async Task GetAllAsync_WithoutParameters_ReturnsAllDogs()
    {
        // Act
        var result = await _dogsService.GetAllAsync(null, null, null, null);

        // Assert
        Assert.Equal(_fakeDogs, result);
    }
    
    [Fact]
    public async Task GetAllAsync_WithInvalidAttribute_ThrowsBadRequestException()
    {
        // Arrange
        const string attribute = "invalidAttribute";

        // Act and Assert
        await Assert.ThrowsAsync<BadRequestException>(() =>
            _dogsService.GetAllAsync(attribute, "asc", 1, 10));
    }

    [Fact]
    public async Task GetAllAsync_WithInvalidOrder_ThrowsBadRequestException()
    {
        // Arrange
        const string order = "invalidOrder";

        // Act and Assert
        await Assert.ThrowsAsync<BadRequestException>(() =>
            _dogsService.GetAllAsync("name", order, 1, 10));
    }

    [Theory]
    [InlineData(-1, 10)]
    [InlineData(1, -10)]
    [InlineData(0, 10)]
    [InlineData(1, 0)]
    public async Task GetAllAsync_WithInvalidPageParameters_ThrowsBadRequestException(int pageNumber, int pageSize)
    {
        // Act and Assert
        await Assert.ThrowsAsync<BadRequestException>(() =>
            _dogsService.GetAllAsync("name", "asc", pageNumber, pageSize));
    }
    
    [Fact]
    public async Task GetAllAsync_WithSortingAndPagination_ReturnsSortedAndPaginatedDogs()
    {
        // Act
        var result = await _dogsService.GetAllAsync("name", "asc", 1, 2);

        // Assert
        var expectedResult = _fakeDogs.OrderBy(d => d.Name).Skip(0).Take(2);
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public async Task CreateAsync_WithNonExistingDog_ShouldCreateAndReturnDog()
    {
        // Arrange
        var newDog = new Dog { Name = "Fido" };
     
        var dogsRepository = new Mock<IDogsRepository>();
        dogsRepository.Setup(r => r.GetByNameAsync(newDog.Name, new CancellationToken()))
            .ReturnsAsync((Dog)null!);

        _repositoryManager.Setup(r => r.DogsRepository).Returns(dogsRepository.Object);

        // Act
        var createdDog = await _dogsService.CreateAsync(newDog);

        // Assert
        Assert.Equal(newDog, createdDog);
    }
    
    [Fact]
     public async Task CreateAsync_WithExistingDog_ShouldThrowConflictException()
     {
         // Arrange
         var existingDog = new Dog { Name = "Fido" };
         var newDog = new Dog { Name = "Fido" };
     
         var dogsRepository = new Mock<IDogsRepository>();
         dogsRepository.Setup(r => r.GetByNameAsync(newDog.Name, new CancellationToken()))
             .ReturnsAsync(existingDog);
     
         _repositoryManager.Setup(r => r.DogsRepository).Returns(dogsRepository.Object);
     
         // Act and Assert
         await Assert.ThrowsAsync<ConflictException>(() => _dogsService.CreateAsync(newDog));
     }
}