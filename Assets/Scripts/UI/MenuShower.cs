using UnityEngine;

public class MenuShower : MonoBehaviour
{
    [SerializeField] private Joystick _joystick;
    [SerializeField] private ButtonHandler _openBtn;
    [SerializeField] private GameObject _menu;
    [SerializeField] private GameObject _inventory;
    [SerializeField] private GameObject _healthScreen;
    [SerializeField] private ButtonHandler _inventoryBtn;
    [SerializeField] private ButtonHandler _healthBtn;

    private void Awake()
    {
        _openBtn.AddListener(OpenMenu);
        _inventoryBtn.AddListener(OpenInventory);
        _healthBtn.AddListener(OpenHealthScreen);
    }

    private void OpenMenu()
    {
        _joystick.enabled = _menu.activeInHierarchy;
        _menu.SetActive(!_menu.activeInHierarchy);
        OpenInventory();
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
