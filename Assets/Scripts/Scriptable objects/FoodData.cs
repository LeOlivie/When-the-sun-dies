using UnityEngine;
using Statuses;
using System;

[CreateAssetMenu(fileName = "FoodData", menuName = "ScriptableObjects/Items/FoodData", order = 2)]
public class FoodData : ItemData
{
    [Serializable]
    private struct StatusSpawnChance
    {
        [SerializeField] private TimedStatusData _timedStatusData;
        [SerializeField] private int _spawnChance;

        public TimedStatusData TimedStatusData => _timedStatusData;
        public int SpawnChance => _spawnChance;
    }

    [SerializeField] private int _calories;
    [SerializeField] private int _water;
    [SerializeField] private sbyte _maxHappinessAdd;
    [SerializeField] private sbyte _minHappinessAdd;
    [SerializeField] private StatusSpawnChance[] _statusSpawnChances = new StatusSpawnChance[0];
    private sbyte _happinessAdd;

    public int Calories => _calories;
    public int Water => _water;
    public sbyte MaxHappiness => _maxHappinessAdd;
    public sbyte MinHappiness => _minHappinessAdd;


    public override string GetAdditionalInfo()
    {
        string info = "";


        sbyte eatenCount = 0;

        foreach(string eatenFoodName in GlobalRepository.LastEatenFood)
        {
            if (this.Name.Equals(eatenFoodName))
            {
                eatenCount++;
            }
        }

        _happinessAdd = (sbyte)(_maxHappinessAdd - (_maxHappinessAdd - _minHappinessAdd) / (float)GlobalRepository.Difficulty.FoodMemoryLength * eatenCount);


        if (Calories != 0)
        {
            info += "<sprite name=\"EatFoodIcon\">" + Calories + "kcal";
        }

        if (Water != 0)
        {
            info += "  <sprite name=\"DrinkWaterIcon\">" + Water + "ml";
        }

        if (_happinessAdd > 0)
        {
            info += "  +" + _happinessAdd + "";
        }
        else if(_happinessAdd < 0)
        {
            info += "  " + _happinessAdd + "";
        }

        info += "  <sprite name=\"WeightIcon\">" + Weight + "kg\n";
        info += $"<sprite name=\"SellFoodIcon\">{KcalPrice}kcal  <sprite name=\"SellWaterIcon\">{MLPrice}ml";

        return info;
    }

    public override void Use()
    {
        if (ItemsToAddAfterUse != null && ItemsToAddAfterUse.Length > 0) 
        {
            foreach (Item item in this.ItemsToAddAfterUse)
            {
                GlobalRepository.Inventory.AddItem(item, false);
            }
        }

        foreach (StatusSpawnChance status in _statusSpawnChances)
        {
            if (UnityEngine.Random.Range(0, 101) < status.SpawnChance)
            {
                GlobalRepository.AddStatus(new TimedStatus(status.TimedStatusData));
            }
        }
        
        GlobalRepository.AddLastEatenFood(this.Name);
        GlobalRepository.AddKcal(_calories);
        GlobalRepository.AddWater(_water);
        GlobalRepository.AddHappiness(_happinessAdd);
    }
}
