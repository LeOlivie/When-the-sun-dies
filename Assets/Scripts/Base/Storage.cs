using System;
using UnityEngine;

[Serializable]
public class Storage : IUpgradable
{
    [Serializable]
    private struct Upgrades
    {
        [SerializeField] private Item[] _upgradeRequierements;
        [SerializeField] private IUpgradable.SkillRequirement[] _skillRequirements;
        [SerializeField] private uint _upgradeTime;
        [SerializeField] private Sprite _upgradedSprite;
        [SerializeField] private LootSpawnerData.ContainerSizeEnum _storageSize;

        public Item[] UpgradeRequierements => _upgradeRequierements;
        public IUpgradable.SkillRequirement[] SkillRequirements => _skillRequirements;
        public uint UpgradeTime => _upgradeTime;
        public Sprite UpgradedSprite => _upgradedSprite;
        public LootSpawnerData.ContainerSizeEnum StorageSize => _storageSize;
    }

    [SerializeField] private string _name;
    [SerializeField] private Upgrades[] _upgrades;
    private int _upgradeTimeLeft;
    private uint _currLevel;
    private bool _isBeingUpgraded;
    private ItemContainer _itemContainer = new ItemContainer(4);

    public delegate void OnUpgradedDelegate();
    public event OnUpgradedDelegate OnUpgradedEvent;

    public string ObjectName => _name;
    public Item[] UpgradeRequirements => _upgrades[_currLevel].UpgradeRequierements;
    public IUpgradable.SkillRequirement[] SkillRequirements => _upgrades[_currLevel].SkillRequirements;
    public int UpgradeTimeLeft => _upgradeTimeLeft;
    public int UpgradeTimeRequired => (int)_upgrades[_currLevel].UpgradeTime;
    public LootSpawnerData.ContainerSizeEnum StorageSize => _upgrades[_currLevel].StorageSize;
    public uint CurrLevel => _currLevel;
    public uint MaxLevel => (uint)(_upgrades.Length - 1);
    public bool IsBeingUpgraded => _isBeingUpgraded;
    public Sprite UpgradedSprite => _upgrades[_currLevel].UpgradedSprite;
    public ItemContainer ItemContainer => _itemContainer;

    public void DecreaseUpgradeTimeLeft()
    {
        _upgradeTimeLeft--;
    }

    public void OnUpgradeEnded()
    {
        _currLevel += 1;
        OnUpgradedEvent?.Invoke();
        _isBeingUpgraded = false;
        Item[] itemsTemp = new Item[_itemContainer.Items.Length];

        for (int i = 0; i < _itemContainer.Items.Length; i++)
        {
            if (_itemContainer.Items[i] == null || _itemContainer.Items[i].ItemData == null)
            {
                continue;
            }

            itemsTemp[i] = new Item(_itemContainer.Items[i].ItemData, _itemContainer.Items[i].Count);
        }

        Array.Resize<Item>(ref itemsTemp, (int)_upgrades[_currLevel].StorageSize);
        _itemContainer.SetItems(itemsTemp);
    }

    public void OnUpgradeStarted()
    {
        _isBeingUpgraded = true;
        _upgradeTimeLeft = (int)_upgrades[_currLevel].UpgradeTime;
    }

    public void LoadStorage(SaveDatas.StorageSaveData storageSaveData)
    {
        _currLevel = storageSaveData.UpgradableSave.CurrLevel;
        _upgradeTimeLeft = storageSaveData.UpgradableSave.UpgradeTimeLeft;
        _isBeingUpgraded = storageSaveData.UpgradableSave.IsBeingUpgraded;
        _itemContainer = new ItemContainer(storageSaveData.ContainerSave);
    }
}
