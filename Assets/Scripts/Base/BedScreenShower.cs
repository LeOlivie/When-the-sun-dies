using UnityEngine;
using TMPro;

public class BedScreenShower : MonoBehaviour, IClosable
{
    [SerializeField] private Joystick _joystick;
    [SerializeField] private GameObject _upgradesMenu;
    [SerializeField] private GameObject _bedMenu;
    [SerializeField] private GameObject _sleepingScreen;
    [SerializeField] private TextMeshProUGUI _fatigueMultiplierText;
    [SerializeField] private TextMeshProUGUI _sleepingText;
    [SerializeField] private ItemShower[] _inventoryItemShowers;
    [SerializeField] private ButtonHandler _startSleepingBtn;
    [SerializeField] private ButtonHandler _stopSleepingBtn;
    [SerializeField] private ButtonHandler _bedMenuBtn;
    [SerializeField] private ButtonHandler _upgradesBtn;
    [SerializeField] private Upgrader _upgrader;

    private Bed _bed;
    private ScreensCloser _screensCloser;

    private void Awake()
    {
        _screensCloser = GameObject.FindObjectOfType<ScreensCloser>();
        _bedMenuBtn.AddListener(OpenBedMenu);
        _upgradesBtn.AddListener(OpenUpgradesMenu);
        this.gameObject.SetActive(false);
    }

    public void OpenBedScreen(Bed bed)
    {
        _screensCloser.CloseAllScreens();
        this.gameObject.SetActive(true);
        _joystick.enabled = false;
        _bed = bed;
        _upgrader.ShowUpgradeMenu(_bed, OpenBedMenu);
        GlobalRepository.Inventory.ContainerUpdated += ShowInventory;
        GlobalRepository.CountWeight();
        _startSleepingBtn.AddListener(StartSleeping);
        _stopSleepingBtn.AddListener(StopSleeping);
        OpenBedMenu();
        ShowInventory();
    }

    public void CloseScreen()
    {
        _upgrader.CloseUpgradeMenu();
        StopSleeping();
        _joystick.enabled = true;
        GlobalRepository.Inventory.ContainerUpdated -= ShowInventory;
        _startSleepingBtn.RemoveListener(StartSleeping);
        _stopSleepingBtn.RemoveListener(StopSleeping);
        this.gameObject.SetActive(false);
    }

    private void ShowInventory()
    {
        for (int i = 0; i < GlobalRepository.Inventory.Items.Length; i++)
        {
            _inventoryItemShowers[i].ShowItem(GlobalRepository.Inventory.Items[i]);
        }
    }

    private void OpenBedMenu()
    {
        _bedMenu.SetActive(true);
        _upgradesMenu.SetActive(false);
        _fatigueMultiplierText.text = $"Fatigue recovery multiplier: {_bed.FatigueMultiplier * 100}%";
    }

    private void OpenUpgradesMenu()
    {
        _bedMenu.SetActive(false);
        _upgradesMenu.SetActive(true);
        _upgrader.ShowUpgradeMenu(_bed, OpenBedMenu);
    }

    private void StartSleeping()
    {
        _sleepingScreen.SetActive(true);
        Time.timeScale = 70;
        GlobalRepository.OnTimeUpdated += SleepProgress;
    }

    private void SleepProgress()
    {
        GlobalRepository.AddFatigue(100f / GlobalRepository.Difficulty.SleepTimePerDay / 60f * _bed.FatigueMultiplier + 100 / 1440f);
        string fatigueString = GlobalRepository.Fatigue.ToString();

        while (fatigueString.Length < 3)
        {
            fatigueString = "0" + fatigueString;
        }

        _sleepingText.text = $"Fatigue: {fatigueString}/100";
    }

    private void StopSleeping()
    {
        _sleepingScreen.SetActive(false);
        Time.timeScale = 1;
        GlobalRepository.OnTimeUpdated -= SleepProgress;
    }
}
