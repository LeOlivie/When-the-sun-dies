using UnityEngine;
using TMPro;

public class TVScreenShower : MonoBehaviour, IClosable
{
    [SerializeField] private Joystick _joystick;
    [SerializeField] private GameObject _upgradesMenu;
    [SerializeField] private GameObject _tvMenu;
    [SerializeField] private TextMeshProUGUI _infoText;
    [SerializeField] private ItemShower[] _inventoryItemShowers;
    [SerializeField] private ButtonHandler _tvMenuBtn;
    [SerializeField] private ButtonHandler _upgradesBtn;
    [SerializeField] private Upgrader _upgrader;

    private TV _tv;
    private ScreensCloser _screensCloser;

    private void Awake()
    {
        _screensCloser = GameObject.FindObjectOfType<ScreensCloser>();
        _tvMenuBtn.AddListener(OpenTVMenu);
        _upgradesBtn.AddListener(OpenUpgradesMenu);
        this.gameObject.SetActive(false);
    }

    public void OpenTVScreen(TV tv)
    {
        _screensCloser.CloseAllScreens();
        this.gameObject.SetActive(true);
        _joystick.enabled = false;
        _tv = tv;
        _upgrader.ShowUpgradeMenu(_tv, OpenTVMenu);
        GlobalRepository.Inventory.ContainerUpdated += ShowInventory;
        GlobalRepository.CountWeight();
        OpenTVMenu();
        ShowInventory();
        GlobalRepository.OnTimeUpdated += ShowInfo;
    }

    public void CloseScreen()
    {
        _upgrader.CloseUpgradeMenu();
        _joystick.enabled = true;
        GlobalRepository.Inventory.ContainerUpdated -= ShowInventory;
        GlobalRepository.OnTimeUpdated -= ShowInfo;
        this.gameObject.SetActive(false);
    }

    private void ShowInventory()
    {
        for (int i = 0; i < GlobalRepository.Inventory.Items.Length; i++)
        {
            _inventoryItemShowers[i].ShowItem(GlobalRepository.Inventory.Items[i]);
        }
    }

    private void OpenTVMenu()
    {
        _tvMenu.SetActive(true);
        _upgradesMenu.SetActive(false);
    }

    private void ShowInfo()
    {
        if (_tv.CurrLevel > 0)
        {
            string watchTime = ConvertTime(_tv.WatchTime);
            _infoText.text = $"Happiness boost: {_tv.HappinessBoost} point(s) every 60 minutes.\n\nCurrent watch time: {watchTime} minutes.";
        }
        else
        {
            _infoText.text = "Making a TV provides happiness boost every 60 minutes.";
        }
    }

    private string ConvertTime(uint time)
    {
        string timeConverted = "";

        if (time < 10)
        {
            timeConverted += "0";
        }

        timeConverted += (time).ToString();

        return timeConverted;
    }

    private void OpenUpgradesMenu()
    {
        _tvMenu.SetActive(false);
        _upgradesMenu.SetActive(true);
        _upgrader.ShowUpgradeMenu(_tv, OpenTVMenu);
    }
}
