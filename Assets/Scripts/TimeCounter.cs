using UnityEngine;
using System.Collections;

public class TimeCounter : MonoBehaviour
{
    private float _oneMinuteLength;
    private float _kcalDecrease = -2000 / 1440f;
    private float _waterDecrease = -2000 / 1440f;
    private float _fatigueDeacrease = -100 / 1440f;

    public float KcalDecreaseBuff = 1;
    public float WaterDecreaseBebuff = 1;

    private void Start()
    {
        _oneMinuteLength = GlobalRepository.Difficulty.DayCycleLength / 24f;
        StartCoroutine(AddTime());
    }

    IEnumerator AddTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(_oneMinuteLength);
            GlobalRepository.AddTime(1);
            GlobalRepository.AddKcal(_kcalDecrease * KcalDecreaseBuff);
            GlobalRepository.AddWater(_waterDecrease * WaterDecreaseBebuff);
            GlobalRepository.AddFatigue(_fatigueDeacrease);
        }
    }
}
