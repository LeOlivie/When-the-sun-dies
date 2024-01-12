using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using Statuses;
using UnityEngine.PlayerLoop;
using SaveDatas;

public static class GlobalRepository
{
    public enum SkillType { DIY, Electronics, Bakery, Building };
    public enum WeatherTypeEnum { ClearWeather, Foggy, Rainy, Windy };
    private static PlayerVarsInstance _playerVars = new PlayerVarsInstance();
    private static SystemVarsInstance _systemVars = new SystemVarsInstance();

    public delegate void TimeUpdatedDelegate();
    public static event TimeUpdatedDelegate OnTimeUpdated;
    
    public static PlayerVarsInstance PlayerVars => _playerVars;
    public static SystemVarsInstance SystemVars => _systemVars;
    public static void Reset()
    {
        OnTimeUpdated = null;
        _playerVars = new PlayerVarsInstance();
        _systemVars = new SystemVarsInstance();
    }

    public static void CountWeight()
    {
        _playerVars.Weight = 0;

        foreach (Item item in _playerVars.Inventory.Items)
        {
            if (item == null || item.ItemData == null)
            {
                continue;
            }
            _playerVars.Weight += item.Count * item.Weight;
        }

        _playerVars.Weight = float.Parse(Math.Round(_playerVars.Weight, 2).ToString());
    }

    public static void AddTime(uint minutesCount)
    {
        if (_systemVars.RaidTimeLeft > 0)
        {
            _systemVars.RaidTimeLeft -= (int)minutesCount;
        }

        if (_systemVars.TimeBeforeArrival > 0)
        {
            _systemVars.TimeBeforeArrival -= (int)minutesCount;
        }

        _systemVars.WeatherChangeTime -= (int)minutesCount;

        if (_systemVars.WeatherChangeTime <= 0)
        {
            int newWeatherIndex = UnityEngine.Random.Range(0, Enum.GetValues(typeof(WeatherTypeEnum)).Length);
            _systemVars.WeatherType = (WeatherTypeEnum)(Enum.GetValues(_systemVars.WeatherType.GetType())).GetValue(newWeatherIndex);
            _systemVars.WeatherChangeTime = UnityEngine.Random.Range(120, _systemVars.Difficulty.WeatherMaxDuration);
        }

        _systemVars.GlobalTime += (int)minutesCount;

        if (_systemVars.GlobalTime % 1440 == 0)
        {
            _systemVars.ShowDayChange = true;
        }

        OnTimeUpdated?.Invoke();
    }

    public static void ChangeLocation(LocationData locationData, int raidTime, int arrivalTime)
    {
        OnTimeUpdated = null;

        foreach (Status status in _playerVars.ActiveStatuses)
        {
            OnTimeUpdated += status.TimeUpdated;
        }

        _playerVars.Inventory.ContainerUpdated = null;
        _systemVars.CurrentLocationData = locationData;
        _systemVars.TimeBeforeArrival = arrivalTime;
        _systemVars.RaidTimeLeft = raidTime + arrivalTime;
        Time.timeScale = 1;
        SceneManager.LoadScene("GoingScene");
    }


    public static void AddStatus(Status status)
    {
        foreach (Status stat in _playerVars.ActiveStatuses)
        {
            if (stat.Data.name == status.Data.name)
            {
                return;
            }
        }

        OnTimeUpdated += status.TimeUpdated;
        _playerVars.ActiveStatuses.Add(status);
        Debug.Log("Added status");
    }

    public static void RemoveStatus(Status status)
    {
        if (!_playerVars.ActiveStatuses.Contains(status))
        {
            return;
        }

        OnTimeUpdated -= status.TimeUpdated;
        _playerVars.ActiveStatuses.Remove(status);
    }

    public static void LoadSaveData(SaveDatas.PlayerSaveData playerSaveData)
    {
        _playerVars.Inventory = new ItemContainer(playerSaveData.InventorySaveData);
        _playerVars.Water = playerSaveData.Water;
        _playerVars.KCal = playerSaveData.Calories;

        Vector3 playerPos = new Vector3(playerSaveData.PlayerPos.xPos, playerSaveData.PlayerPos.yPos, playerSaveData.PlayerPos.zPos);
        _systemVars.Difficulty = playerSaveData.DifficultyData;

        _systemVars.GlobalTime = playerSaveData.GlobalTime;
        _playerVars.QuestsProgress = playerSaveData.QuestsProgress;

        _playerVars.Health = playerSaveData.Health;
        _systemVars.IsStartKitReceived = playerSaveData.IsStartKitReceived;
        _playerVars.LastEatenFood = playerSaveData.LastEatenFood;

        if (playerSaveData.QuestSave != null && playerSaveData.QuestSave.Data != null)
        {
            _systemVars.ActiveQuest = new Quest(playerSaveData.QuestSave);
        }

        foreach (StatusSaveData saveData in playerSaveData.statusSaveDatas)
        {
            GlobalRepository.AddStatus(new Status(saveData));
        }
    }
}