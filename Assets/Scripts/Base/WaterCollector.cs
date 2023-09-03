using System;
using UnityEngine;

[Serializable]
public class WaterCollector : IUpgradable
{
    [Serializable]
    private struct Upgrades
    {
        [SerializeField] private Item[] _upgradeRequierements;
        [SerializeField] private IUpgradable.SkillRequirement[] _skillRequirements;
        [SerializeField] private uint _upgradeTime;
        [SerializeField] private Sprite _upgradedSprite;
        [SerializeField, Tooltip("Time to collect 1 bottle of water. Measured in minutes.")] private ushort _timeToCollectWater;
        [SerializeField] private ushort _maxWaterAmount;

        public Item[] UpgradeRequierements => _upgradeRequierements;
        public IUpgradable.SkillRequirement[] SkillRequirements => _skillRequirements;
        public uint UpgradeTime => _upgradeTime;
        public Sprite UpgradedSprite => _upgradedSprite;
        public ushort TimeToCollectWater => _timeToCollectWater;
        public ushort MaxWaterAmount => _maxWaterAmount;
    }

    [SerializeField] private string _name;
    [SerializeField] private Upgrades[] _upgrades;
    private int _upgradeTimeLeft;
    private uint _currLevel;
    private bool _isBeingUpgraded;
    private ushort _waterCollected = 0;
    private uint _waterCollectionStart;

    public string ObjectName => _name;
    public Item[] UpgradeRequirements => _upgrades[_currLevel].UpgradeRequierements;
    public int UpgradeTimeLeft => _upgradeTimeLeft;
    public int UpgradeTimeRequired => (int)_upgrades[_currLevel].UpgradeTime;
    public ushort TimeToCollectWater => _upgrades[_currLevel].TimeToCollectWater;
    public uint CurrLevel => _currLevel;
    public uint MaxLevel => (uint)(_upgrades.Length - 1);
    public bool IsBeingUpgraded => _isBeingUpgraded;
    public Sprite UpgradedSprite => _upgrades[_currLevel].UpgradedSprite;
    public ushort WaterCollected => _waterCollected;
    public ushort MaxWaterAmount => _upgrades[_currLevel].MaxWaterAmount;
    public uint WaterCollectionStart => _waterCollectionStart;
    public IUpgradable.SkillRequirement[] SkillRequirements => _upgrades[_currLevel].SkillRequirements;

    public delegate void OnUpgradedDelegate();
    public event OnUpgradedDelegate OnUpgradedEvent;

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

    public void AddWater(short amount)
    {
        _waterCollected = (ushort)(_waterCollected + amount);
    }

    public void CheckWaterCollection()
    {
        if (_waterCollectionStart == 0)
        {
            _waterCollectionStart = (uint)GlobalRepository.GlobalTime;
        }

        while (GlobalRepository.GlobalTime >= _waterCollectionStart + TimeToCollectWater && _waterCollected < MaxWaterAmount)
        {
            _waterCollected++;
            _waterCollectionStart += TimeToCollectWater;
        }

        if (_waterCollected >= MaxWaterAmount)
        {
            _waterCollected = MaxWaterAmount;
            _waterCollectionStart = (uint)GlobalRepository.GlobalTime;
        }
    }

    public void LoadSaveData(SaveDatas.WaterCollectorSaveData saveData)
    {
        _currLevel = saveData.CurrLevel;
        _upgradeTimeLeft = saveData.UpgradeTimeLeft;
        _isBeingUpgraded = saveData.IsBeingUpgraded;
        _waterCollected = saveData.WaterCollected;
        _waterCollectionStart = saveData.WaterCollectionStart;
    }
}
