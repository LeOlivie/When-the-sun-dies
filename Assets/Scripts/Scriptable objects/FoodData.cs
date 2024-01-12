using UnityEngine;
using Statuses;
using System;

[CreateAssetMenu(fileName = "FoodData", menuName = "ScriptableObjects/Items/FoodData", order = 2)]
public class FoodData : ItemData
{
    [Serializable]
    private struct StatusSpawnChance
    {
        [SerializeField] private StatusData _statusData;
        [SerializeField] private int _spawnChance;

        public StatusData StatusData => _statusData;
        public int SpawnChance => _spawnChance;
    }

    [Space, Header("Food info")]
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

        foreach(string eatenFoodName in GlobalRepository.PlayerVars.LastEatenFood)
        {
            if (Name.Equals(eatenFoodName))
            {
                eatenCount++;
            }
        }

        _happinessAdd = (sbyte)(_maxHappinessAdd - (_maxHappinessAdd - _minHappinessAdd) / (float)GlobalRepository.SystemVars.Difficulty.FoodMemoryLength * eatenCount);


        if (Calories != 0)
        {
            info += "<sprite name=\"EatFoodIcon\">" + Calories + "kcal  ";
        }

        if (Water != 0)
        {
            info += "<sprite name=\"DrinkWaterIcon\">" + Water + "ml  ";
        }

        if (_happinessAdd > 0)
        {
            info += "<sprite name=\"HappinessIcon\">+" + _happinessAdd + "  ";
        }
        else if(_happinessAdd < 0)
        {
            info += "<sprite name=\"HappinessIcon\">" + _happinessAdd + "  ";
        }

        info += "<sprite name=\"WeightIcon\">" + Weight + "kg\n";
        info += $"<sprite name=\"SellFoodIcon\">{KcalPrice}kcal  <sprite name=\"SellWaterIcon\">{MLPrice}ml";

        return info;
    }

    public override void Use()
    {
        if (ItemsToAddAfterUse != null && ItemsToAddAfterUse.Length > 0)
        {
            foreach (Item item in this.ItemsToAddAfterUse)
            {
                GlobalRepository.PlayerVars.Inventory.AddItem(item, false);
            }
        }

        foreach (StatusSpawnChance status in _statusSpawnChances)
        {
            if (UnityEngine.Random.Range(0, 101) < status.SpawnChance)
            {
               GlobalRepository.AddStatus(new Status(status.StatusData));
            }
        }

        GlobalRepository.PlayerVars.LastEatenFood.Add(this.Name);
        GlobalRepository.PlayerVars.KCal += _calories;
        GlobalRepository.PlayerVars.Water += _water;
        GlobalRepository.PlayerVars.Happiness += _happinessAdd;

        int eatenFoodCount = GlobalRepository.PlayerVars.LastEatenFood.Count;
        int lengthByDifficulty = GlobalRepository.SystemVars.Difficulty.FoodMemoryLength;
        if(eatenFoodCount > lengthByDifficulty)
        {
            GlobalRepository.PlayerVars.LastEatenFood.RemoveRange(lengthByDifficulty-1, eatenFoodCount - lengthByDifficulty);
        }
    }
}
