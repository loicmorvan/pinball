using Graphics.Interfaces;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Graphics.Implementations;

public sealed class Program : IProgram, IDisposable
{
    private readonly int program;

    public Program(IShader vertexShader, IShader fragmentShader)
    {
        program = GL.CreateProgram();
        Helper.CheckError();

        GL.AttachShader(program, vertexShader.ShaderId);
        GL.AttachShader(program, fragmentShader.ShaderId);
        Helper.CheckError();

        GL.LinkProgram(program);
        Helper.CheckError();

        GL.ValidateProgram(program);
        Helper.CheckError();

        GL.UseProgram(program);
        Helper.CheckError();
    }

    public Program(string vertexShaderResourceName, string fragmentShaderResourceName, IShaderFactory shaderFactory)
        :this (shaderFactory.CreateShader(ShaderType.VertexShader, vertexShaderResourceName), shaderFactory.CreateShader(ShaderType.FragmentShader, fragmentShaderResourceName))
    { }

    public void Use()
    {
        GL.UseProgram(program);
        Helper.CheckError();
    }

    public void Uniform1(string name, int value)
    {
        var location = GL.GetUniformLocation(program, name);
        GL.Uniform1(location, value);
        Helper.CheckError();
    }

    public void Uniform2(string name, int value1, int value2)
    {
        var location = GL.GetUniformLocation(program, name);
        GL.Uniform2(location, value1, value2);
        Helper.CheckError();
    }

    public void Uniform2(string name, Vector2i value)
    {
        Uniform2(name, value.X, value.Y);
    }

    public void Uniform2(string name, float value1, float value2)
    {
        var location = GL.GetUniformLocation(program, name);
        GL.Uniform2(location, value1, value2);
        Helper.CheckError();
    }

    public void Uniform2(string name, Vector2 value)
    {
        Uniform2(name, value.X, value.Y);
    }

    ~Program()
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
        GL.DeleteProgram(program);
    }
}
