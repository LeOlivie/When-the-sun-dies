using UnityEngine;
using System;
using System.Collections.Generic;

public class LootSpawner : MonoBehaviour
{
    [SerializeField] private LootSpawnerData _lootSpawnerData;
    [SerializeField] private GameObject _lootScreen;
    [SerializeField] private ButtonHandler _interractBtn;
    [SerializeField] private LootScreenShower _lootScreenShower;
    [SerializeField] private bool _isLooted;
    private ItemContainer _itemContainer;

    public ItemContainer ItemContainer => _itemContainer;
    public bool IsLooted => _isLooted;

    private void Awake()
    {
        if(_lootSpawnerData != null && !IsLooted)
        {
            _itemContainer = new ItemContainer((sbyte)_lootSpawnerData.ContainerSize);
            Item[] items = _lootSpawnerData.GetSpawnedItems();

            foreach (Item item in items)
            {
                _itemContainer.AddItem(item, false);
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        _interractBtn.AddListener(Interract);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _interractBtn.RemoveListener(Interract);
    }

    private void Interract()
    {
        if (!_lootScreen.activeInHierarchy)
        {
            _lootScreenShower.OpenLootScreen(_itemContainer, _lootSpawnerData.ContainerName, _isLooted, OnLooted, _lootSpawnerData.LootTime);
        }
        else
        {
            _lootScreenShower.CloseScreen();
        }
    }

    private void OnLooted()
    {
        _isLooted = true;
    }

    public void LoadSaveData(SaveDatas.LootSpawnerSaveData saveData)
    {
        _itemContainer = new ItemContainer(saveData.ContainerSave);
        _isLooted = saveData.IsLooted;
    }
}
