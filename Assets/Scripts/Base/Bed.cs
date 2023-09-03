using UnityEngine;
using System;

[Serializable]
public class Bed : IUpgradable
{
    [Serializable]
    private struct Upgrades
    {
        [SerializeField] private Item[] _upgradeRequierements;
        [SerializeField] private IUpgradable.SkillRequirement[] _skillRequirements;
        [SerializeField] private uint _upgradeTime;
        [SerializeField] private Sprite _upgradedSprite;
        [SerializeField] private float _fatigueMultiplier;
        [SerializeField] private float _kcalDeacreaseBuff;
        [SerializeField] private float _waterDecreaseBuff;

        public Item[] UpgradeRequierements => _upgradeRequierements;
        public IUpgradable.SkillRequirement[] SkillRequirements => _skillRequirements;
        public uint UpgradeTime => _upgradeTime;
        public Sprite UpgradedSprite => _upgradedSprite;
        public float FatigueMultiplier => _fatigueMultiplier;
        public float KcalDeacreaseBuff => _kcalDeacreaseBuff;
        public float WaterDecreaseBuff => _waterDecreaseBuff;
    }

    [SerializeField] private string _name;
    [SerializeField] private Upgrades[] _upgrades;
    private int _upgradeTimeLeft;
    private uint _currLevel;
    private bool _isBeingUpgraded;

    public delegate void OnUpgradedDelegate();
    public event OnUpgradedDelegate OnUpgradedEvent;

    public string ObjectName => _name;
    public Item[] UpgradeRequirements => _upgrades[_currLevel].UpgradeRequierements;
    public IUpgradable.SkillRequirement[] SkillRequirements => _upgrades[_currLevel].SkillRequirements;
    public int UpgradeTimeLeft => _upgradeTimeLeft;
    public int UpgradeTimeRequired => (int)_upgrades[_currLevel].UpgradeTime;
    public float FatigueMultiplier => _upgrades[_currLevel].FatigueMultiplier;
    public uint CurrLevel => _currLevel;
    public uint MaxLevel => (uint)(_upgrades.Length - 1);
    public bool IsBeingUpgraded => _isBeingUpgraded;
    public Sprite UpgradedSprite => _upgrades[_currLevel].UpgradedSprite;
    public float KcalDecreaseBuff => _upgrades[_currLevel].KcalDeacreaseBuff;
    public float WaterDecreaseBuff => _upgrades[_currLevel].WaterDecreaseBuff;

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

    public void LoadSaveData(SaveDatas.UpgradableSaveData saveData)
    {
        _currLevel = saveData.CurrLevel;
        _upgradeTimeLeft = saveData.UpgradeTimeLeft;
        _isBeingUpgraded = saveData.IsBeingUpgraded;
    }
}
