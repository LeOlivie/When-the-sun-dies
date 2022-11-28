public interface IItem
{
    public string Name { get; }
    public string Description { get; }
    public int Count { get; }
    public int MaxInStack { get; }
}
