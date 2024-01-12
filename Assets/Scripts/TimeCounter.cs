using UnityEngine;
using System.Collections;
using Statuses;
using System;
using UnityEngine.SceneManagement;

public class TimeCounter : MonoBehaviour
{
    [Serializable]
    private struct StatusCheck
    {
        [SerializeField] private DependentVarEnum _dependentVar;
        [SerializeField] private float _triggerValue;
        [SerializeField] private StatusData _data;

        public DependentVarEnum DependentVar => _dependentVar;
        public StatusData Data => _data;
        public float TriggerValue => _triggerValue;
    }

    private float _oneMinuteLength;
    private float _kcalDecrease = -2000 / 1440f;
    private float _waterDecrease = -2000 / 1440f;
    private float _fatigueDacrease = -100 / 1440f;
    private float _happinessDecrease = -50 / 4320f;
    [SerializeField] private StatusCheck[] _statusChecks;

    public float KcalDecreaseBuff = 1;
    public float WaterDecreaseBebuff = 1;

    private void Start()
    {
        _oneMinuteLength = GlobalRepository.SystemVars.Difficulty.DayCycleLength / 24f;
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

            foreach (Status status in GlobalRepository.PlayerVars.ActiveStatuses)
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
            GlobalRepository.PlayerVars.KCal += _kcalDecrease * KcalDecreaseBuff * (1 - kcalDebuff);
            GlobalRepository.PlayerVars.Water += _waterDecrease * WaterDecreaseBebuff * (1 - mlDebuff);
            GlobalRepository.PlayerVars.Fatigue += _fatigueDacrease * (1 - fatigueDebuff);
            GlobalRepository.PlayerVars.Happiness += _happinessDecrease * (1 - happinessDebuff);
            GlobalRepository.PlayerVars.Health += HPDebuff;

            foreach (StatusCheck statusCheck in _statusChecks)
            {
                float value = (float)GlobalRepository.PlayerVars.GetType().GetProperty(statusCheck.DependentVar.ToString()).GetValue(GlobalRepository.PlayerVars);
                if (value <= statusCheck.TriggerValue)
                {
                    GlobalRepository.AddStatus(new Status(statusCheck.Data));
                }
            }
            Debug.Log(GlobalRepository.PlayerVars.Health);

            if (GlobalRepository.PlayerVars.Health <= 0)
            {
                Time.timeScale = 1;
                SceneManager.LoadScene("DeathScene");
            }
        }
    }
}
