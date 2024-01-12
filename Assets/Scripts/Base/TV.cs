using System;
using UnityEngine;

[Serializable]
public class TV : IUpgradable
{
    [Serializable]
    private struct Upgrades
    {
        [SerializeField] private Item[] _upgradeRequierements;
        [SerializeField] private IUpgradable.SkillRequirement[] _skillRequirements;
        [SerializeField] private uint _upgradeTime;
        [SerializeField] private Sprite _upgradedSprite;
        [SerializeField] private sbyte _happinessBoost;

        public Item[] UpgradeRequierements => _upgradeRequierements;
        public IUpgradable.SkillRequirement[] SkillRequirements => _skillRequirements;
        public uint UpgradeTime => _upgradeTime;
        public Sprite UpgradedSprite => _upgradedSprite;
        public sbyte HappinessBoost => _happinessBoost;
    }

    [SerializeField] private string _name;
    [SerializeField] private Upgrades[] _upgrades;
    private int _upgradeTimeLeft;
    private uint _currLevel;
    private bool _isBeingUpgraded;
    private ushort _watchTime;

    public delegate void OnUpgradedDelegate();
    public event OnUpgradedDelegate OnUpgradedEvent;

    public string ObjectName => _name;
    public Item[] UpgradeRequirements => _upgrades[_currLevel].UpgradeRequierements;
    public IUpgradable.SkillRequirement[] SkillRequirements => _upgrades[_currLevel].SkillRequirements;
    public int UpgradeTimeLeft => _upgradeTimeLeft;
    public int UpgradeTimeRequired => (int)_upgrades[_currLevel].UpgradeTime;
    public sbyte HappinessBoost => _upgrades[_currLevel].HappinessBoost;
    public uint CurrLevel => _currLevel;
    public uint MaxLevel => (uint)(_upgrades.Length - 1);
    public bool IsBeingUpgraded => _isBeingUpgraded;
    public Sprite UpgradedSprite => _upgrades[_currLevel].UpgradedSprite;
    public ushort WatchTime => _watchTime;

    public void DecreaseUpgradeTimeLeft()
    {
        _upgradeTimeLeft--;
    }

    public void OnUpgradeEnded()
    {
        _currLevel += 1;
        OnUpgradedEvent?.Invoke();
        _isBeingUpgraded = false;
    }

    public void OnUpgradeStarted()
    {
        _isBeingUpgraded = true;
        _upgradeTimeLeft = (int)_upgrades[_currLevel].UpgradeTime;
    }

    public void AddWatchTime()
    {
        _watchTime += 1;

        if (_watchTime >= 60)
        {
            _watchTime = 0;
            GlobalRepository.PlayerVars.Happiness += HappinessBoost;
        }
    }

    public void LoadSaveData(SaveDatas.TVSaveData saveData)
    {
        _currLevel = saveData.CurrLevel;
        _upgradeTimeLeft = saveData.UpgradeTimeLeft;
        _isBeingUpgraded = saveData .IsBeingUpgraded;
        _watchTime = saveData.WatchTime;
    }
}
