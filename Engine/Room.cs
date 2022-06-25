namespace Engine;

public class Room
{
    public Camera Camera { get; set; } = new(0, 0, 800, 600);

    public List<GameObject> GameObjects { get; } = new List<GameObject>();

    public void Step(decimal Δt)
    {
        foreach (var gameObject in GameObjects)
        {
            gameObject.Step(Δt);
        }
    }
}
