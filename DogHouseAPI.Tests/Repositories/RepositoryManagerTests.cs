using Domain.Repositories;
using Moq;
using Persistence;
using Persistence.Repositories;

namespace DogHouseAPI.Tests.Repositories
{
    public class RepositoryManagerTests
    {
        private readonly IDogsRepository _dogsRepository;
        private readonly IUnitOfWork _unitOfWork;
        
        public RepositoryManagerTests()
        {
            var dbContextMock = new Mock<RepositoryDbContext>();
            var repositoryManager = new RepositoryManager(dbContextMock.Object);
            
            _dogsRepository = repositoryManager.DogsRepository;
            _unitOfWork = repositoryManager.UnitOfWork;
        }
        
        [Fact]
        public void DogsRepository_ShouldReturnValidInstance()
        {
            // Assert
            Assert.NotNull(_dogsRepository);
            Assert.IsType<DogsRepository>(_dogsRepository);
        }

        [Fact]
        public void UnitOfWork_ShouldReturnValidInstance()
        {
            // Assert
            Assert.NotNull(_unitOfWork);
            Assert.IsType<UnitOfWork>(_unitOfWork);
        }
    }
}