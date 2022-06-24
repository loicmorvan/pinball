using Graphics.Implementations;
using Graphics.Interfaces;
using OpenTK.Graphics.OpenGL4;

namespace Engine;

public class ShaderFactory : IShaderFactory
{
    private readonly IResourceRepository resourceRepository;

    public ShaderFactory(IResourceRepository resourceRepository)
    {
        this.resourceRepository = resourceRepository;
    }

    public IShader CreateShader(ShaderType shaderType, string shaderResourceName)
    {
        return new Shader(shaderType, shaderResourceName, resourceRepository);
    }
}