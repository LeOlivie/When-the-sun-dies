using UnityEngine;
using TMPro;

public class Crafter : MonoBehaviour
{
    [SerializeField] private CraftingRecipieShower[] _recipieShowers;
    [SerializeField] private GameObject _craftInProgress;
    [SerializeField] private TextMeshProUGUI _craftingTimeLeftText;
    [SerializeField] private TextMeshProUGUI _craftsPageText;
    [SerializeField] private ItemShower _itemCraftingShower;
    [SerializeField] private ButtonHandler _pauseCraftBtn;
    [SerializeField] private IndexedButtonHandler _prevPageBtn;
    [SerializeField] private IndexedButtonHandler _nextPageBtn;
    private CraftingRecipieData[] _recipies;
    private int _craftsPage = 0;
    private ICrafter _crafter;

    public delegate void CraftEndedDelegate();
    private CraftEndedDelegate OnCraftEnded;

    public void OpenCraftMenu(ICrafter crafter, CraftEndedDelegate craftEndedDelegate)
    {
        _crafter = crafter;
        _recipies = crafter.Recipies;
        _craftsPage = 0;
        _prevPageBtn.RemoveListener(ChangeCraftPage);
        _nextPageBtn.RemoveListener(ChangeCraftPage);
        _prevPageBtn.AddListener(ChangeCraftPage);
        _nextPageBtn.AddListener(ChangeCraftPage);
        _pauseCraftBtn.ResetListeners();
        ChangeCraftPage(0);

        if (_crafter.ActiveCraftData == null)
        {
            _craftInProgress.SetActive(false);
            ShowRecipies();
        }
        else
        {
            _craftInProgress.SetActive(true);
            _itemCraftingShower.ShowItem(crafter.ActiveCraftData.Output[0]);
            _craftingTimeLeftText.text = TimeConverter.InsertTime("{0}:{1}", _crafter.CraftTimeLeft, TimeConverter.InsertionType.HourMinute);
            PauseCraft();
        }
    }

    private void ShowRecipies()
    {
        foreach (CraftingRecipieShower recipieShower in _recipieShowers)
        {
            recipieShower.ShowRecipie(null, null);
        }

        for (int i = 0; i < _recipieShowers.Length; i++)
        {
            if (i + _craftsPage * _recipieShowers.Length >= _recipies.Length)
            {
                break;
            }
            _recipieShowers[i].ShowRecipie(_recipies[i + _craftsPage * _recipieShowers.Length], StartCraft);
        }
    }

    private void ChangeCraftPage(int change)
    {
        int pagesCount = Mathf.CeilToInt(_recipies.Length / (float)_recipieShowers.Length);
        _craftsPage += change;

        if (_craftsPage >= pagesCount)
        {
            _craftsPage = 0;
        }
        else if (_craftsPage < 0)
        {

            _craftsPage = pagesCount - 1;
        }

        _craftsPageText.text = (_craftsPage + 1) + "/" + pagesCount;
        ShowRecipies();
    }

    private void StartCraft(int index)
    {
        _craftInProgress.SetActive(true);
        _itemCraftingShower.ShowItem(_recipies[index + _craftsPage * _recipieShowers.Length].Output[0]);
        _crafter.OnCraftStarted(_recipies[index + _craftsPage * _recipieShowers.Length]);

        foreach (Item inputItem in _recipies[index + _craftsPage * _recipieShowers.Length].ItemRequirements)
        {
            GlobalRepository.Inventory.RemoveItem(inputItem, inputItem.Count);
        }

        ResumeCraft();
    }

    private void EndCraft()
    {
        foreach (Item item in _crafter.ActiveCraftData.Output)
        {
            GlobalRepository.Inventory.AddItem(item, false);
        }

        _craftInProgress.SetActive(false);
        _crafter.OnCraftEnded();
        OnCraftEnded?.Invoke();
        PauseCraft();
        _pauseCraftBtn.RemoveListener(PauseCraft);
        _pauseCraftBtn.RemoveListener(ResumeCraft);
        ShowRecipies();
    }

    public void ResumeCraft()
    {
        GlobalRepository.OnTimeUpdated += CraftInProgress;
        Time.timeScale = 20 * GlobalRepository.Difficulty.DayCycleLength / 24;
        _pauseCraftBtn.RemoveListener(ResumeCraft);
        _pauseCraftBtn.AddListener(PauseCraft);
    }

    public void PauseCraft()
    {
        GlobalRepository.OnTimeUpdated -= CraftInProgress;
        Time.timeScale = 1;
        _pauseCraftBtn.RemoveListener(PauseCraft);
        _pauseCraftBtn.AddListener(ResumeCraft);
    }

    private void CraftInProgress()
    {
        _craftingTimeLeftText.text =  TimeConverter.InsertTime("{0}:{1}", _crafter.CraftTimeLeft,TimeConverter.InsertionType.HourMinute);
        _crafter.DecreaseCraftTimeLeft();

        if (_crafter.CraftTimeLeft <= 0)
        {
            GlobalRepository.OnTimeUpdated -= CraftInProgress;
            EndCraft();
        }
    }
}
