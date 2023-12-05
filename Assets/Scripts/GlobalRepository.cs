using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using Statuses;
using UnityEngine.PlayerLoop;

public static class GlobalRepository
{
    public enum SkillType { DIY, Electronics, Bakery, Building };
    public enum WeatherTypeEnum { ClearWeather, Foggy, Rainy, Windy };
    private static int _globalTime =2710; //2160 = 1 day 12:00
    private static float _kcal = 2000;
    private static float _water = 2000;
    private static float _weight;
    private static float _maxWeight = 15;
    private static float _happiness = 50;
    private static int _raidTimeLeft = 90;
    private static int _timeBeforeArrival = 0;
    private static int _weatherChangeTime = 1;
    private static int _questsProgress;
    private static float _fatigue = 100;
    private static WeatherTypeEnum _weatherType;
    private static LocationData _currentLocationData;
    private static List<string> _lastEatenFood = new List<string>();
    private static Dictionary<SkillType, int> _skills = new Dictionary<SkillType, int>();
    private static LightSourceData _lightSourceData;
    private static Quest _activeQuest = null;
    private static List<Status> _activeStatuses = new List<Status>();
    public static bool IsStartKitReceived;
    public static bool ShowDayChange = true;

    private static DifficultyData _difficulty;
    private static ItemContainer _inventory = new ItemContainer(16, CountWeight);

    public delegate void TimeUpdatedDelegate();
    public static event TimeUpdatedDelegate OnTimeUpdated;
    
    public static int GlobalTime => _globalTime;
    public static float Kcal => _kcal;
    public static float Water => _water;
    public static float Weight => _weight;
    public static float MaxWeight => _maxWeight;
    public static float Happiness => Mathf.RoundToInt(_happiness);
    public static int RaidTimeLeft => _raidTimeLeft;
    public static int TimeBeforeArrival => _timeBeforeArrival;
    public static int Fatigue => Mathf.FloorToInt(_fatigue);
    public static int QuestsProgress => _questsProgress;
    public static WeatherTypeEnum WeatherType => _weatherType;
    public static LocationData CurrentLocationData => _currentLocationData;
    public static List<string> LastEatenFood => _lastEatenFood;
    public static DifficultyData Difficulty => _difficulty;
    public static ItemContainer Inventory => _inventory;
    public static LightSourceData LightSourceData => _lightSourceData;
    public static Quest ActiveQuest => _activeQuest;
    public static List<Status> ActiveStatuses => _activeStatuses;
    public static Dictionary<SkillType, int> Skills
    {
        get
        {
            if (_skills.Count <= 0) CreateSkillDict();
            return _skills;
        }
    }

    public static void CreateSkillDict()
    {
        foreach(string str in Enum.GetNames(typeof(SkillType)))
        {
            SkillType skillType = (SkillType)Enum.Parse(typeof(SkillType), str);
            _skills.Add(skillType, 0);
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

        _weatherChangeTime -= (int)minutesCount;

        if(_weatherChangeTime <= 0)
        {
            int newWeatherIndex = UnityEngine.Random.Range(0, Enum.GetValues(typeof(WeatherTypeEnum)).Length);
            _weatherType = (WeatherTypeEnum)(Enum.GetValues(_weatherType.GetType())).GetValue(newWeatherIndex);
            _weatherChangeTime = UnityEngine.Random.Range(120, _difficulty.WeatherMaxDuration);
            Debug.Log(_weatherType.ToString());
        }

        _globalTime += (int)minutesCount;

        if (_globalTime%1440 == 0)
        {
            ShowDayChange = true;
        }

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

    public static void AddHappiness(float value)
    {
        _happiness += value;

        if (_happiness > 50)
        {
            _happiness = 50;
        }
        else if (_happiness < 0)
        {
            _happiness = 0;
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

    public static void AddFatigue(float change)
    {
        _fatigue += change;
        
        if (_fatigue > 100)
        {
            _fatigue = 100;
        }
        else if (_fatigue < 0)
        {
            _fatigue = 0;
        }
    }

    public static void ChangeLocation(LocationData locationData, int raidTime, int arrivalTime)
    {
        OnTimeUpdated = null;
        _inventory.ContainerUpdated = null;
        _currentLocationData = locationData;
        _timeBeforeArrival = arrivalTime;
        _raidTimeLeft = raidTime + arrivalTime;
        Time.timeScale = 1;
        SceneManager.LoadScene("GoingScene");
    }

    public static void SetLightSourceData(LightSourceData lightSourceData)
    {
        _lightSourceData = lightSourceData;
    }

    public static void SetActiveQuest(Quest quest)
    {
        _activeQuest = quest;
        if (_activeQuest != null)
        {
            _activeQuest.OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
        }
    }

    public static void SetQuestProgress(int progress)
    {
        _questsProgress = progress;
    }

    public static void AddStatus(Status status)
    {
        foreach (Status stat in _activeStatuses)
        {
            if (stat.Data == status.Data)
            {
                return;
            }
        }

        OnTimeUpdated += status.TimeUpdated;
        _activeStatuses.Add(status);
        Debug.Log("Added status");
    }

    public static void RemoveStatus(Status status)
    {
        if (!_activeStatuses.Contains(status))
        {
            return;
        }

        OnTimeUpdated -= status.TimeUpdated;
        _activeStatuses.Remove(status);
    }

    public static void LoadSaveData(SaveDatas.PlayerSaveData playerSaveData)
    {
        _inventory = new ItemContainer(playerSaveData.InventorySaveData);
        _water = playerSaveData.Water;
        _kcal = playerSaveData.Calories;

        Vector3 playerPos = new Vector3(playerSaveData.PlayerPos.xPos, playerSaveData.PlayerPos.yPos, playerSaveData.PlayerPos.zPos);
        //GameObject.FindGameObjectWithTag("Player").transform.localPosition = playerPos;
        _difficulty = playerSaveData.DifficultyData;

        _globalTime = playerSaveData.GlobalTime;
        _questsProgress = playerSaveData.QuestsProgress;

        if (playerSaveData.QuestSave != null && playerSaveData.QuestSave.Data != null)
        {
            _activeQuest = new Quest(playerSaveData.QuestSave);
        }
    }
}