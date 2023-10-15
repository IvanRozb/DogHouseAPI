using Domain.Repositories;
using Moq;
using Persistence;
using Persistence.Repositories;

namespace DogHouseAPI.Tests.Repositories;

public class UnitOfWorkTests
{
    private readonly Mock<RepositoryDbContext> _mockContext;
    private readonly IUnitOfWork _unitOfWork;

    public UnitOfWorkTests()
    {
        _mockContext = new Mock<RepositoryDbContext>();
        _unitOfWork = new UnitOfWork(_mockContext.Object);
    }
    
    [Fact]
    public async Task SaveChangesAsync_ShouldSaveChangesInUnitOfWork()
    {
        // Act
        await _unitOfWork.SaveChangesAsync();

        // Assert
        _mockContext.Verify(d => d.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}