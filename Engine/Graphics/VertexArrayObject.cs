using Graphics.Interfaces;
using OpenTK.Graphics.OpenGL4;

namespace Graphics.Implementations;

public sealed class VertexArrayObject : IDisposable, IVertexArrayObject
{
    private readonly int vao;

    public VertexArrayObject()
    {
        GL.CreateVertexArrays(1, out vao);
        GL.BindVertexArray(vao);

        GL.CreateBuffers(1, out int positionBufferObject);
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
