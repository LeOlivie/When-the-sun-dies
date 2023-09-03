using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using TMPro;

public class MapScreenShower : MonoBehaviour, IClosable
{
    [Serializable]
    public struct LocationsClusterButton
    {
        [SerializeField] private IndexedButtonHandler _locationBtnHandler;
        [SerializeField] private LocationsClusterData _locationClusterData;
        [SerializeField] private Sprite _selectedBtnSprite;
        [SerializeField] private Sprite _unselectedBtnSprite;

        public IndexedButtonHandler LocationBtnHandler => _locationBtnHandler;
        public LocationsClusterData LocationClusterData => _locationClusterData;
        public Sprite SelectedBtnSprite => _selectedBtnSprite;
        public Sprite UnselectedBtnSprite => _unselectedBtnSprite;
    }

    [SerializeField] private List<LocationsClusterButton> _locationClustersButtons;
    [SerializeField] private IndexedButtonHandler[] _locationBtns;
    [SerializeField] private RadioScreenOpener _radioScreenOpener;
    [SerializeField] private Joystick _joystick;
    [SerializeField] private TextMeshProUGUI _infoText;
    [SerializeField] private TextMeshProUGUI _selectBtnText;
    [SerializeField] private TextMeshProUGUI _noLocsAvailableText;
    [SerializeField] private ButtonHandler _selectBtn;
    [SerializeField] private GameObject _selectLocFromClusterMenu;
    [SerializeField] private GameObject _lightSourceMenu;
    [SerializeField] private IndexedButtonHandler _prevLS;
    [SerializeField] private IndexedButtonHandler _nextLS;
    [SerializeField] private TextMeshProUGUI _lightSourcePageText;
    [SerializeField] private TextMeshProUGUI _lightSourceInfoText;
    [SerializeField] private ItemShower _lightSourceItemShower;
    [SerializeField] private Color _unavailableColor;
    [SerializeField] private Color _availableColor;
    [SerializeField] private Item _noLightSourceItem;
    private Radio _radio;
    private ScreensCloser _screensCloser;
    private LocationData _selectedLocation;
    private int _raidTime;
    private int _selectedClusterIndex;
    private List<Item> _lightSources;
    private List<string> _lightSourcesNames;
    private int _lightSourceIndex = 0;

    private void Awake()
    {
        _screensCloser = GameObject.FindObjectOfType<ScreensCloser>(true);
        _radio = _radioScreenOpener.Radio;
        this.gameObject.SetActive(false);
    }

    private void Start()
    {
        for (int i = 0; i < _locationClustersButtons.Count; i++)
        {
            _locationClustersButtons[i].LocationBtnHandler.SetIndex(i);
            _locationClustersButtons[i].LocationBtnHandler.AddListener(ChooseLocationFromCluster);
        }

        for (int i = 0; i < _locationBtns.Length; i++)
        {
            _locationBtns[i].SetIndex(i);
            _locationBtns[i].AddListener(ShowLocationInfo);
        }

        _prevLS.SetIndex(-1);
        _nextLS.SetIndex(1);
    }

    public void OpenScreen()
    {
        _screensCloser.CloseAllScreens();
        this.gameObject.SetActive(true);
        _infoText.text = "-Select location-";
        _selectBtnText.text = "";
        _selectBtn.RemoveListener(SelectLocation);
        _selectBtn.RemoveListener(Departure);
        _lightSourceMenu.SetActive(false);
        _joystick.enabled = false;


        _selectLocFromClusterMenu.SetActive(false);
        _lightSources = new List<Item>();
        _lightSourcesNames = new List<string>();
        _lightSources.Add(_noLightSourceItem);

        foreach (LocationsClusterButton locationClusterButton in _locationClustersButtons)
        {
            if (locationClusterButton.LocationClusterData is EventLocClusterData)
            {
                bool isEventActive = _radio.ActiveEventLocCluster != null && _radio.ActiveEventLocCluster == locationClusterButton.LocationClusterData && _radio.EventSwitchOffTime > GlobalRepository.GlobalTime;
                locationClusterButton.LocationBtnHandler.gameObject.SetActive(isEventActive);
            }
        }

        foreach (Item item in GlobalRepository.Inventory.Items)
        {
            if (item == null || item.ItemData == null)
            {
                continue;
            }

            if (!(item.ItemData is LightSourceData) || _lightSourcesNames.Contains(item.Name))
            {
                continue;
            }

            _lightSources.Add(item);
            _lightSourcesNames.Add(item.Name);
        }

        for (int i = 0; i < _locationClustersButtons.Count; i++)
        {
            _locationClustersButtons[i].LocationBtnHandler.GetComponent<Image>().sprite = _locationClustersButtons[i].UnselectedBtnSprite;
        }
    }

    public void CloseScreen()
    {
        this.gameObject.SetActive(false);
        GlobalRepository.OnTimeUpdated -= UpdateScavTime;
        GlobalRepository.OnTimeUpdated -= GetRaidTime;
        _joystick.enabled = true;
    }

    private void GetRaidTime()
    {
        if (_selectedLocation == null)
        {
            return;
        }

        _raidTime = Mathf.FloorToInt(_selectedLocation.Distance / 2f * 60) * (-2);
        int globalTime = GlobalRepository.GlobalTime - GlobalRepository.GlobalTime / 1440 * 1440;

        if (globalTime < GlobalRepository.Difficulty.ScavTimeEnd * 60) //Dawn
        {
            _raidTime += GlobalRepository.Difficulty.ScavTimeEnd * 60 - globalTime;
        }
        else if (globalTime >= GlobalRepository.Difficulty.ScavTimeStart * 60) //Sunset
        {
            _raidTime += 24 * 60 - globalTime;
            _raidTime += GlobalRepository.Difficulty.ScavTimeEnd * 60;
        }

        if (_raidTime <= 0)
        {
            _selectBtnText.color = _unavailableColor;
        }
    }

    private void ChooseLocationFromCluster(int index)
    {
        _selectedClusterIndex = index;
        _selectBtn.gameObject.SetActive(false);
        _selectLocFromClusterMenu.SetActive(true);

        for (int i = 0; i < _locationClustersButtons.Count; i++)
        {
            _locationClustersButtons[i].LocationBtnHandler.GetComponent<Image>().sprite = _locationClustersButtons[i].UnselectedBtnSprite;
        }

        _locationClustersButtons[index].LocationBtnHandler.GetComponent<Image>().sprite = _locationClustersButtons[index].SelectedBtnSprite;

        foreach (IndexedButtonHandler btn in _locationBtns)
        {
            btn.gameObject.SetActive(false);
        }

        for (int i = 0; i < _locationClustersButtons[index].LocationClusterData.LocationDatas.Length; i++)
        {
            _locationBtns[i].gameObject.SetActive(true);
            _locationBtns[i].transform.GetComponentInChildren<TextMeshProUGUI>().text = _locationClustersButtons[index].LocationClusterData.LocationDatas[i].Name;
        }

        _noLocsAvailableText.gameObject.SetActive(_locationClustersButtons[index].LocationClusterData.LocationDatas.Length <= 0);
    }

    private void ShowLocationInfo(int index)
    {
        _selectLocFromClusterMenu.SetActive(false);
        _selectBtn.gameObject.SetActive(true);
        GlobalRepository.OnTimeUpdated -= GetRaidTime;
        _selectBtn.RemoveListener(SelectLocation);
        _selectBtn.RemoveListener(Departure);
        _selectBtn.RemoveListener(ShowLightMenu);
        _prevLS.RemoveListener(ShowLightSourceInfo);
        _nextLS.RemoveListener(ShowLightSourceInfo);
        GlobalRepository.OnTimeUpdated -= UpdateScavTime;
        _lightSourceMenu.SetActive(false);

        _selectedLocation = _locationClustersButtons[_selectedClusterIndex].LocationClusterData.LocationDatas[index];

        string locationInfo = string.Format("<size=30>{0}</size>\n\n\n{1}\n\nDistance: {2}km\n", _selectedLocation.Name, _selectedLocation.Description, _selectedLocation.Distance);
        _infoText.text = locationInfo;

        _selectBtn.AddListener(SelectLocation);
        _selectBtnText.text = "Select";
        _selectBtnText.color = _availableColor;
    }

    private void SelectLocation()
    {
        GlobalRepository.OnTimeUpdated -= GetRaidTime;
        GlobalRepository.OnTimeUpdated += GetRaidTime;
        GlobalRepository.OnTimeUpdated += UpdateScavTime;
        GetRaidTime();
        UpdateScavTime();
    }

    private void UpdateScavTime()
    {
        _selectBtn.RemoveListener(SelectLocation);
        _selectBtn.RemoveListener(ShowLightMenu);
        _lightSourceMenu.SetActive(false);

        string timeInfo = string.Format("<size=30>{0}</size>\n\n\n", _selectedLocation.Name);
        timeInfo += TimeConverter.InsertTime("Time to arrive: {0}:{1}\n\n", Mathf.FloorToInt(_selectedLocation.Distance / 2f * 60), TimeConverter.InsertionType.HourMinute);
        timeInfo += TimeConverter.InsertTime("Time to return: {0}:{1}\n\n", Mathf.FloorToInt(_selectedLocation.Distance / 2f * 60), TimeConverter.InsertionType.HourMinute);
        timeInfo += $"Safe scavange time: {GlobalRepository.Difficulty.ScavTimeStart}:00-0{GlobalRepository.Difficulty.ScavTimeEnd}:00\n\n";
        
        if(_raidTime <= 0)
        {
            timeInfo += "<color=#EC6F6F>-Unable to safely return until dawn-</color>";
            _selectBtnText.text = "Select";
            _selectBtnText.color = _unavailableColor;
            _raidTime = 0;
            _infoText.text = timeInfo;
            return;
        }

        timeInfo += TimeConverter.InsertTime("Estimated scavange time: {0}:{1}\n\n", _raidTime, TimeConverter.InsertionType.HourMinute);
        _selectBtnText.color = _availableColor;
        _selectBtnText.text = "Select";
        _infoText.text = timeInfo;

        _selectBtn.AddListener(ShowLightMenu);
    }

    private void ShowLightMenu()
    {
        GlobalRepository.OnTimeUpdated -= UpdateScavTime;
        _lightSourceMenu.SetActive(true);
        _lightSourceIndex = 0;
        ShowLightSourceInfo(0);
        _selectBtn.RemoveListener(ShowLightMenu);
        _prevLS.AddListener(ShowLightSourceInfo);
        _nextLS.AddListener(ShowLightSourceInfo);
    }

    private void ShowLightSourceInfo(int change)
    {
        _selectBtn.RemoveListener(Departure);

        _lightSourceIndex += change;

        if (_lightSourceIndex < 0)
        {
            _lightSourceIndex = _lightSources.Count - 1;
        }
        else if (_lightSourceIndex >= _lightSources.Count)
        {
            _lightSourceIndex = 0;
        }

        _lightSourcePageText.text = $"{_lightSourceIndex+1}/{_lightSources.Count}";
        LightSourceData lsData = (LightSourceData)_lightSources[_lightSourceIndex].ItemData;
        _lightSourceItemShower.ShowItem(_lightSources[_lightSourceIndex]);
        _lightSourceInfoText.text = lsData.Name + "\n\n";
        _lightSourceInfoText.text += $"Search time: {ConvertLightStat(lsData.SearchSpeed * 100)}%\n";
        _lightSourceInfoText.text += $"Harvest time: {ConvertLightStat(lsData.HarvestSpeed * 100)}%\n";
        _lightSourceInfoText.text += $"Light radius: {ConvertLightStat(lsData.LightRadius * 100)}%\n";
        _lightSourceInfoText.text += $"Light intensity: {ConvertLightStat(lsData.LightIntensity * 100)}%\n\n";

        if (lsData.DisposableItem.ItemData != null)
        {
            _lightSourceInfoText.text += $"Disposable item\n{lsData.DisposableItem.ItemData.Name}";

            if (!GlobalRepository.Inventory.CheckIfHas(lsData.DisposableItem.ItemData, lsData.DisposableItem.Count))
            {
                _selectBtnText.text = "Depart";
                _selectBtnText.color = _unavailableColor;
                return;
            }
        }
        else
        {
            _lightSourceInfoText.text += $"Disposable item\nNone";
        }

        _selectBtnText.text = "Depart";
        _selectBtnText.color = _availableColor;
        _selectBtn.AddListener(Departure);
    }

    private string ConvertLightStat(float stat)
    {
        if (stat-100 >= 0)
        {
            return $"+{stat-100}";
        }

        return $"{stat-100}";
    }

    private void Departure()
    {
        if (_raidTime <= 0) 
        {
            return;
        }

        GameObject.FindObjectOfType<Saver>().SavePlayer();
        GameObject.FindObjectOfType<Saver>().SaveBase();
        GlobalRepository.SetLightSourceData((LightSourceData)_lightSources[_lightSourceIndex].ItemData);
        GlobalRepository.Inventory.RemoveItem(((LightSourceData)_lightSources[_lightSourceIndex].ItemData).DisposableItem, 1);
        GlobalRepository.ChangeLocation(_selectedLocation, _raidTime,  Mathf.CeilToInt(_selectedLocation.Distance / 2f * 60f));
        GlobalRepository.OnTimeUpdated -= UpdateScavTime;
    }

}