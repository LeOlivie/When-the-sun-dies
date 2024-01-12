using UnityEngine;
using TMPro;

public class TimeShower : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private void Start()
    {
        ShowTime();
        GlobalRepository.OnTimeUpdated += ShowTime;
    }

    private void ShowTime()
    {
        string text = "";
        int days = GlobalRepository.SystemVars.GlobalTime / 1440;
        int hours = (GlobalRepository.SystemVars.GlobalTime - days * 1440) / 60; 

        if (hours < 10)
        {
            text += "0";
        }

        text += hours.ToString() + ";";
        int minutes = GlobalRepository.SystemVars.GlobalTime - days * 1440 - hours * 60;

        if (minutes < 10)
        {
            text += "0";
        }

        text += minutes.ToString();

        _text.text = text;

    }
}
