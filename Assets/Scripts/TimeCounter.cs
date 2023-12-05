using UnityEngine;
using System.Collections;
using Statuses;

public class TimeCounter : MonoBehaviour
{
    private float _oneMinuteLength;
    private float _kcalDecrease = -2000 / 1440f;
    private float _waterDecrease = -2000 / 1440f;
    private float _fatigueDacrease = -100 / 1440f;
    private float _happinessDecrease = -50 / 4320f;
    [SerializeField] private StatusData _starvationStatusData;
    [SerializeField] private StatusData _dehydrationStatusData;
    [SerializeField] private StatusData _depressionStatusData;
    [SerializeField] private StatusData _fatigueStatusData;

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
            float kcalDebuff = 0;
            float mlDebuff = 0;
            float fatigueDebuff = 0;
            float happinessDebuff = 0;
            float HPDebuff = 0;

            foreach (Status status in GlobalRepository.ActiveStatuses)
            {
                foreach (EffectData effectData in status.GetActiveEffects())
                {
                    foreach (EffectData.BuffData buff in effectData.Buffs)
                    {
                        switch (buff.StatToChange)
                        {
                            case Statuses.StatToChangeEnum.KCal:
                                kcalDebuff += buff.Change;
                                break;
                            case Statuses.StatToChangeEnum.ML:
                                mlDebuff += buff.Change;
                                break;
                            case Statuses.StatToChangeEnum.Fatigue:
                                fatigueDebuff += buff.Change;
                                break;
                            case Statuses.StatToChangeEnum.Happiness:
                                happinessDebuff += buff.Change;
                                break;
                            case Statuses.StatToChangeEnum.HP:
                                HPDebuff += buff.Change;
                                break;

                        }
                    }
                }
            }

            yield return new WaitForSeconds(_oneMinuteLength);
            GlobalRepository.AddTime(1);
            GlobalRepository.AddKcal(_kcalDecrease * KcalDecreaseBuff * (1 - kcalDebuff));
            GlobalRepository.AddWater(_waterDecrease * WaterDecreaseBebuff * (1 - mlDebuff));
            GlobalRepository.AddFatigue(_fatigueDacrease * (1 - fatigueDebuff));
            GlobalRepository.AddHappiness(_happinessDecrease * (1 - happinessDebuff));

            if (GlobalRepository.Kcal <= 0)
            {
                GlobalRepository.AddStatus(new StarvationStatus(_starvationStatusData));
            }
            if (GlobalRepository.Water <= 0)
            {
                GlobalRepository.AddStatus(new DehydrationStatus(_dehydrationStatusData));
            }
            if (GlobalRepository.Happiness <= 0)
            {
                GlobalRepository.AddStatus(new DepressionStatus(_depressionStatusData));
            }
            if (GlobalRepository.Fatigue <= 15)
            {
                GlobalRepository.AddStatus(new FatigueStatus(_fatigueStatusData));
            }
        }
    }
}
