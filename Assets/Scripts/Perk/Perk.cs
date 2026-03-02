public abstract class Perk
{
    public abstract int Id { get; }
    public virtual void OnEnable() { }
    public virtual void OnDisable() { }
}
