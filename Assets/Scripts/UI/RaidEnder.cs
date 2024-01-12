using UnityEngine;
using TMPro;

public class RaidEnder : MonoBehaviour
{
    public delegate void RaidEndedDelegate();
    public event RaidEndedDelegate RaidEnded;
    [SerializeField] private ButtonHandler _leaveButton;
    [SerializeField] private LocationData _baseData;
    [SerializeField] private TextMeshProUGUI _timeLeftText;
    private Saver saver;

    private void Start()
    {
        saver = GameObject.FindObjectOfType<Saver>();
        _leaveButton.AddListener(EndRaid);
        GlobalRepository.OnTimeUpdated += ShowRaidTime;

        if (GlobalRepository.SystemVars.CurrentLocationData == null || GlobalRepository.SystemVars.CurrentLocationData.Name == "Base")
        {
            _leaveButton.gameObject.SetActive(false);
        }
    }

    private void ShowRaidTime()
    {
        string hrsLeft = (GlobalRepository.SystemVars.RaidTimeLeft / 60).ToString();
        string minsLeft = (GlobalRepository.SystemVars.RaidTimeLeft - GlobalRepository.SystemVars.RaidTimeLeft / 60 * 60).ToString();
        string timeColor = "#FFFFFF";

        if (hrsLeft.Length < 2)
        {
            hrsLeft = "0" + hrsLeft;
        }

        if (minsLeft.Length < 2)
        {
            minsLeft = "0" + minsLeft;
        }

        if (GlobalRepository.SystemVars.RaidTimeLeft / 60 <= 0)
        {
            timeColor = "#FF0000";
        }

        _timeLeftText.text = string.Format("Time before leaving\n<size=20><color={0}>{1}:{2}</color></size>", timeColor, hrsLeft, minsLeft);

        if (GlobalRepository.SystemVars.RaidTimeLeft <= 0)
        {
            EndRaid();
        }
    }

    public void EndRaid()
    {
        RaidEnded?.Invoke();
        RaidEnded = null;
        saver.SaveLocation();
        GlobalRepository.ChangeLocation(_baseData, 0, Mathf.CeilToInt(GlobalRepository.SystemVars.CurrentLocationData.Distance / 2f * 60f));
    }
}
