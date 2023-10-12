using Domain.Repositories;
using Services.Abstractions;

namespace Services
{
    public sealed class ServiceManager : IServiceManager
    {
        private readonly Lazy<IDogsService> _lazyDogsService;

        public ServiceManager(IRepositoryManager repositoryManager)
        {
            _lazyDogsService = new Lazy<IDogsService>(() => 
                new DogsService(repositoryManager));
        }

        public IDogsService DogsService => _lazyDogsService.Value;
    }
}
