using UnityEngine;
using System;

[CreateAssetMenu(fileName = "ItemData", menuName = "ScriptableObjects/ItemData", order = 2)]
public class ItemData : ScriptableObject
{
    [SerializeField] private Sprite _icon;
    [SerializeField] private string _name;
    [SerializeField, TextArea] private string _description;
    [SerializeField] private float _weight;
    [SerializeField] private int _maxInStack;
    [SerializeField] private bool _isUsable;
    [SerializeField] private Item[] _itemsToAddAfterUse;
    [SerializeField] private int _lootPointsToSpawn; 

    public Sprite Icon => _icon;
    public string Name => _name;
    public string Description => _description;
    public float Weight => _weight;
    public int MaxInStack => _maxInStack;
    public bool IsUsable => _isUsable;
    public int LootPointsToSpawn => _lootPointsToSpawn;
    public Item[] ItemsToAddAfterUse => _itemsToAddAfterUse;

    public virtual string GetAdditionalInfo()
    {
        return "Weight: " + Weight + "kg";
    }

    public virtual void Use()
    {
    }
}

