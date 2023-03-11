using UnityEngine;
using TMPro;

public class StorageScreenShower : MonoBehaviour, IClosable
{
    [SerializeField] private Joystick _joystick;
    [SerializeField] private GameObject _upgradesMenu;
    [SerializeField] private GameObject _storageMenu;
    [SerializeField] private ButtonHandler _storageMenuBtn;
    [SerializeField] private ButtonHandler _upgradesBtn;
    [SerializeField] private Upgrader _upgrader;
    [SerializeField] private LootScreenShower _containerMenuShower;

    private Storage _storage;
    private ScreensCloser _screensCloser;

    private void Awake()
    {
        _screensCloser = GameObject.FindObjectOfType<ScreensCloser>();
        _storageMenuBtn.AddListener(OpenStorageMenu);
        _upgradesBtn.AddListener(OpenUpgradesMenu);
        this.gameObject.SetActive(false);
    }

    public void OpenStorageScreen(Storage storage)
    {
        _screensCloser.CloseAllScreens();
        this.gameObject.SetActive(true);
        _joystick.enabled = false;
        _storage = storage;
        _upgrader.ShowUpgradeMenu(_storage, OpenStorageMenu);
        GlobalRepository.CountWeight();
        OpenStorageMenu();
    }

    public void CloseScreen()
    {
        this.gameObject.SetActive(false);
        _upgrader.CloseUpgradeMenu();
        _joystick.enabled = true;   
    }

    private void OpenStorageMenu()
    {
        _storageMenu.SetActive(true);
        _upgradesMenu.SetActive(false);
        _containerMenuShower.OpenLootScreen(_storage.ItemContainer, "Storage", true, null, 0);
    }

    private void OpenUpgradesMenu()
    {
        _storageMenu.SetActive(false);
        _upgradesMenu.SetActive(true);
        _upgrader.ShowUpgradeMenu(_storage, OpenStorageMenu);
    }
}
