using Domain.Entities;

namespace Tests;

public static class TestDataHelper
{
    public static IEnumerable<Dog> GetFakeDogsList()
    {
        return new List<Dog>
        {
            new() { Name = "Dog1", Color = "Brown", TailLength = 10, Weight = 20 },
            new() { Name = "Dog3", Color = "White", TailLength = 8, Weight = 18 },
            new() { Name = "Dog2", Color = "Black", TailLength = 12, Weight = 25 },
        };
    }
}