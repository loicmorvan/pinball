using Graphics.Interfaces;
using System.Reflection;

namespace Graphics.Implementations;

public class AssemblyResourceRepository : IResourceRepository
{
    private readonly Assembly assembly;

    public AssemblyResourceRepository(Assembly assembly)
    {
        this.assembly = assembly;
    }

    public Stream? GetResourceStream(string resourceName)
    {
        return assembly.GetManifestResourceStream(resourceName);
    }
}
