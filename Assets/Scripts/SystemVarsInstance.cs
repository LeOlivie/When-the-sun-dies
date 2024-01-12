using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static GlobalRepository;

public class SystemVarsInstance
{
    private Quest _activeQuest = null;
    public int GlobalTime = 2520; //2160 = 1 day 12:00
    public int RaidTimeLeft = 90;
    public int TimeBeforeArrival = 0;
    public int WeatherChangeTime = 1;
    public WeatherTypeEnum WeatherType;
    public LocationData CurrentLocationData;
    public LightSourceData LightSourceData;
    public bool IsStartKitReceived;
    public bool ShowDayChange = true;
    public DifficultyData Difficulty;

    public Quest ActiveQuest
    {
        get { return _activeQuest; }
        set
        {
            _activeQuest = value;
            if (_activeQuest != null)
            {
                _activeQuest.OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
            }
        }
    }

    public float MaxWeight => Difficulty.MaxWeight;
}
