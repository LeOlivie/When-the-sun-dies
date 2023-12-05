using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class DayChangeShower : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _dayChangeText;

    private void Start()
    {
        GlobalRepository.OnTimeUpdated += CheckDayChange;
        this.gameObject.SetActive(false);
    }

    private void CheckDayChange()
    {
        if (GlobalRepository.ShowDayChange)
        {
            GlobalRepository.ShowDayChange = false;
            this.gameObject.SetActive(true);
            StartCoroutine(ShowDayChange());
        }
    }

    IEnumerator ShowDayChange()
    {
        int day = GlobalRepository.GlobalTime / 1440;
        string prevDayText = $"Day {day - 1}";

        for (int i = 0; i < prevDayText.Length; i++)
        {
            yield return new WaitForSeconds(0.1f);
            _dayChangeText.text += prevDayText[i];
        }

        yield return new WaitForSeconds(2f);

        for (int i = 0; i < (day-1).ToString().Length; i++)
        {
            yield return new WaitForSeconds(0.1f);
            _dayChangeText.text = _dayChangeText.text.Remove(_dayChangeText.text.Length - 1, 1);
        }

        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < day.ToString().Length; i++)
        {
            yield return new WaitForSeconds(0.1f);
            _dayChangeText.text += day.ToString()[i];
        }

        yield return new WaitForSeconds(2f);

        while (_dayChangeText.text.Length > 0)
        {
            _dayChangeText.text = _dayChangeText.text.Remove(_dayChangeText.text.Length - 1, 1);
            yield return new WaitForSeconds(0.1f);
        }

        this.gameObject.SetActive(false);
    }
}
