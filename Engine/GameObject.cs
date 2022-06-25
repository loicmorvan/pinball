namespace Engine;

public class GameObject
{
    public decimal X { get; set; }
    public decimal Y { get; set; }

    public Sprite? Sprite { get; set; }

    protected internal virtual void Step(decimal Δt) { }
}
