using UnityEngine;
using System.Collections;

public class TimeCounter : MonoBehaviour
{
    [SerializeField] private DifficultyData _difficultyData;
    private float _oneMinuteLength;
    private float _kcalDecrease = -2000 / 60f / 24f;
    private float _waterDecrease = -2000 / 60f / 24f;

    private void Start()
    {
        GlobalRepository.SetDifficulty(_difficultyData);
        _oneMinuteLength = GlobalRepository.Difficulty.DayCycleLength / 24f;
        StartCoroutine(AddTime());
    }

    IEnumerator AddTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(_oneMinuteLength);
            GlobalRepository.AddTime(1);
            GlobalRepository.AddKcal(_kcalDecrease);
            GlobalRepository.AddWater(_waterDecrease);
        }
    }
}
