using UnityEngine;
using TMPro;

public class WaterCollectorScreenShower : MonoBehaviour, IClosable
{
    [SerializeField] private Joystick _joystick;
    [SerializeField] private GameObject _upgradesMenu;
    [SerializeField] private GameObject _waterCollectorMenu;
    [SerializeField] private TextMeshProUGUI _infoText;
    [SerializeField] private ItemShower[] _inventoryItemShowers;
    [SerializeField] private ButtonHandler _collectWaterBtn;
    [SerializeField] private ButtonHandler _waterCollectorMenuBtn;
    [SerializeField] private ButtonHandler _upgradesBtn;
    [SerializeField] private Upgrader _upgrader;
    [SerializeField] private Item _emptyBottleItem;
    [SerializeField] private Item _waterBottleItem;

    private WaterCollector _waterCollector;
    private ScreensCloser _screensCloser;

    private void Awake()
    {
        _screensCloser = GameObject.FindObjectOfType<ScreensCloser>();
        _waterCollectorMenuBtn.AddListener(OpenWaterCollectorMenu);
        _upgradesBtn.AddListener(OpenUpgradesMenu);
        this.gameObject.SetActive(false);
    }

    public void OpenWaterCollectorScreen(WaterCollector waterCollector)
    {
        _screensCloser.CloseAllScreens();
        this.gameObject.SetActive(true);
        _joystick.enabled = false;
        _waterCollector = waterCollector;
        _upgrader.ShowUpgradeMenu(_waterCollector, OpenWaterCollectorMenu);
        GlobalRepository.Inventory.ContainerUpdated += ShowInventory;
        GlobalRepository.CountWeight();
        OpenWaterCollectorMenu();
        ShowInventory();
        GlobalRepository.OnTimeUpdated += ShowInfo;
    }

    public void CloseScreen()
    {
        _upgrader.CloseUpgradeMenu();
        _joystick.enabled = true;
        GlobalRepository.Inventory.ContainerUpdated -= ShowInventory;
        _collectWaterBtn.RemoveListener(CollectWaterInBottle);
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

    private void OpenWaterCollectorMenu()
    {
        _waterCollectorMenu.SetActive(true);
        _upgradesMenu.SetActive(false);
    }

    private void ShowInfo()
    {
        _waterCollector.CheckWaterCollection(); 
        int timeToCollect = (int)(_waterCollector.WaterCollectionStart + _waterCollector.TimeToCollectWater - GlobalRepository.GlobalTime);
        _infoText.text = TimeConverter.InsertTime("Time to collect water: {0}:{1}\n\n",timeToCollect,TimeConverter.InsertionType.HourMinute);
        _infoText.text += $"Water collected: {_waterCollector.WaterCollected}/{_waterCollector.MaxWaterAmount} bottles.";
        _collectWaterBtn.RemoveListener(CollectWaterInBottle);

        if (GlobalRepository.Inventory.CheckIfHas(_emptyBottleItem.ItemData, 1) && GlobalRepository.Inventory.CheckIfCanFit(_waterBottleItem) && _waterCollector.WaterCollected > 0)
        {
            _collectWaterBtn.AddListener(CollectWaterInBottle);
        }
    }

    private void CollectWaterInBottle()
    {
        if (GlobalRepository.Inventory.CheckIfHas(_emptyBottleItem.ItemData, 1) && GlobalRepository.Inventory.CheckIfCanFit(_waterBottleItem) && _waterCollector.WaterCollected > 0)
        {
            GlobalRepository.Inventory.RemoveItem(_emptyBottleItem, 1);
            GlobalRepository.Inventory.AddItem(_waterBottleItem, false);
            _waterCollector.AddWater(-1);
        }
    }

    private void OpenUpgradesMenu()
    {
        _waterCollectorMenu.SetActive(false);
        _upgradesMenu.SetActive(true);
        _upgrader.ShowUpgradeMenu(_waterCollector, OpenWaterCollectorMenu);
    }
}
