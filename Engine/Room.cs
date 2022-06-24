namespace Engine;

public class Room
{
    public List<GameObject> GameObjects { get; } = new List<GameObject>();

    public void Step(decimal Δt)
    {
        foreach (var gameObject in GameObjects)
        {
            gameObject.Step(Δt);
        }
    }
}
