using Graphics.Interfaces;
using OpenTK.Graphics.OpenGL4;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Graphics.Implementations;

public sealed class Texture : ITexture, IDisposable
{
    private readonly int texture;

    public unsafe Texture(string textureResourceName, IResourceRepository resourceRepository)
    {
        texture = GL.GenTexture();
        GL.BindTexture(TextureTarget.Texture2D, texture);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

        using var stream = resourceRepository.GetResourceStream(textureResourceName)
            ?? throw new ArgumentException($"Resource {textureResourceName} not found.");
        using var image = Image.Load<Rgba32>(stream);
        (Width, Height) = image.Size();
        image.Mutate(x => x.Flip(FlipMode.Vertical));
        if (image.DangerousTryGetSinglePixelMemory(out var pixels))
        {
            using var pinnedPixels = pixels.Pin();

            GL.TexImage2D(
                TextureTarget.Texture2D, 0,
                PixelInternalFormat.Rgba,
                Width, Height, 0,
                PixelFormat.Rgba, PixelType.UnsignedByte,
                new IntPtr(pinnedPixels.Pointer));
        }
        else
        {
            var buffer = new Rgba32[Width * Height];
            var pixelSpan = new Span<Rgba32>(buffer);
            image.CopyPixelDataTo(pixelSpan);

            GL.TexImage2D(
                TextureTarget.Texture2D, 0,
                PixelInternalFormat.Rgba,
                Width, Height, 0,
                PixelFormat.Rgba, PixelType.UnsignedByte,
                buffer);
        }

        Helper.CheckError();
    }

    public int Width { get; }

    public int Height { get; }

    public void Bind(TextureUnit textureUnit)
    {
        GL.ActiveTexture(textureUnit);
        Helper.CheckError();

        GL.BindTexture(TextureTarget.Texture2D, texture);
        Helper.CheckError();
    }

    ~Texture()
    {
        DoDispose();
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);

        DoDispose();
    }

    private void DoDispose()
    {
        GL.DeleteTexture(texture);
    }
}
