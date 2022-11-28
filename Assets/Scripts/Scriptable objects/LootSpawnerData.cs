using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "LootSpawnerData", menuName = "ScriptableObjects/LootSpawnerData", order = 2)]
public class LootSpawnerData : ScriptableObject
{

    [Serializable]
    private struct ItemSpawnData
    {
        [SerializeField] private ItemData _itemData;
        [SerializeField, Range(0,100)] private sbyte _spawnChance;
        [SerializeField] private sbyte _count;

        public ItemData ItemData => _itemData;
        public sbyte SpawnChance => _spawnChance;
        public sbyte Count => _count;
    }

    [SerializeField] private ItemSpawnData[] _itemSpawnDatas;
    [SerializeField] private sbyte _containerSize;
    [SerializeField] private string _containerName;
    [SerializeField] private float _lootTime;

    public sbyte ContainerSize => _containerSize;
    public string ContainerName => _containerName;
    public float LootTime => _lootTime;

    public Item[] GetSpawnedItems()
    {
        List<Item> items = new List<Item>();

        foreach (ItemSpawnData spawnData in _itemSpawnDatas)
        {
            if (UnityEngine.Random.Range(0, 100) <= spawnData.SpawnChance)
            {
                items.Add(new Item(spawnData.ItemData, spawnData.Count));
            }
        }

        return items.ToArray();
    }
}
