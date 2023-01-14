using UnityEngine;

public class MenuShower : MonoBehaviour, IClosable
{
    [SerializeField] private Joystick _joystick;
    [SerializeField] private ButtonHandler _openBtn;
    [SerializeField] private GameObject _menu;
    [SerializeField] private GameObject _inventory;
    [SerializeField] private GameObject _healthScreen;
    [SerializeField] private ButtonHandler _inventoryBtn;
    [SerializeField] private ButtonHandler _healthBtn;
    private ScreensCloser _screensCloser;
    
    private void Awake()
    {
        _screensCloser = GameObject.FindObjectOfType<ScreensCloser>();
        _openBtn.AddListener(OpenMenu);
        _openBtn.RemoveListener(CloseScreen);
        _inventoryBtn.AddListener(OpenInventory);
        _healthBtn.AddListener(OpenHealthScreen);
    }

    private void OpenMenu()
    {
        _screensCloser.CloseAllScreens();
        _openBtn.RemoveListener(OpenMenu);
        _openBtn.AddListener(CloseScreen);
        _joystick.enabled = false;
        _menu.SetActive(true);
        OpenInventory();
    }

    public void CloseScreen()
    {
        _openBtn.AddListener(OpenMenu);
        _openBtn.RemoveListener(CloseScreen);
        _joystick.enabled = true;
        _menu.SetActive(false);
    }

    private void OpenInventory()
    {
        _inventory.SetActive(true);
        _healthScreen.SetActive(false);
    }

    private void OpenHealthScreen()
    {
        _inventory.SetActive(false);
        _healthScreen.SetActive(true);
    }
}
