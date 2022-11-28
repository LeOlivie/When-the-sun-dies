using UnityEngine;

[CreateAssetMenu(fileName = "DifficultyData", menuName = "ScriptableObjects/DifficultyData", order = 3)]
public class DifficultyData : ScriptableObject
{
    [SerializeField] private ushort _dayCycleLength;
    [SerializeField] private ushort _maxWeight;
    [SerializeField] private sbyte _foodMemoryLength;

    public ushort DayCycleLength => _dayCycleLength;
    public ushort MaxWeight => _maxWeight;
    public sbyte FoodMemoryLength => _foodMemoryLength;
}
