using System.Collections;
using UnityEngine;
using TMPro;

public class TimeShower : MonoBehaviour
{
    private float _oneMinuteLength;
    [SerializeField] private TextMeshProUGUI _text;

    private void Start()
    {
        _oneMinuteLength = GlobalRepository.Difficulty.DayCycleLength / 24f;
        StartCoroutine(TimeUpdate());
    }

    IEnumerator TimeUpdate()
    {
        while (true)
        {
            string text = "";

            if (GlobalRepository.Hours < 10)
            {
                text += "0";
            }

            text += GlobalRepository.Hours.ToString() + ";";

            if (GlobalRepository.Minutes < 10)
            {
                text += "0";
            }

            text += GlobalRepository.Minutes.ToString();

            _text.text = text;

            yield return new WaitForSeconds(_oneMinuteLength);
        }
    }
}
