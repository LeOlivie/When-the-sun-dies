using UnityEngine;

[CreateAssetMenu(fileName = "DifficultyData", menuName = "ScriptableObjects/DifficultyData", order = 3)]
public class DifficultyData : ScriptableObject
{
    [SerializeField] private string _difficultyName;
    [SerializeField, TextArea] private string _difficultyDescription;
    [SerializeField] private Color32 _difficultyColor;

    [Space,Header("Start kit")]
    [SerializeField] private string _startKitDescription;
    [SerializeField] private Item[] _startKit;

    [Space, Header("World settings")]
    [SerializeField] private ushort _dayCycleLength;
    [SerializeField] private ushort _scavTimeStart;
    [SerializeField] private ushort _scavTimeEnd;
    [SerializeField, Tooltip("Maximum duration of a single weather type in hours."), Range(2,36)] private ushort _weatherMaxDuration;
    [SerializeField] private float _lootAmbulanceMultiplier;
    [SerializeField] private float _lootSpawnChanceMultiplier;
    [SerializeField, Tooltip("Set in days")] private int _locationResetDelay;

    [Space,Header("Player settings")]
    [SerializeField] private ushort _maxWeight;
    [SerializeField] private sbyte _foodMemoryLength;
    [SerializeField, Tooltip("Hours of sleep for 1 day."), Range(0,24)] private ushort _sleepTimePerDay;
    [SerializeField] private float _radioCooldownMultiplier;

    public string DifficultyName => _difficultyName;
    public string DifficultyDescription => _difficultyDescription;
    public Color32 DifficultyColor => _difficultyColor;
    public string StartKitDescription => _startKitDescription;
    public Item[] StartKit => _startKit;
    public ushort DayCycleLength => _dayCycleLength;
    public ushort ScavTimeStart => _scavTimeStart;
    public ushort ScavTimeEnd => _scavTimeEnd;
    public ushort MaxWeight => _maxWeight;
    public sbyte FoodMemoryLength => _foodMemoryLength;
    public ushort WeatherMaxDuration => (ushort)(_weatherMaxDuration * 60);
    public ushort SleepTimePerDay => _sleepTimePerDay;
    public float LootAmbulanceMultiplier => _lootAmbulanceMultiplier;
    public float LootSpawnChanceMultiplier => _lootSpawnChanceMultiplier;
    public float RadioCooldownMultiplier => _radioCooldownMultiplier;
    public int LocationResetDelay => _locationResetDelay;
}
