namespace Services.Abstractions;

public interface IServiceManager
{
    IDogsService DogsService { get; }
}