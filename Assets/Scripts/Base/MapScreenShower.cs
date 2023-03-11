using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class MapScreenShower : MonoBehaviour, IClosable
{
    [Serializable]
    private struct LocationButton
    {
        [SerializeField] private IndexedButtonHandler _locationBtnHandler;
        [SerializeField] private LocationData _locationData;
        [SerializeField] private Sprite _selectedBtnSprite;
        [SerializeField] private Sprite _unselectedBtnSprite;

        public IndexedButtonHandler LocationBtnHandler => _locationBtnHandler;
        public LocationData LocationData => _locationData;
        public Sprite SelectedBtnSprite => _selectedBtnSprite;
        public Sprite UnselectedBtnSprite => _unselectedBtnSprite;
    }

    [SerializeField] private LocationButton[] _locationButtons;
    [SerializeField] private TextMeshProUGUI _infoText;
    [SerializeField] private TextMeshProUGUI _selectBtnText;
    [SerializeField] private ButtonHandler _selectBtn;
    private ScreensCloser _screensCloser;
    private LocationData _selectedLocation;
    private int _raidTime;

    private void Awake()
    {
        _screensCloser = GameObject.FindObjectOfType<ScreensCloser>(true);
        this.gameObject.SetActive(false);
    }

    private void Start()
    {
        for (int i = 0; i < _locationButtons.Length; i++)
        {
            _locationButtons[i].LocationBtnHandler.SetIndex(i);
            _locationButtons[i].LocationBtnHandler.AddListener(ShowLocationInfo);
        }    
    }

    public void OpenScreen()
    {
        _screensCloser.CloseAllScreens();
        this.gameObject.SetActive(true);
        _infoText.text = "-Select location-";
        _selectBtnText.text = "";
        _selectBtn.RemoveListener(SelectLocation);
        _selectBtn.RemoveListener(Departure);

        for (int i = 0; i < _locationButtons.Length; i++)
        {
            _locationButtons[i].LocationBtnHandler.GetComponent<Image>().sprite = _locationButtons[i].UnselectedBtnSprite;
        }
    }

    public void CloseScreen()
    {
        this.gameObject.SetActive(false);
        GlobalRepository.OnTimeUpdated -= UpdateScavTime;
    }

    private void ShowLocationInfo(int index)
    {
        _selectBtn.RemoveListener(SelectLocation);
        _selectBtn.RemoveListener(Departure);
        GlobalRepository.OnTimeUpdated -= UpdateScavTime;

        _selectedLocation = _locationButtons[index].LocationData;

        for (int i = 0; i < _locationButtons.Length; i++)
        {
            _locationButtons[i].LocationBtnHandler.GetComponent<Image>().sprite = _locationButtons[i].UnselectedBtnSprite;
        }

        _locationButtons[index].LocationBtnHandler.GetComponent<Image>().sprite = _locationButtons[index].SelectedBtnSprite;
        string locationInfo = string.Format("<size=30>{0}</size>\n\n\n{1}\n\nDistance: {2}km\n", _selectedLocation.Name, _selectedLocation.Description, _selectedLocation.Distance);
        _infoText.text = locationInfo;

        _selectBtn.AddListener(SelectLocation);
        _selectBtnText.text = "<color=#FFFFFF>Select</color>";
    }

    private void SelectLocation()
    {
        GlobalRepository.OnTimeUpdated += UpdateScavTime;
        UpdateScavTime();
    }

    private void UpdateScavTime()
    {
        _selectBtn.RemoveListener(SelectLocation);
        _selectBtn.RemoveListener(Departure);

        string timeInfo = string.Format("<size=30>{0}</size>\n\n\n", _selectedLocation.Name);
        timeInfo += "Time to arrive: " + ConvertTimeFormat(Mathf.FloorToInt(_selectedLocation.Distance / 2f * 60)) + "\n\n";
        timeInfo += "Time to return: " + ConvertTimeFormat(Mathf.FloorToInt(_selectedLocation.Distance / 2f * 60)) + "\n\n";
        timeInfo += "Safe scavange time: 23:00-06:00\n\n";

        int globalTime = GlobalRepository.GlobalTime - GlobalRepository.GlobalTime / 1440 * 1440; //Substract days count
        _raidTime = Mathf.FloorToInt(_selectedLocation.Distance / 2f * 60) * (-2);

        if (globalTime < 6 * 60) //Dawn at 6:00
        {
            _raidTime += 6 * 60 - globalTime;
        }
        else if (globalTime >= 23 * 60) //Sunset at 23:00
        {
            _raidTime += 24 * 60 - globalTime;
            _raidTime += 6 * 60;
        }
        
        if(_raidTime <= 0)
        {
            timeInfo += "<color=#EC6F6F>-Unable to safely return until dawn-</color>";
            _selectBtnText.text = "<color=#AE4646>Depart</color>";
            _raidTime = 0;
            _infoText.text = timeInfo;
            return;
        }

        timeInfo += string.Format("Estimated scavange time: {0}\n\n", ConvertTimeFormat(_raidTime));
        _selectBtnText.text = "<color=#FFFFFF>Depart</color>";
        _infoText.text = timeInfo;

        _selectBtn.AddListener(Departure);
    }

    private void Departure()
    {
        GlobalRepository.ChangeLocation(_selectedLocation, _raidTime,  Mathf.CeilToInt(_selectedLocation.Distance / 2f * 60f));
        GlobalRepository.OnTimeUpdated -= UpdateScavTime;
    }

    private string ConvertTimeFormat(int timeInMins)
    {
        string hours = (timeInMins / 60).ToString();
        string minutes = (timeInMins - timeInMins / 60 * 60).ToString();

        if (hours.Length < 2)
        {
            hours = "0" + hours;
        }

        if (minutes.Length < 2)
        {
            minutes = "0" + minutes;
        }

        string output = hours + ":" + minutes;

        return output;
    }

}