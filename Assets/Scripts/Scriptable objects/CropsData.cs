using UnityEngine;


[CreateAssetMenu(fileName = "CropData", menuName = "ScriptableObjects/CropData", order = 5)]
public class CropsData : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private Item[] _requirements;
    [SerializeField] private uint _growTime;
    [SerializeField] private uint _wateringTime;
    [SerializeField] private Item _output;

    public string Name => _name;
    public Item[] Requirements => _requirements;
    public uint GrowTime => _growTime;
    public uint WateringTime => _wateringTime;
    public Item Output => _output;

}
