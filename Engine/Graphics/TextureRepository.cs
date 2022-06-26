using Graphics.Implementations;
using Graphics.Interfaces;

namespace Engine.Graphics;

public class TextureRepository : ITextureRepository
{
    private readonly IResourceRepository resourceRepository;
    private IDictionary<string, ITexture> textures = new Dictionary<string, ITexture>();

    public TextureRepository(IResourceRepository resourceRepository)
    {
        this.resourceRepository = resourceRepository;
    }

    public ITexture Get(string textureResourceName)
    {
        if (textures.TryGetValue(textureResourceName, out var texture))
        {
            return texture;
        }

        texture = new Texture(textureResourceName, resourceRepository);
        textures.Add(textureResourceName, texture);

        return texture;
    }
}