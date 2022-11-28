using System;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalRepository
{
    private static byte _minutes = 00;
    private static byte _hours = 12;
    private static ushort _days = 1;
    private static float _kcal = 2000;
    private static float _water = 2000;
    private static float _weight;
    private static float _maxWeight = 15;
    private static sbyte _happiness = 50;
    private static List<string> _lastEatenFood = new List<string>();

    private static DifficultyData _difficulty;
    private static ItemContainer _inventory = new ItemContainer(16, CountWeight);

    public delegate void TimeUpdatedDelegate();
    public static event TimeUpdatedDelegate OnTimeUpdated;

    public static byte Minutes => _minutes;
    public static byte Hours => _hours;
    public static ushort Days => _days;
    public static float Kcal => _kcal;
    public static float Water => _water;
    public static float Weight => _weight;
    public static float MaxWeight => _maxWeight;
    public static sbyte Happiness => _happiness;
    public static List<string> LastEatenFood => _lastEatenFood;
    public static DifficultyData Difficulty => _difficulty;
    public static ItemContainer Inventory => _inventory;


    public static void SetDifficulty(DifficultyData difficultyData)
    {
        if (_difficulty == null)
        {
            _difficulty = difficultyData;
            _maxWeight = _difficulty.MaxWeight;
        }
        else
        {
            throw new System.Exception("Trying to change difficulty.");
        }
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

        _days += (ushort)(minutesCount / 1440);
        minutesCount %= 1440;

        _hours += (byte)(minutesCount / 60);
        minutesCount %= 60;

        _minutes += (byte)(minutesCount);

        _hours += (byte)(_minutes / 60);
        _minutes = (byte)(_minutes % 60);

        _days += (byte)(_hours / 24);
        _hours = (byte)(_hours % 24);

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
}