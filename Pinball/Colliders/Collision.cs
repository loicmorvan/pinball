using Pinball.Math;

namespace Pinball;

public record Collision(decimal δt, Vector p, Vector N, decimal c);
