using Graphics.Interfaces;

namespace Engine.Graphics;

public interface ITextureRepository
{
    ITexture Get(string textureResourceName);
}
