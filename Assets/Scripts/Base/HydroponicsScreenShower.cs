using UnityEngine;
using TMPro;

public class HydroponicsScreenShower : MonoBehaviour, IClosable
{
    [SerializeField] private Joystick _joystick;
    [SerializeField] private GameObject _upgradesMenu;
    [SerializeField] private GameObject _cropsMenu;
    [SerializeField] private CropGrowthShower _growingCropMenu;
    [SerializeField] private ItemShower[] _inventoryItemShowers;
    [SerializeField] private CropShower[] _cropShowers;
    [SerializeField] private TextMeshProUGUI _pageText;
    [SerializeField] private ButtonHandler _upgradesMenuBtn;
    [SerializeField] private ButtonHandler _cropsMenuBtn;
    [SerializeField] private IndexedButtonHandler _prevPageBtn;
    [SerializeField] private IndexedButtonHandler _nextPageBtn;
    [SerializeField] private Upgrader _upgrader;

    private Hydroponics _hydroponics;
    private ScreensCloser _screensCloser;
    private sbyte _pageIndex = 0;

    private void Awake()
    {
        _screensCloser = GameObject.FindObjectOfType<ScreensCloser>();
        _cropsMenuBtn.AddListener(OpenCropsMenu);
        _upgradesMenuBtn.AddListener(OpenUpgradesMenu);
        _prevPageBtn.AddListener(ChangePage);
        _prevPageBtn.SetIndex(-1);
        _nextPageBtn.AddListener(ChangePage);
        _nextPageBtn.SetIndex(1);
        this.gameObject.SetActive(false);
    }

    public void OpenHydroponicsScreen(Hydroponics hydroponics)
    {
        _screensCloser.CloseAllScreens();
        _growingCropMenu.gameObject.SetActive(false);
        this.gameObject.SetActive(true);
        _joystick.enabled = false;
        _hydroponics = hydroponics;
        _upgrader.ShowUpgradeMenu(_hydroponics, OpenCropsMenu);
        GlobalRepository.PlayerVars.Inventory.ContainerUpdated += ShowInventory;
        GlobalRepository.CountWeight();
        OpenCropsMenu();
        ShowInventory();

        if (_hydroponics.GrowingCropData != null)
        {
            _growingCropMenu.ShowCropGrowth(_hydroponics);
            _growingCropMenu.gameObject.SetActive(true);
        }
    }

    public void CloseScreen()
    {
        _upgrader.CloseUpgradeMenu();
        _joystick.enabled = true;
        GlobalRepository.PlayerVars.Inventory.ContainerUpdated -= ShowInventory;
        this.gameObject.SetActive(false);
    }

    private void ShowInventory()
    {
        for (int i = 0; i < GlobalRepository.PlayerVars.Inventory.Items.Length; i++)
        {
            _inventoryItemShowers[i].ShowItem(GlobalRepository.PlayerVars.Inventory.Items[i]);
        }
    }

    private void OpenCropsMenu()
    {
        _cropsMenu.SetActive(true);
        _upgradesMenu.SetActive(false);
        ChangePage(0);
    }

    private void StartGrowing(int index)
    {
        foreach (Item item in _hydroponics.CropsDatas[index].Requirements)
        {
            GlobalRepository.PlayerVars.Inventory.RemoveItem(item, item.Count);
        }

        _hydroponics.StartGrowingCrop(_hydroponics.CropsDatas[index]);
        _growingCropMenu.ShowCropGrowth(_hydroponics);
    }

    private void ChangePage(int change)
    {
        _pageIndex += (sbyte)change;

        if (_pageIndex >= Mathf.RoundToInt(_hydroponics.CropsDatas.Length / 2f))
        {
            _pageIndex = 0;
        }
        else if (_pageIndex < 0)
        {
            _pageIndex = (sbyte)(Mathf.RoundToInt(_hydroponics.CropsDatas.Length / 2f) - 1);
        }

        _pageText.text = $"{_pageIndex+1}/{Mathf.RoundToInt(_hydroponics.CropsDatas.Length/2f)}";

        for (int i = 0; i < _cropShowers.Length; i++)
        {
            if (i + _pageIndex * _cropShowers.Length >= _hydroponics.CropsDatas.Length)
            {
                _cropShowers[i].ShowCrop(null, i + _pageIndex * _cropShowers.Length, StartGrowing);
                continue;
            }

            _cropShowers[i].ShowCrop(_hydroponics.CropsDatas[i + _pageIndex * _cropShowers.Length], i + _pageIndex * _cropShowers.Length, StartGrowing);
        }
    }

    private void OpenUpgradesMenu()
    {
        _cropsMenu.SetActive(false);
        _upgradesMenu.SetActive(true);
        _upgrader.ShowUpgradeMenu(_hydroponics, OpenCropsMenu);
    }
}