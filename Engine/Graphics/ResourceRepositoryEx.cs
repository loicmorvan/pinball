using Graphics.Interfaces;

namespace Graphics.Implementations;

public static class ResourceRepositoryEx
{
    public static string? GetResourceString(this IResourceRepository @this, string resourceName)
    {
        using var resourceStream = @this.GetResourceStream(resourceName);
        if (resourceStream == null)
        {
            return null;
        }

        using var streamReader = new StreamReader(resourceStream);
        return streamReader.ReadToEnd();
    }
}
