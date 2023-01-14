using UnityEngine;
using System;

[CreateAssetMenu(fileName = "HarvesterData", menuName = "ScriptableObjects/HarvesterData", order = 3)]
public class HarvesterData : ScriptableObject
{
    [Serializable]
    public struct HarvestOptionData
    {
        [Serializable]
        public struct Requirement
        {
            [SerializeField] private Item _itemRequirement;
            [SerializeField] private bool _isDisposable;

            public Item ItemRequirement => _itemRequirement;
            public bool IsDisposable => _isDisposable;
        }

        [SerializeField] private Item _itemToHarvest;
        [SerializeField] private Requirement[] _requirements;
        [SerializeField] private int _timeToHarvest;
        [SerializeField, Tooltip("In kcal/minute")] private float _kcalDebuff;
        [SerializeField, Tooltip("In ml/minute")] private float _waterDebuff;
        [SerializeField] private int _maxHarvests;
        [SerializeField, Range(0, 100)] private int _availabilityChance;

        public Item ItemToHarvest => _itemToHarvest;
        public Requirement[] Requirements => _requirements;
        public int TimeToHarvest => _timeToHarvest;
        public float KcalDebuff => _kcalDebuff;
        public float WaterDebuff => _waterDebuff;
        public int MaxHarvests => _maxHarvests;
        public int AvailabilityChance => _availabilityChance;
    }

    [SerializeField] private HarvestOptionData[] _harvestOptionDatas;
    public HarvestOptionData[] HarvestOptionDatas => _harvestOptionDatas;
}
