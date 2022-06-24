using OpenTK.Graphics.OpenGL4;
using System.Diagnostics;
using System.Reflection;

namespace Graphics.Implementations;

internal static class Helper
{
    [Conditional("DEBUG")]
    public static void CheckError()
    {
        var error = GL.GetError();
        if (error != ErrorCode.NoError)
        {
            throw new NotSupportedException(error.ToString());
        }
    }

    public static string ReadTextResource(string resourceName)
    {
        var assembly = Assembly.GetExecutingAssembly();

        using var stream = assembly.GetManifestResourceStream(resourceName) ?? throw new NotSupportedException();
        using var reader = new StreamReader(stream);

        return reader.ReadToEnd();
    }
}
