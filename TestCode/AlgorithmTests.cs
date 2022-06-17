namespace TestCode;

public class AlgorithmTests
{
    public static IEnumerable<object[]> IntegrationTestData
    {
        get
        {
            yield return new object[]
            {
                new Vector(0, 0), new Vector(0, 0), new Vector(0, -9.80m), 0.20m, 100,
                new Vector(0, -196.00m), new Vector(0, -1979.60m)
            };
            yield return new object[]
            {
                new Vector(0, 10m), new Vector(0, 0), new Vector(0, -9.80m), 0.20m, 100,
                new Vector(0, -186.00m), new Vector(0, -1779.60m)
            };
        }
    }

    [Theory]
    [MemberData(nameof(IntegrationTestData))]
    public void IntegrationTest(Vector speed, Vector position, Vector gravity, decimal dt, int steps, Vector expectedSpeed, Vector expectedPosition)
    {
        for (int i = 0; i < steps; ++i)
        {
            Vector a = gravity;
            speed += dt * a;
            position += dt * speed;
        }

        Assert.Equal(expectedSpeed, speed);
        Assert.Equal(expectedPosition, position);
    }

    public static IEnumerable<object[]> ContinuousCollisionTestData
    {
        get
        {
            yield return new object[]
            {
                0.5m, 1m, new Vector(0, 0), new Vector(0, 1), 1, new Vector(0, -1), 0.25m,
                new Vector(0, 1), new Vector(0, 0.75m)
            };
            yield return new object[]
            {
                0.5m, 1m, new Vector(0, 0), new Vector(1, 0), 1, new Vector(-1, 0), 0.25m,
                new Vector(1, 0), new Vector(0.75m, 0)
            };
        }
    }

    [Theory]
    [MemberData(nameof(ContinuousCollisionTestData))]
    public void ContinuousCollisionTest(decimal timeToCollision, decimal dt, Vector pointToCollision, Vector normalOfSurface, decimal restitution, Vector speed, decimal radius, Vector expectedSpeed, Vector expectedPosition)
    {
        Vector positionAtCollision = pointToCollision + radius * normalOfSurface;
        Vector tangentAtCollision = new Vector(-normalOfSurface.Y, normalOfSurface.X);
        speed = restitution * (speed * tangentAtCollision * tangentAtCollision - speed * normalOfSurface * normalOfSurface);
        decimal timeRemaining = dt - timeToCollision;
        Vector position = positionAtCollision + timeRemaining * speed;

        Assert.Equal(expectedSpeed, speed);
        Assert.Equal(expectedPosition, position);
    }
}
