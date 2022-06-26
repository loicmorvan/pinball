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

using System.Reflection;
using Engine.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace Engine;

public class Window : GameWindow
{
    private static readonly Version TargetVersion = new(4, 0);
    private readonly ITextureRepository textureRepository;
    private readonly Lazy<IProgram> program;
    private readonly Lazy<IVertexArrayObject> vao;

    public Window(params IResourceRepository[] additionalResourceRepositories) : base(
        new GameWindowSettings
        {
            UpdateFrequency = 60,
            RenderFrequency = 60
        },
        new NativeWindowSettings
        {
            APIVersion = TargetVersion,
            Flags = ContextFlags.ForwardCompatible | ContextFlags.Debug
        })
    {
        if (APIVersion != TargetVersion)
        {
            throw new Exception("Not the required target version.");
        }

        var repo = new CompositeResourceRepository(
            additionalResourceRepositories.Prepend(new AssemblyResourceRepository(Assembly.GetExecutingAssembly())).ToArray());
        textureRepository = new TextureRepository(repo);

        program = new Lazy<IProgram>(() => new Program(
            new Shader(ShaderType.VertexShader, "Engine.Resources.TileMap.vert", repo),
            new Shader(ShaderType.FragmentShader, "Engine.Resources.TileMap.frag", repo)));

        vao = new Lazy<IVertexArrayObject>(() => new VertexArrayObject());
    }

    public Room Room { get; set; } = new();

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);

        var (left, bottom, width, height) = Room.Camera;
        var cameraRatio = height / width;
        var screenRatio = (decimal)ClientSize.Y / ClientSize.X;

        if (cameraRatio <= screenRatio)
        {
            var viewportHeight = (int)(ClientSize.X * height / width);
            GL.Viewport(0, (ClientSize.Y - viewportHeight) / 2, ClientSize.X, viewportHeight);
        }
        else
        {
            var viewportWidth = (int)(ClientSize.Y * width / height);
            GL.Viewport((ClientSize.X - viewportWidth) / 2, 0, viewportWidth, ClientSize.Y);
        }
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);

        Room.Step((decimal)args.Time);
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        GL.ClearColor(1, 1, 1, 1);
        GL.Clear(ClearBufferMask.ColorBufferBit);

        program.Value.Use();
        program.Value.Uniform1("tex", 0);
        var (x, y, w, h) = Room.Camera;
        program.Value.Uniform4("camera", (float)x, (float)y, (float)w, (float)h);
        foreach (var gameObject in Room.GameObjects)
        {
            if (gameObject.Sprite is not null)
            {
                textureRepository.Get(gameObject.Sprite.TextureResource).Bind(TextureUnit.Texture0);
                program.Value.Uniform2("topleft", (float)(gameObject.X - gameObject.Sprite.CenterX), (float)(gameObject.Y - gameObject.Sprite.CenterY));
                program.Value.Uniform2("size", (float)gameObject.Sprite.Width, (float)gameObject.Sprite.Height);
                vao.Value.Draw();
            }
        }

        Helper.CheckError();

        SwapBuffers();
    }
}