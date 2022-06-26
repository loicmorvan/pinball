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

public sealed class VertexArrayObject : IDisposable, IVertexArrayObject
{
    private readonly int vao;

    public VertexArrayObject()
    {
        vao = GL.GenVertexArray();
        GL.BindVertexArray(vao);

        var positionBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, positionBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, 12 * sizeof(float), new[]
        {
            0f, 0f,
            0f, 1f,
            1f, 0f,
            1f, 0f,
            0f, 1f,
            1f, 1f,
        }, BufferUsageHint.StaticDraw);

        GL.EnableVertexAttribArray(0);
        GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 0);
        Helper.CheckError();
    }

    public void Draw()
    {
        GL.BindVertexArray(vao);
        Helper.CheckError();

        GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
        Helper.CheckError();
    }

    ~VertexArrayObject()
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
        GL.DeleteVertexArray(vao);
    }
}
