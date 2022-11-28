using UnityEngine;
using System;

public class ItemContainer
{
    private Item[] _items;

    public delegate void ContainerUpdatedDelegate();
    public ContainerUpdatedDelegate ContainerUpdated;

    public Item[] Items => _items;

    public void SetItems(Item[] items)
    {
        _items = items;
        ContainerUpdated?.Invoke();
    }

    public bool CheckIfHas(ItemData itemData, int count)
    {
       int found = 0;
        foreach(Item item in _items)
        {
            if (item != null && item.ItemData != null && item.ItemData == itemData)
            {
                found += item.Count; 

                if (found >= count)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public bool CheckIfCanFit(Item item)
    {
        int count = item.Count;

        foreach (Item itemInCont in _items)
        {
            if (itemInCont == null || itemInCont.ItemData == null)
            {
                return true;
            }

            if (itemInCont.ItemData != item.ItemData)
            {
                continue;
            }

            if (itemInCont.Count + count <= itemInCont.MaxInStack)
            {
                return true;
            }
            else
            {
                count -= (itemInCont.MaxInStack - itemInCont.Count);
            }
        }

        return count <= 0;
    }

    public void AddItem(Item item, bool ifSubtractItemCount)
    {
        Item itemToAdd;

        if (ifSubtractItemCount)
        {
            itemToAdd = item;
        }
        else
        {
            itemToAdd = new Item(item.ItemData, item.Count);
        }

        for (int i = 0; i < _items.Length; i++)
        {
            if (_items[i] == null || _items[i].ItemData == null || itemToAdd == null || itemToAdd.ItemData == null)
            {
                continue;
            }

            if (itemToAdd.Name == _items[i].Name && _items[i].Count < _items[i].MaxInStack && itemToAdd.Count <= _items[i].MaxInStack - _items[i].Count)
            {
                _items[i].AddCount(itemToAdd.Count);
                itemToAdd.AddCount(-itemToAdd.Count);
            }
            else if (itemToAdd.Name == _items[i].Name && _items[i].Count < _items[i].MaxInStack && itemToAdd.Count > _items[i].MaxInStack - _items[i].Count)
            {
                itemToAdd.AddCount(-(_items[i].MaxInStack - _items[i].Count));
                _items[i].AddCount(_items[i].MaxInStack - _items[i].Count);
            }
        }

        if (itemToAdd.Count <= 0)
        {
            ContainerUpdated?.Invoke();
            return;
        }

        for (int i = 0; i < _items.Length; i++)
        {
            if (_items[i] == null || _items[i].ItemData == null)
            {
                _items[i] = new Item(itemToAdd.ItemData, itemToAdd.Count);
                itemToAdd.AddCount(-itemToAdd.Count);
                ContainerUpdated?.Invoke();
                return;
            }
        }

        ContainerUpdated?.Invoke();
    }

    public void RemoveItem(Item item, int amount)
    {
        /*
        int leastAmountIndex = 0;

        for (int i = 0; i < _items.Length; i++)
        {
            if (_items[i] == null || _items[i].ItemData == null || _items[i].ItemData != item.ItemData)
            {
                continue;
            }

            if (_items[leastAmountIndex] == null || _items[leastAmountIndex].ItemData == null || _items[i].Count < _items[leastAmountIndex].Count || _items[leastAmountIndex].Name != item.Name)
            {
                leastAmountIndex = i;
            }
        }

        if (_items[leastAmountIndex].Count < amount)
        {
            amount -= _items[leastAmountIndex].Count;
            _items[leastAmountIndex].AddCount(-_items[leastAmountIndex].Count);
        }
        else
        {
            _items[leastAmountIndex].AddCount(-amount);
            amount = 0;
        }

        if (_items[leastAmountIndex].Count <= 0)
        {
            _items[leastAmountIndex] = new Item();
        }*/


        for (int i = 0; i < _items.Length; i++)
        {
            if (_items[i] == null || _items[i].ItemData == null || _items[i].ItemData != item.ItemData)
            {
                continue;
            }

            if (_items[i].Count < amount)
            {
                amount -= _items[i].Count;
                _items[i].AddCount(-_items[i].Count);
            }
            else
            {
                _items[i].AddCount(-amount);
                amount = 0;
            }

            if (_items[i].Count <= 0)
            {
                _items[i] = new Item();
            }

        }

        ContainerUpdated?.Invoke();
    }

    public void RemoveItem(Item item)
    {
        for (int i = 0; i < _items.Length; i++)
        {
            if (_items[i] == item)
            {
                _items[i] = null;
                break;
            }
        }

        ContainerUpdated?.Invoke();
    }

    public void Sort()
    {
        for (int i = 0; i < _items.Length - 1; i++)
        {
            if (_items[i] == null || _items[i].ItemData == null)
            {
                continue;
            }
            else if (_items[i].Count == _items[i].MaxInStack)
            {
                continue;
            }

            for (int j = i + 1; j < _items.Length; j++)
            {

                if (_items[i] == null || _items[i].ItemData == null || _items[j] == null || _items[j].ItemData == null)
                {
                    continue;
                }
                else if (_items[i].Name != _items[j].Name || _items[j].Count == _items[j].MaxInStack)
                {
                    continue;
                }

                if (_items[i].Count + _items[j].Count < _items[j].MaxInStack)
                {
                    _items[j].AddCount(_items[i].Count);
                    _items[i] = new Item();
                }
                else
                {
                    _items[i].AddCount(- (_items[j].MaxInStack - _items[j].Count));
                    _items[j].AddCount(_items[j].MaxInStack - _items[j].Count);
                }
            }
        }

        for(int i = 0; i < _items.Length; i++)
        {
            if (_items[i] == null)
            {
                _items[i] = new Item();
            }
        }

        Array.Sort(_items);
        ContainerUpdated?.Invoke();
    }

    public ItemContainer(sbyte size)
    {
        _items = new Item[size];
    }

    public ItemContainer(sbyte size, ContainerUpdatedDelegate containerUpdatedDelegate)
    {
        _items = new Item[size];
        ContainerUpdated += containerUpdatedDelegate;
    }
}
