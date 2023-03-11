using UnityEngine;

[CreateAssetMenu(fileName = "DifficultyData", menuName = "ScriptableObjects/DifficultyData", order = 3)]
public class DifficultyData : ScriptableObject
{
    [SerializeField] private ushort _dayCycleLength;
    [SerializeField] private ushort _maxWeight;
    [SerializeField] private sbyte _foodMemoryLength;
    [SerializeField, Tooltip("Maximum duration of a single weather type in hours."), Range(2,36)] private ushort _weatherMaxDuration;
    [SerializeField, Tooltip("Hours of sleep for 1 day."), Range(0,24)] private ushort _sleepTimePerDay;

    public ushort DayCycleLength => _dayCycleLength;
    public ushort MaxWeight => _maxWeight;
    public sbyte FoodMemoryLength => _foodMemoryLength;
    public ushort WeatherMaxDuration => (ushort)(_weatherMaxDuration * 60);
    public ushort SleepTimePerDay => _sleepTimePerDay;
}
