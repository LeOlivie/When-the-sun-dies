using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalRepository;
using Statuses;
using System;

public class PlayerVarsInstance
{
    private float _kcal = 2000;
    private float _water = 2000;
    private float _happiness = 50;
    private float _fatigue = 100;
    private float _health = 1440;
    public float Weight;
    public int QuestsProgress;
    public List<Status> ActiveStatuses = new List<Status>();
    public List<string> LastEatenFood = new List<string>();
    public Dictionary<SkillType, int> Skills = new Dictionary<SkillType, int>();
    public ItemContainer Inventory = new ItemContainer(16, CountWeight);

    public float KCal 
    { 
        get 
        { 
            return _kcal; 
        } 
        set 
        {
            if (value <= 0)
            {
                _kcal = 0;
            }
            else if ( value > 2000)
            {
                _kcal = 2000;
            }
            else
            {
                _kcal = value;
            }
        } 
    }
    public float Water
    {
        get
        {
            return _water;
        }
        set
        {
            if (value <= 0)
            {
                _water = 0;
            }
            else if (value > 2000)
            {
                _water = 2000;
            }
            else
            {
                _water = value;
            }
        }
    }
    public float Happiness
    {
        get 
        { 
            return _happiness; 
        }
        set
        {
            if (value > 50)
            {
                _happiness = 50;
            }
            else if (value < 0)
            {
                _happiness = 0;
            }
            else
            {
                _happiness = value;
            }
        }
    }
    public float Fatigue
    {
        get { return _fatigue; }
        set
        {
            if (value > 100)
            {
                _fatigue = 100;
            }
            else if (value < 0)
            {
                _fatigue = 0;
            }
            else
            {
                _fatigue = value;
            }
        }
    }
    public float Health
    {
        get { return _health; }
        set
        {
            if (value > 1440)
            {
                _health = 1440;
            }
            else if (value < 0)
            {
                _health = 0;
            }
            else
            {
                _health = value;
            }
        }
    }

    public PlayerVarsInstance()
    {
        foreach (string str in Enum.GetNames(typeof(SkillType)))
        {
            SkillType skillType = (SkillType)Enum.Parse(typeof(SkillType), str);
            Skills.Add(skillType, 0);
        }
    }
}
