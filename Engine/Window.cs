using System.Reflection;
using Graphics.Implementations;
using Graphics.Interfaces;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace Engine;

public class Window : GameWindow
{
    private static readonly Version TargetVersion = new(4, 0);

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
            APIVersion = TargetVersion,
            Flags = ContextFlags.ForwardCompatible | ContextFlags.Debug
        })
    {
        if (APIVersion != TargetVersion)
        {
            throw new Exception("Not the required target version.");
        }

        var repo = new AssemblyResourceRepository(Assembly.GetExecutingAssembly());

        program = new Lazy<IProgram>(() => new Program(
            new Shader(ShaderType.VertexShader, "Engine.Resources.TileMap.vert", repo),
            new Shader(ShaderType.FragmentShader, "Engine.Resources.TileMap.frag", repo)));
        texture = new Lazy<ITexture>(() => new Texture("Engine.Resources.Sample.png", repo));

        vao = new Lazy<IVertexArrayObject>(() => new VertexArrayObject());
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
        var (x, y, w, h) = Room.Camera;
        program.Value.Uniform4("camera", (float)x, (float)y, (float)w, (float)h);
        texture.Value.Bind(TextureUnit.Texture0);
        foreach (var gameObject in Room.GameObjects)
        {
            if (gameObject.Sprite is not null)
            {
                program.Value.Uniform2("topleft", (float)gameObject.X, (float)gameObject.Y);
                program.Value.Uniform2("size", (float)gameObject.Sprite.Width, (float)gameObject.Sprite.Height);
                vao.Value.Draw();
            }
        }

        Helper.CheckError();

        SwapBuffers();
    }
}