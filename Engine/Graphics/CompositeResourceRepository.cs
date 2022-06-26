using Graphics.Interfaces;

namespace Engine.Graphics;

public class CompositeResourceRepository : IResourceRepository
{
    private readonly IResourceRepository[] resourceRepositories;

    public CompositeResourceRepository(params IResourceRepository[] resourceRepositories)
    {
        this.resourceRepositories = resourceRepositories;
    }

    public Stream? GetResourceStream(string resourceName)
    {
        foreach (var resourceRepository in resourceRepositories)
        {
            var stream = resourceRepository.GetResourceStream(resourceName);
            if (stream != null)
            {
                return stream;
            }
        }

        return null;
    }
}