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