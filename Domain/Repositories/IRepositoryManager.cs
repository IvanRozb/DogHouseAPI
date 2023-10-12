namespace Domain.Repositories
{
    public interface IRepositoryManager
    {
        IDogsRepository DogsRepository { get; }

        IUnitOfWork UnitOfWork { get; }
    }
}
