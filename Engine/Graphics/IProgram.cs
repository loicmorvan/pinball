using OpenTK.Mathematics;

namespace Graphics.Interfaces;

public interface IProgram
{
    void Use();
    void Uniform1(string v1, int v2);
    void Uniform2(string v, int tileWidth, int tileHeight);
    void Uniform2(string v, float tileWidth, float tileHeight);
    void Uniform2(string v, Vector2i finalDestination);
}