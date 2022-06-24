namespace Graphics.Interfaces;

public interface IResourceRepository
{
    Stream? GetResourceStream(string resourceName);
}
