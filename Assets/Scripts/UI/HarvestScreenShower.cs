using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class HarvestScreenShower : MonoBehaviour, IClosable
{
    [SerializeField] private ItemShower _itemToHarvestShower;
    [SerializeField] private ItemShower _itemBeingHarvestedShower;
    [SerializeField] private ItemShower[] _requirementShowers;
    [SerializeField] private ItemShower[] _inventoryShowers;
    [SerializeField] private TextMeshProUGUI _harvestItemName;
    [SerializeField] private TextMeshProUGUI _additionalInfo;
    [SerializeField] private TextMeshProUGUI _harvestTimeLeftText;
    [SerializeField] private TextMeshProUGUI _weightText;
    [SerializeField] private TextMeshProUGUI _optionCountText;
    [SerializeField] private ButtonHandler _startHarvestButton;
    [SerializeField] private ButtonHandler _cancelHarvestButton;
    [SerializeField] private IndexedButtonHandler _prevHarvest;
    [SerializeField] private IndexedButtonHandler _nextHarvest;
    [SerializeField] private GameObject _harvestInProgress;
    [SerializeField] private Image _weightBar;
    private int _optionIndex = 0;
    private Harvester.HarvestOption[] _harvestOptions;
    private int _harvestTimeLeft;
    private ScreensCloser _screensCloser;

    private void Awake()
    {
        _screensCloser = GameObject.FindObjectOfType<ScreensCloser>();
        this.gameObject.SetActive(false);
    }

    private void Start()
    {
        _prevHarvest.AddListener(ChangeHarvestOption);
        _nextHarvest.AddListener(ChangeHarvestOption);
        _cancelHarvestButton.AddListener(EndHarvest);
    }

    public void OpenHarvestMenu(ref Harvester.HarvestOption[] harvestOptions)
    {
        _screensCloser.CloseAllScreens();
        _optionIndex = 0;
        this.gameObject.SetActive(true);
        _harvestOptions = harvestOptions;
        _harvestInProgress.SetActive(false);
        ChangeHarvestOption(0);
        ShowHarvestOption(_harvestOptions[0]);
        ShowInventory();
    }

    public void CloseScreen()
    {
        EndHarvest();
        this.gameObject.SetActive(false);
    }

    private void ShowHarvestOption(Harvester.HarvestOption harvestOption)
    {        
        _startHarvestButton.RemoveListener(StartHarvest);
        bool ifCanBeHarvested = true;
        HarvesterData.HarvestOptionData harvestData = harvestOption.HarvestOptionData;
        _harvestItemName.text = string.Format("{0} ({1}/{2})", harvestData.ItemToHarvest.Name, harvestOption.HarvestsLeft, harvestData.MaxHarvests);
        _itemToHarvestShower.ShowItem(harvestData.ItemToHarvest);
        int harvestTime = Mathf.RoundToInt(harvestData.TimeToHarvest * GlobalRepository.LightSourceData.HarvestSpeed);
        string additionalInfo = TimeConverter.InsertTime("Time to harvest: {0}:{1}\n",harvestTime,TimeConverter.InsertionType.HourMinute);

        if (harvestOption.HarvestOptionData.KcalDebuff != 0)
        {
            additionalInfo += "Debuff: " + harvestData.KcalDebuff + " kcal/min\n";
        }
        if (harvestOption.HarvestOptionData.WaterDebuff != 0)
        {
            additionalInfo += "Debuff: " + harvestData.WaterDebuff + " ml/min";
        }

        _additionalInfo.text = additionalInfo;

        foreach (ItemShower shower in _requirementShowers)
        {
            shower.ShowItem(null);
        }

        for (int i = 0; i < harvestData.Requirements.Length; i++)
        {
            _requirementShowers[i].ShowItem(harvestData.Requirements[i].ItemRequirement);
            
            if (!GlobalRepository.Inventory.CheckIfHas(harvestData.Requirements[i].ItemRequirement.ItemData, harvestData.Requirements[i].ItemRequirement.Count))
            {
                ifCanBeHarvested = false;
            }
        }

        if (harvestOption.HarvestsLeft <= 0)
        {
            ifCanBeHarvested = false;
        }

        if (ifCanBeHarvested)
        {
            _startHarvestButton.AddListener(StartHarvest);
        }
    }

    private void ChangeHarvestOption(int index)
    {
        _optionIndex += index;

        if (_optionIndex < 0)
        {
            _optionIndex = _harvestOptions.Length - 1;
        }
        else if (_optionIndex >= _harvestOptions.Length)
        {
            _optionIndex = 0;
        }

        _optionCountText.text = string.Format("{0}/{1}", _optionIndex + 1, _harvestOptions.Length);
        ShowHarvestOption(_harvestOptions[_optionIndex]);
    }

    private void ShowInventory()
    {
        GlobalRepository.Inventory.ContainerUpdated?.Invoke();

        GlobalRepository.CountWeight();

        float r = 0.6f - 0.3f / GlobalRepository.MaxWeight * (GlobalRepository.MaxWeight - GlobalRepository.Weight);
        float g = 0.6f - 0.3f / GlobalRepository.MaxWeight * GlobalRepository.Weight;

        _weightBar.color = new Color(r, g, 0.3f);
        _weightBar.rectTransform.localScale = new Vector3(0.216f / GlobalRepository.MaxWeight * GlobalRepository.Weight, 0.216f, 0.216f);

        _weightText.text = string.Format("{0}/{1} KG", GlobalRepository.Weight, GlobalRepository.MaxWeight);

        for (int i = 0; i < _inventoryShowers.Length; i++)
        {
            _inventoryShowers[i].ShowItem(GlobalRepository.Inventory.Items[i]);
        }
    }

    private void StartHarvest()
    {
        _harvestInProgress.SetActive(true);
        _itemBeingHarvestedShower.ShowItem(_harvestOptions[_optionIndex].HarvestOptionData.ItemToHarvest);
        _harvestTimeLeft = _harvestOptions[_optionIndex].HarvestOptionData.TimeToHarvest;
        _harvestTimeLeft = Mathf.RoundToInt(_harvestTimeLeft * GlobalRepository.LightSourceData.HarvestSpeed);
        _harvestTimeLeftText.text = TimeConverter.InsertTime("Time to harvest: {0}:{1}", _harvestTimeLeft, TimeConverter.InsertionType.HourMinute);
        GlobalRepository.OnTimeUpdated += HarvestInProgress;
        Time.timeScale = 20;
    }

    private void HarvestInProgress()
    {
        _harvestTimeLeft -= 1;
        _harvestTimeLeftText.text = TimeConverter.InsertTime("Time to harvest: {0}:{1}", _harvestTimeLeft, TimeConverter.InsertionType.HourMinute);
        GlobalRepository.AddKcal(_harvestOptions[_optionIndex].HarvestOptionData.KcalDebuff);
        GlobalRepository.AddWater(_harvestOptions[_optionIndex].HarvestOptionData.WaterDebuff);

        if (_harvestTimeLeft <= 0)
        {
            GlobalRepository.Inventory.AddItem(_harvestOptions[_optionIndex].HarvestOptionData.ItemToHarvest,false);

            foreach (HarvesterData.HarvestOptionData.Requirement requirement in _harvestOptions[_optionIndex].HarvestOptionData.Requirements)
            {
                if (requirement.IsDisposable)
                {
                    GlobalRepository.Inventory.RemoveItem(requirement.ItemRequirement, requirement.ItemRequirement.Count);
                }
            }

            _harvestOptions[_optionIndex].ChangeHarvestsLeftAmount(-1);

            EndHarvest();
        }
    }

    private void EndHarvest()
    {
        if (_harvestOptions == null)
        {
            return;
        }

        ShowHarvestOption(_harvestOptions[_optionIndex]);
        _harvestInProgress.SetActive(false);
        GlobalRepository.OnTimeUpdated -= HarvestInProgress;
        ShowInventory();
        Time.timeScale = 1;
    }
}
