using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class HarvestScreenShower : MonoBehaviour
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
    private HarvesterData _harvesterData;
    private int _harvestTimeLeft;

    private void Start()
    {
        _prevHarvest.AddListener(ChangeHarvestOption);
        _nextHarvest.AddListener(ChangeHarvestOption);
        _cancelHarvestButton.AddListener(EndHarvest);
    }

    public void OpenHarvestMenu(HarvesterData harvesterData)
    {
        _optionIndex = 0;
        this.gameObject.SetActive(true);
        _harvesterData = harvesterData;
        _harvestInProgress.SetActive(false);
        ChangeHarvestOption(0);
        ShowHarvestOption(_harvesterData.HarvestOptions[0]);
        ShowInventory();
    }

    public void CloseHarvestMenu()
    {
        EndHarvest();
        this.gameObject.SetActive(false);
    }

    private void ShowHarvestOption(HarvesterData.HarvestOption harvestOption)
    {
        _startHarvestButton.RemoveListener(StartHarvest);

        bool ifCanBeHarvested = true;

        _harvestItemName.text = harvestOption.ItemToHarvest.Name;

        _itemToHarvestShower.ShowItem(harvestOption.ItemToHarvest);

        string additionalInfo = harvestOption.TimeToHarvest + " minutes\n";

        if (harvestOption.KcalDebuff != 0)
        {
            additionalInfo += "Debuff: " + harvestOption.KcalDebuff + " kcal/min\n";
        }
        if (harvestOption.WaterDebuff != 0)
        {
            additionalInfo += "Debuff: " + harvestOption.WaterDebuff + " ml/min";
        }

        _additionalInfo.text = additionalInfo;

        foreach (ItemShower shower in _requirementShowers)
        {
            shower.ShowItem(null);
        }

        for (int i = 0; i < harvestOption.Requirements.Length; i++)
        {
            _requirementShowers[i].ShowItem(harvestOption.Requirements[i].ItemRequirement);
            
            if (!GlobalRepository.Inventory.CheckIfHas(harvestOption.Requirements[i].ItemRequirement.ItemData, harvestOption.Requirements[i].ItemRequirement.Count))
            {
                ifCanBeHarvested = false;
            }
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
            _optionIndex = _harvesterData.HarvestOptions.Length - 1;
        }
        else if (_optionIndex >= _harvesterData.HarvestOptions.Length)
        {
            _optionIndex = 0;
        }

        _optionCountText.text = string.Format("{0}/{1}", _optionIndex + 1, _harvesterData.HarvestOptions.Length);
        ShowHarvestOption(_harvesterData.HarvestOptions[_optionIndex]);
    }

    private void ShowInventory()
    {
        GlobalRepository.Inventory.ContainerUpdated();

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
        _itemBeingHarvestedShower.ShowItem(_harvesterData.HarvestOptions[_optionIndex].ItemToHarvest);
        _harvestTimeLeft = _harvesterData.HarvestOptions[_optionIndex].TimeToHarvest;
        _harvestTimeLeftText.text = _harvestTimeLeft + " minutes";
        GlobalRepository.OnTimeUpdated += HarvestInProgress;
        Time.timeScale = 20;
    }

    private void HarvestInProgress()
    {
        _harvestTimeLeft -= 1;
        _harvestTimeLeftText.text = _harvestTimeLeft + " minutes";
        GlobalRepository.AddKcal(_harvesterData.HarvestOptions[_optionIndex].KcalDebuff);
        GlobalRepository.AddWater(_harvesterData.HarvestOptions[_optionIndex].WaterDebuff);

        if (_harvestTimeLeft <= 0)
        {
            GlobalRepository.Inventory.AddItem(_harvesterData.HarvestOptions[_optionIndex].ItemToHarvest,false);

            foreach (HarvesterData.HarvestOption.Requirement requirement in _harvesterData.HarvestOptions[_optionIndex].Requirements)
            {
                if (requirement.IsDisposable)
                {
                    GlobalRepository.Inventory.RemoveItem(requirement.ItemRequirement, requirement.ItemRequirement.Count);
                }
            }

            EndHarvest();
        }
    }

    private void EndHarvest()
    {
        _harvestInProgress.SetActive(false);
        GlobalRepository.OnTimeUpdated -= HarvestInProgress;
        ShowInventory();
        Time.timeScale = 1;
    }
}
