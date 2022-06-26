/*
 Copyright (c) 2022 Loïc Morvan

 Permission is hereby granted, free of charge, to any person obtaining a copy of
 this software and associated documentation files (the "Software"), to deal in
 the Software without restriction, including without limitation the rights to
 use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
 the Software, and to permit persons to whom the Software is furnished to do so,
 subject to the following conditions:

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.

 THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
 FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
 COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
 IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
 CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

using OpenTK.Graphics.OpenGL4;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Engine.Graphics;

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
