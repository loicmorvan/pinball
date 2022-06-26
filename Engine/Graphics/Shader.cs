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

using Graphics.Interfaces;
using OpenTK.Graphics.OpenGL4;

namespace Graphics.Implementations;

public class Shader: IShader
{
    public Shader(ShaderType shaderType, string shaderResourceName, IResourceRepository resourceRepository)
    {
        var code = resourceRepository.GetResourceString(shaderResourceName)
                        ?? throw new ArgumentException($"Resource not found: {shaderResourceName}.");

        ShaderId = GL.CreateShader(shaderType);
        GL.ShaderSource(ShaderId, code);
        GL.CompileShader(ShaderId);
        Helper.CheckError();

        string infoLog = GL.GetShaderInfoLog(ShaderId);
        Helper.CheckError();
        if (infoLog != string.Empty)
        {
            Console.WriteLine(infoLog);
            throw new NotSupportedException(infoLog);
        }
    }

    public int ShaderId { get; }
}