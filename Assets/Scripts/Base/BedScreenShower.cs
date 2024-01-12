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
    [SerializeField] private TimeCounter _timeCounter;

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
        GlobalRepository.PlayerVars.Inventory.ContainerUpdated += ShowInventory;
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
        GlobalRepository.PlayerVars.Inventory.ContainerUpdated -= ShowInventory;
        _startSleepingBtn.RemoveListener(StartSleeping);
        _stopSleepingBtn.RemoveListener(StopSleeping);
        this.gameObject.SetActive(false);
    }

    private void ShowInventory()
    {
        for (int i = 0; i < GlobalRepository.PlayerVars.Inventory.Items.Length; i++)
        {
            _inventoryItemShowers[i].ShowItem(GlobalRepository.PlayerVars.Inventory.Items[i]);
        }
    }

    private void OpenBedMenu()
    {
        _bedMenu.SetActive(true);
        _upgradesMenu.SetActive(false);
        _fatigueMultiplierText.text = $"Fatigue recovery multiplier: {Mathf.Round(_bed.FatigueMultiplier) * 100}%\n";
        _fatigueMultiplierText.text += $"Kcal deacrease buff: +{Mathf.Round(1 - _bed.KcalDecreaseBuff) * 100}%\n";
        _fatigueMultiplierText.text += $"Water deacrease buff: +{Mathf.Round(1 - _bed.WaterDecreaseBuff) * 100}%";
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
        Time.timeScale = 70 * GlobalRepository.SystemVars.Difficulty.DayCycleLength / 30;
        GlobalRepository.OnTimeUpdated += SleepProgress;
        _timeCounter.KcalDecreaseBuff = _bed.KcalDecreaseBuff;
        _timeCounter.WaterDecreaseBebuff = _bed.WaterDecreaseBuff;
    }

    private void SleepProgress()
    {
        GlobalRepository.PlayerVars.Fatigue += 100f / GlobalRepository.SystemVars.Difficulty.SleepTimePerDay / 60f * _bed.FatigueMultiplier + 100 / 1440f;
        GlobalRepository.PlayerVars.Health += 0.1f;
        string fatigueString = Mathf.Round(GlobalRepository.PlayerVars.Fatigue).ToString();

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
        _timeCounter.KcalDecreaseBuff = 1;
        _timeCounter.WaterDecreaseBebuff = 1;
    }
}
