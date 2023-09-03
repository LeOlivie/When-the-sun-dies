using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GoingSceneShower : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private void Start()
    {
        GlobalRepository.OnTimeUpdated += ShowText;
        ShowText();
        Time.timeScale = 30 * GlobalRepository.Difficulty.DayCycleLength / 24;
    }

    private void ShowText()
    {
        string hours = (GlobalRepository.TimeBeforeArrival / 60).ToString();
        string minutes = (GlobalRepository.TimeBeforeArrival - int.Parse(hours) * 60).ToString();

        if (hours.Length < 2)
        {
            hours = "0" + hours;
        }

        if(minutes.Length < 2) 
        {
            minutes= "0" + minutes;
        }

        _text.text = string.Format("Currently going to...\n\n{0}\n\nEstimated time of arrival\n{1}:{2}",GlobalRepository.CurrentLocationData.Name,hours,minutes);

        if (GlobalRepository.TimeBeforeArrival <= 0)
        {
            GlobalRepository.OnTimeUpdated -= ShowText;
            Time.timeScale = 1;
            SceneManager.LoadScene(GlobalRepository.CurrentLocationData.LocationID);
        }
    }
}
