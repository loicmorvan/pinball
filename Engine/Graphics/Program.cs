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
using OpenTK.Mathematics;

namespace Engine.Graphics;

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
        : this(shaderFactory.CreateShader(ShaderType.VertexShader, vertexShaderResourceName), shaderFactory.CreateShader(ShaderType.FragmentShader, fragmentShaderResourceName))
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

    public void Uniform4(string name, float v1, float v2, float v3, float v4)
    {
        var location = GL.GetUniformLocation(program, name);
        GL.Uniform4(location, v1, v2, v3, v4);
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
