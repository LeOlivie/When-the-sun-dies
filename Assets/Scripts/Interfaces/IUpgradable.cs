public interface IUpgradable
{
    public string ObjectName { get; }
    public Item[] UpgradeRequirements { get; }
    public int UpgradeTimeLeft { get; }
    public int UpgradeTimeRequired { get; }
    public uint CurrLevel { get; }
    public uint MaxLevel { get; }
    public bool IsBeingUpgraded { get; }
    public void OnUpgradeEnded();
    public void OnUpgradeStarted();
    public void DecreaseUpgradeTimeLeft();
}
