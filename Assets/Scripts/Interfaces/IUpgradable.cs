using System;
using UnityEngine;

public interface IUpgradable
{
    [Serializable]
    public struct SkillRequirement
    {
        [SerializeField] private GlobalRepository.SkillType _skillType;
        [SerializeField] private int _skillLevel;

        public GlobalRepository.SkillType SkillType => _skillType;
        public int SkillLevel => _skillLevel;
    }

    public string ObjectName { get; }
    public Item[] UpgradeRequirements { get; }
    public SkillRequirement[] SkillRequirements { get; }
    public int UpgradeTimeLeft { get; }
    public int UpgradeTimeRequired { get; }
    public uint CurrLevel { get; }
    public uint MaxLevel { get; }
    public bool IsBeingUpgraded { get; }
    public void OnUpgradeEnded();
    public void OnUpgradeStarted();
    public void DecreaseUpgradeTimeLeft();
}
