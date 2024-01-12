using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Radio : IUpgradable
{
    [Serializable]
    private struct Upgrades
    {
        [SerializeField] private Item[] _upgradeRequierements;
        [SerializeField] private IUpgradable.SkillRequirement[] _skillRequirements;
        [SerializeField] private uint _upgradeTime;
        [SerializeField] private Sprite _upgradedSprite;
        [SerializeField] private EventLocClusterData[] _eventLocClusters;
        [SerializeField] private List<String> _infoTransmissions;

        public Item[] UpgradeRequierements => _upgradeRequierements;
        public IUpgradable.SkillRequirement[] SkillRequirements => _skillRequirements;
        public uint UpgradeTime => _upgradeTime;
        public Sprite UpgradedSprite => _upgradedSprite;
        public EventLocClusterData[] EventLocClusters => _eventLocClusters;
        public List<String> InfoTransmissions => _infoTransmissions;
    }

    [SerializeField] private string _name;
    [SerializeField] private Upgrades[] _upgrades;
    [SerializeField] private int _minTimeBeforeEvent;
    [SerializeField] private int _maxTimeBeforeEvent;
    private EventLocClusterData _activeEventLocCluster;
    private int _eventSwitchOffTime;
    private int _nextEventTime;
    private int _upgradeTimeLeft;
    private uint _currLevel;
    private bool _isBeingUpgraded;

    public delegate void OnUpgradedDelegate();
    public event OnUpgradedDelegate OnUpgradedEvent;

    public string ObjectName => _name;
    public Item[] UpgradeRequirements => _upgrades[_currLevel].UpgradeRequierements;
    public IUpgradable.SkillRequirement[] SkillRequirements => _upgrades[_currLevel].SkillRequirements;
    public EventLocClusterData ActiveEventLocCluster => _activeEventLocCluster;
    public int UpgradeTimeLeft => _upgradeTimeLeft;
    public int UpgradeTimeRequired => (int)_upgrades[_currLevel].UpgradeTime;
    public int EventSwitchOffTime => _eventSwitchOffTime;
    public int NextEventTime => _nextEventTime;
    public uint CurrLevel => _currLevel;
    public uint MaxLevel => (uint)(_upgrades.Length - 1);
    public bool IsBeingUpgraded => _isBeingUpgraded;
    public Sprite UpgradedSprite => _upgrades[_currLevel].UpgradedSprite;
    public List<String> InfoTransmissions => _upgrades[_currLevel].InfoTransmissions;

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

    public void CheckEvent()
    {
        if (_nextEventTime <= GlobalRepository.SystemVars.GlobalTime)
        {
            int eventIndex = UnityEngine.Random.Range(0, _upgrades[_currLevel].EventLocClusters.Length);
            _activeEventLocCluster = _upgrades[_currLevel].EventLocClusters[eventIndex];
            _eventSwitchOffTime = GlobalRepository.SystemVars.GlobalTime + _activeEventLocCluster.AvailableTime;
            _nextEventTime = _eventSwitchOffTime + UnityEngine.Random.Range(_minTimeBeforeEvent, _maxTimeBeforeEvent);
        }

        if (_eventSwitchOffTime <= GlobalRepository.SystemVars.GlobalTime)
        {
            _activeEventLocCluster = null;
        }
    }

    public void LoadSaveData(SaveDatas.RadioSaveData saveData)
    {
        _currLevel = saveData.CurrLevel;
        _upgradeTimeLeft = saveData.UpgradeTimeLeft;
        _isBeingUpgraded = saveData.IsBeingUpgraded;
        _activeEventLocCluster = saveData.ActiveEventLocCluster;
        _eventSwitchOffTime = saveData.EventSwitchOffTime;
        _nextEventTime = saveData.NextEventTime;
    }
}
