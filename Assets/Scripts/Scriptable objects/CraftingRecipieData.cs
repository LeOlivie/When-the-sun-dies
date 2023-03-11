using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "CraftingRecipieData", menuName = "ScriptableObjects/CraftingRecipieData", order = 3)]
public class CraftingRecipieData : ScriptableObject
{
    [SerializeField] private Item[] _itemRequirements;
    [Tooltip("Required time to craft (in minutes)")]
    [SerializeField] private UInt16 _time;
    [SerializeField] private Item[] _output;

    public Item[] ItemRequirements => _itemRequirements;
    public UInt16 Time => _time;
    public Item[] Output => _output;
}
