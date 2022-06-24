using OpenTK.Graphics.OpenGL4;

namespace Graphics.Interfaces;

public interface ITexture
{
    int Width { get; }
    int Height { get; }

    void Bind(TextureUnit texture0);
}