using System;
using UnityEngine;
using TMPro;

[Serializable]
public class CraftingStation : IUpgradable, ICrafter
{
    [SerializeField] private CraftingStaionData _staionData;
    private uint _level;
    private int _upgradeTimeLeft;
    private bool _isBeingUpgraded = false;
    private CraftingRecipieData _activeCraftData;
    private int _craftTimeLeft;

    public delegate void OnUpgradedDelegate();
    public event OnUpgradedDelegate OnUpgradedEvent;

    public CraftingStaionData StationData => _staionData;
    public uint CurrLevel => _level;
    public uint MaxLevel => (uint)_staionData.MaxLevel;
    public int UpgradeTimeLeft => _upgradeTimeLeft;
    public int CraftTimeLeft => _craftTimeLeft;
    public int UpgradeTimeRequired => (int)_staionData.GetUpgradeTime(_level);
    public string ObjectName => _staionData.Name;
    public bool IsBeingUpgraded => _isBeingUpgraded;
    public CraftingRecipieData ActiveCraftData => _activeCraftData;
    public CraftingRecipieData[] Recipies => _staionData.GetRecipies(_level);
    public Item[] UpgradeRequirements => _staionData.GetUpgradeRequierements(_level);
    public IUpgradable.SkillRequirement[] SkillRequirements => _staionData.GetSkillRequirements(_level);

    public void OnUpgradeEnded()
    {
        _isBeingUpgraded = false;
        _level += 1;
        OnUpgradedEvent?.Invoke();
    }

    public void OnUpgradeStarted()
    {
        _isBeingUpgraded = true;
        _upgradeTimeLeft = (int)_staionData.GetUpgradeTime(_level);
    }

    public void DecreaseUpgradeTimeLeft()
    {
        _upgradeTimeLeft--;
    }

    public void OnCraftStarted(CraftingRecipieData craftingRecipieData)
    {
        _activeCraftData = craftingRecipieData;
        _craftTimeLeft = craftingRecipieData.Time;
    }

    public void OnCraftEnded()
    {
        _activeCraftData = null;
        _craftTimeLeft = 0;
    }

    public void DecreaseCraftTimeLeft()
    {
        _craftTimeLeft--;
    }

    public void LoadSaveData(SaveDatas.CraftingStationSaveData saveData)
    {
        _level = saveData.CurrLevel;
        _upgradeTimeLeft = saveData.UpgradeTimeLeft;
        _isBeingUpgraded = saveData.IsBeingUpgraded;
        _activeCraftData = saveData.ActiveCraft;
        _craftTimeLeft = saveData.CraftingTimeLeft;
    }
}
