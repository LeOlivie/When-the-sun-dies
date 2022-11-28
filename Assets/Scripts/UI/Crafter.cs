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
        _prevPageBtn.AddListener(ChangeCraftPage);
        _nextPageBtn.AddListener(ChangeCraftPage);
        ChangeCraftPage(0);

        if (_crafter.ActiveCraftData == null)
        {
            _craftInProgress.SetActive(false);
            ShowRecipies();
        }
        else
        {
            _craftInProgress.SetActive(true);
            _craftingTimeLeftText.text = _crafter.CraftTimeLeft + " minutes";
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
        _pauseCraftBtn.AddListener(ResumeCraft);
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
        _pauseCraftBtn.RemoveListener(PauseCraft);
        _crafter.OnCraftEnded();
        OnCraftEnded?.Invoke();
        PauseCraft();
    }

    public void ResumeCraft()
    {
        GlobalRepository.OnTimeUpdated += CraftInProgress;
        Time.timeScale = 20;
    }

    public void PauseCraft()
    {
        GlobalRepository.OnTimeUpdated -= CraftInProgress;
        Time.timeScale = 1;
    }

    private void CraftInProgress()
    {
        _craftingTimeLeftText.text = _crafter.CraftTimeLeft + " minutes";
        _crafter.DecreaseCraftTimeLeft();

        if (_crafter.CraftTimeLeft <= 0)
        {
            EndCraft();
        }
    }
}
