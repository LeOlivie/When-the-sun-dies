using UnityEngine;
using System;

[Serializable]
public class Item : IComparable
{

    [SerializeField] private ItemData _itemData;
    [SerializeField] private int _count;

    public ItemData ItemData => _itemData;
    public Sprite Icon => _itemData.Icon;
    public string Name => _itemData.Name;
    public string Description => _itemData.Description;
    public float Weight => _itemData.Weight;
    public int Count => _count;
    public int MaxInStack => _itemData.MaxInStack;
    public string AdditionalInfo => _itemData.GetAdditionalInfo();

    public void AddCount(int count)
    {
        if (_count + count >= 0 && _count + count <= MaxInStack)
        {
            _count += count;
        }
        else if (_count + count < 0)
        {
            _count = 0;
        }
        else
        {
            _count = MaxInStack;
        }

        if (_count <= 0)
        {
            _itemData = null;
        }
    }

    public void Use()
    {
        if (!_itemData.IsUsable)
        {
            return;
        }

        _itemData.Use();
        AddCount(-1);
    }

    public int CompareTo(object obj)
    {
        Item other = obj as Item;

        if (ItemData == null)
        {
            return 1;
        }
        else if (other.ItemData == null || obj == null)
        {
            return -1;
        }

        int order = other.Name.CompareTo(Name);

        if (order == 0)
        {
            return other.Count.CompareTo(Count);
        }

        return order;
    }

    public Item()
    {
    }

    public Item(ItemData itemData, int count)
    {
        _itemData = itemData;
        _count = count;
    }

    public Item(SaveDatas.ItemSaveData itemSaveData)
    {
        if (itemSaveData != null) 
        {
            _itemData = itemSaveData.Data;
            _count = itemSaveData.Count;
        }
    }
}
