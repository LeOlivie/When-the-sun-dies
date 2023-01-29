using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GlobalRepository
{
    public enum SkillType { DIY, Electronics, Bakery, Building };
    private static int _globalTime = 2160;
    private static float _kcal = 2000;
    private static float _water = 2000;
    private static float _weight;
    private static float _maxWeight = 15;
    private static sbyte _happiness = 50;
    private static int _raidTimeLeft = 90;
    private static int _timeBeforeArrival = 0;
    private static LocationData _currentLocationData;
    private static List<string> _lastEatenFood = new List<string>();
    private static Dictionary<SkillType, int> skills = new Dictionary<SkillType, int>();

    private static DifficultyData _difficulty;
    private static ItemContainer _inventory = new ItemContainer(16, CountWeight);

    public delegate void TimeUpdatedDelegate();
    public static event TimeUpdatedDelegate OnTimeUpdated;
    
    public static int GlobalTime => _globalTime;
    public static float Kcal => _kcal;
    public static float Water => _water;
    public static float Weight => _weight;
    public static float MaxWeight => _maxWeight;
    public static sbyte Happiness => _happiness;
    public static int RaidTimeLeft => _raidTimeLeft;
    public static int TimeBeforeArrival => _timeBeforeArrival;
    public static LocationData CurrentLocationData => _currentLocationData;
    public static List<string> LastEatenFood => _lastEatenFood;
    public static DifficultyData Difficulty => _difficulty;
    public static ItemContainer Inventory => _inventory;
    public static Dictionary<SkillType, int> Skills
    {
        get
        {
            if (skills.Count <= 0) CreateSkillDict();
            return skills;
        }
    }

    public static void CreateSkillDict()
    {
        foreach(string str in Enum.GetNames(typeof(SkillType)))
        {
            SkillType skillType = (SkillType)Enum.Parse(typeof(SkillType), str);
            skills.Add(skillType, 0);
        }
    }

    public static void SetDifficulty(DifficultyData difficultyData)
    {
        if (_difficulty == null)
        {
            _difficulty = difficultyData;
            _maxWeight = _difficulty.MaxWeight;
        }
        /*else
        {
            throw new System.Exception("Trying to change difficulty.");
        }*/
    }

    public static void CountWeight()
    {
        _weight = 0;

        foreach (Item item in _inventory.Items)
        {
            if (item == null || item.ItemData == null)
            {
                continue;
            }
            _weight += item.Count * item.Weight;
        }

        _weight = float.Parse(Math.Round(_weight, 2).ToString());
    }

    public static void AddTime(uint minutesCount)
    {
        if (_raidTimeLeft > 0)
        {
            _raidTimeLeft -= (int)minutesCount;
        }

        if (_timeBeforeArrival > 0)
        {
            _timeBeforeArrival -= (int)minutesCount;
        }

        _globalTime += (int)minutesCount;

        OnTimeUpdated?.Invoke();
    }

    public static void AddKcal(float amount)
    {
        if (_kcal + amount <= 0)
        {
            _kcal = 0;
        }
        else if (_kcal + amount > 2000)
        {
            _kcal = 2000;
        }
        else
        {
            _kcal += amount;
        }
    }

    public static void AddWater(float amount)
    {
        if (_water + amount <= 0)
        {
            _water = 0;
        }
        else if (_water + amount > 2000)
        {
            _water = 2000;
        }
        else
        {
            _water += amount;
        }
    }

    public static void AddHappiness(sbyte value)
    {
        _happiness += value;

        if (_happiness > 50)
        {
            _happiness = 50;
        }
        else if (_happiness < -50)
        {
            _happiness -= 50;
        }
    }

    public static void AddLastEatenFood(string foodName)
    {
        _lastEatenFood.Add(foodName);

        while (_lastEatenFood.Count > _difficulty.FoodMemoryLength)
        {
            _lastEatenFood.RemoveAt(0);
        }
    }

    public static void ChangeLocation(LocationData locationData)
    {
        OnTimeUpdated = null;
        _currentLocationData = locationData;
        _timeBeforeArrival = Mathf.CeilToInt(locationData.Distance / 6f * 60f);
        _raidTimeLeft = 0;
        Time.timeScale = 1;
        SceneManager.LoadScene("GoingScene");
    }

    public static void LoadSaveData(SaveDatas.PlayerSaveData playerSaveData)
    {
        _inventory = new ItemContainer(playerSaveData.InventorySaveData);
        _water = playerSaveData.Water;
        _kcal = playerSaveData.Calories;

        Vector3 playerPos = new Vector3(playerSaveData.PlayerPos.xPos, playerSaveData.PlayerPos.yPos, playerSaveData.PlayerPos.zPos);
        GameObject.FindGameObjectWithTag("Player").transform.localPosition = playerPos;

        _globalTime = playerSaveData.GlobalTime;
    }
}