using OpenTK.Graphics.OpenGL4;

namespace Graphics.Interfaces;

public interface IShaderFactory
{
    IShader CreateShader(ShaderType shaderType, string shaderResourceName);
}
