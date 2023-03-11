using UnityEngine;
using System;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "CraftingStaionData", menuName = "ScriptableObjects/CraftingStaionData", order = 3)]
public class CraftingStaionData : ScriptableObject
{
    [Serializable]
    private struct StationLevelData
    {
        [SerializeField] private CraftingRecipieData[] _recipies;
        [SerializeField] private IUpgradable.SkillRequirement[] _skillRequirements;
        [SerializeField] private Item[] _upgradeRequierements;
        [SerializeField] private uint _upgradeTime;
        [SerializeField] private Sprite _upgradedSprite;

        public CraftingRecipieData[] Recipies => _recipies;
        public Item[] UpgradeRequierements => _upgradeRequierements;
        public IUpgradable.SkillRequirement[] SkillRequirements => _skillRequirements;
        public uint UpgradeTime => _upgradeTime;
        public Sprite UpgradedSprite => _upgradedSprite;
    }

    [SerializeField] private StationLevelData[] _stationLevels;
    [SerializeField] private string _name;

    public string Name => _name;
    public int MaxLevel => _stationLevels.Length - 1;

    public CraftingRecipieData[] GetRecipies(uint level)
    {
        List<CraftingRecipieData> _recepies = new List<CraftingRecipieData>();

        for (int i = 0; i <= level; i++)
        {
            _recepies.AddRange(_stationLevels[i].Recipies);
        }

        return _recepies.ToArray();
    }

    public Item[] GetUpgradeRequierements(uint level)
    {
        return _stationLevels[level].UpgradeRequierements;
    }

    public uint GetUpgradeTime(uint level)
    {
        return _stationLevels[level].UpgradeTime;
    }

    public Sprite GetSprite(uint level)
    {
        return _stationLevels[level].UpgradedSprite;
    }

    public IUpgradable.SkillRequirement[] GetSkillRequirements(uint level)
    {
        return _stationLevels[level].SkillRequirements;
    }
}
