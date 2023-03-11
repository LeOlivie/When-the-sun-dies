using System;
using UnityEngine;

[Serializable]
public class Hydroponics : IUpgradable
{
    [Serializable]
    private struct Upgrades
    {
        [SerializeField] private Item[] _upgradeRequierements;
        [SerializeField] private IUpgradable.SkillRequirement[] _skillRequirements;
        [SerializeField] private uint _upgradeTime;
        [SerializeField] private Sprite _upgradedSprite;
        [SerializeField] private CropsData[] _cropDatas;

        public Item[] UpgradeRequierements => _upgradeRequierements;
        public IUpgradable.SkillRequirement[] SkillRequirements => _skillRequirements;
        public uint UpgradeTime => _upgradeTime;
        public Sprite UpgradedSprite => _upgradedSprite;
        public CropsData[] CropDatas => _cropDatas;
    }

    [SerializeField] private string _name;
    [SerializeField] private Upgrades[] _upgrades;
    private int _upgradeTimeLeft;
    private uint _currLevel;
    private bool _isBeingUpgraded;
    private CropsData _growingCropData;
    private int _growStartTime;
    private int _lastWateringTime;



    public delegate void OnUpgradedDelegate();
    public event OnUpgradedDelegate OnUpgradedEvent;

    public string ObjectName => _name;
    public Item[] UpgradeRequirements => _upgrades[_currLevel].UpgradeRequierements;
    public IUpgradable.SkillRequirement[] SkillRequirements => _upgrades[_currLevel].SkillRequirements;
    public int UpgradeTimeLeft => _upgradeTimeLeft;
    public int UpgradeTimeRequired => (int)_upgrades[_currLevel].UpgradeTime;
    public uint CurrLevel => _currLevel;
    public uint MaxLevel => (uint)(_upgrades.Length - 1);
    public bool IsBeingUpgraded => _isBeingUpgraded;
    public Sprite UpgradedSprite => _upgrades[_currLevel].UpgradedSprite;
    public CropsData[] CropsDatas => _upgrades[_currLevel].CropDatas;
    public CropsData GrowingCropData => _growingCropData;
    public int GrowStartTime => _growStartTime;
    public int LastWateringTime => _lastWateringTime;

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

    public void StartGrowingCrop(CropsData cropData)
    {
        _growingCropData = cropData;
        _growStartTime = GlobalRepository.GlobalTime;
        _lastWateringTime = GlobalRepository.GlobalTime;
    }

    public void Reset()
    {
        Debug.Log("Resetted");
        _growingCropData = null;
    }

    public void WaterCrop()
    {
        Debug.Log("Watered");
        _lastWateringTime = GlobalRepository.GlobalTime;
    }

    public void LoadSaveData(SaveDatas.UpgradableSaveData saveData)
    {
        _currLevel = saveData.CurrLevel;
        _upgradeTimeLeft = saveData.UpgradeTimeLeft;
        _isBeingUpgraded = saveData.IsBeingUpgraded;
    }
}
