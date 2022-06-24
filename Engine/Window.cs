using System.Reflection;
using Graphics.Implementations;
using Graphics.Interfaces;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace Engine;

public class Window : GameWindow
{
    private readonly Lazy<IProgram> program;
    private readonly Lazy<IVertexArrayObject> vao;
    private readonly Lazy<ITexture> texture;

    public Window() : base(
        new GameWindowSettings
        {
            UpdateFrequency = 60,
            RenderFrequency = 60
        },
        new NativeWindowSettings
        {
            APIVersion = new Version(4, 0),
            Flags = ContextFlags.ForwardCompatible | ContextFlags.Debug
        })
    {
        var repo = new AssemblyResourceRepository(Assembly.GetExecutingAssembly());
        program = new Lazy<IProgram>(() => new Program(
            new Shader(ShaderType.VertexShader, "Engine.Resources.TileMap.vert", repo),
            new Shader(ShaderType.FragmentShader, "Engine.Resources.TileMap.frag", repo)));
        vao = new Lazy<IVertexArrayObject>(() => new VertexArrayObject());
        texture = new Lazy<ITexture>(() => new Texture("Engine.Resources.Sample.png", repo));
    }

    public Room Room { get; set; } = new();

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);

        Room.Step((decimal)args.Time);
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        GL.Clear(ClearBufferMask.ColorBufferBit);

        program.Value.Use();
        program.Value.Uniform1("tex", 0);
        texture.Value.Bind(TextureUnit.Texture0);
        foreach (var gameObject in Room.GameObjects)
        {
            if (gameObject.Sprite != null)
            {
                program.Value.Uniform2("topleft", (int)gameObject.X, (int)gameObject.Y);
                vao.Value.Draw();
            }
        }

        Helper.CheckError();

        SwapBuffers();
    }
}