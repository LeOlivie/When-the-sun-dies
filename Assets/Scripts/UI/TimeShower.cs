using System.Collections;
using UnityEngine;
using TMPro;

public class TimeShower : MonoBehaviour
{
    private float _oneMinuteLength;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private TextMeshProUGUI _timeLeftText;

    private void Start()
    {
        ShowTime();
        _oneMinuteLength = GlobalRepository.Difficulty.DayCycleLength / 24f;
        GlobalRepository.OnTimeUpdated += ShowTime;
    }

    private void ShowTime()
    {
        string text = "";
        int days = GlobalRepository.GlobalTime / 1440;
        int hours = (GlobalRepository.GlobalTime - days * 1440) / 60; 

        if (hours < 10)
        {
            text += "0";
        }

        text += hours.ToString() + ";";
        int minutes = GlobalRepository.GlobalTime - days * 1440 - hours * 60;

        if (minutes < 10)
        {
            text += "0";
        }

        text += minutes.ToString();

        _text.text = text;


        string hrsLeft = (GlobalRepository.RaidTimeLeft / 60).ToString();
        string minsLeft = (GlobalRepository.RaidTimeLeft - GlobalRepository.RaidTimeLeft / 60 * 60).ToString();
        string timeColor = "#FFFFFF";

        if (hrsLeft.Length < 2)
        {
            hrsLeft = "0" + hrsLeft;
        }

        if (minsLeft.Length < 2)
        {
            minsLeft = "0" + minsLeft;
        }

        if (GlobalRepository.RaidTimeLeft / 60 <= 0)
        {
            timeColor = "#FF0000";
        }

        _timeLeftText.text = string.Format("Time before leaving\n<size=20><color={0}>{1}:{2}</color></size>",timeColor, hrsLeft, minsLeft);
    }
}
