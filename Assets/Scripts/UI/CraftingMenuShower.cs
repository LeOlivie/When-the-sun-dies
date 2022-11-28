using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftingMenuShower : MonoBehaviour
{
    [SerializeField] private Joystick _joystick;
    [SerializeField] private GameObject _craftingScr;
    [SerializeField] private GameObject _craftMenu;
    [SerializeField] private GameObject _upgradesMenu;
    [SerializeField] private ItemShower[] _inventoryItemShowers;
    [SerializeField] private ButtonHandler _closeBtn;
    [SerializeField] private ButtonHandler _craftsBtn;
    [SerializeField] private ButtonHandler _upgradesBtn;
    [SerializeField] private Upgrader _upgrader;
    [SerializeField] private Crafter _crafter;

    private CraftingStation _craftingStation;


    public void OpenCraftScreen(CraftingStation craftingStation)
    {
        _joystick.enabled = false;
        _craftingStation = craftingStation;
        _upgrader.ShowUpgradeMenu(_craftingStation, OpenCraftsMenu);
        _craftingScr.SetActive(true);
        _closeBtn.AddListener(CloseCraftScreen);
        GlobalRepository.Inventory.ContainerUpdated += ShowInventory;
        GlobalRepository.CountWeight();
        OpenCraftsMenu();
        ShowInventory();

        _craftsBtn.AddListener(OpenCraftsMenu);
        _upgradesBtn.AddListener(OpenUpgradesMenu);
    }

    public void CloseCraftScreen()
    {
        _joystick.enabled = true;
        _craftingScr.SetActive(false);
        GlobalRepository.Inventory.ContainerUpdated -= ShowInventory;
        _closeBtn.RemoveListener(CloseCraftScreen);
        _crafter.PauseCraft();
        _upgrader.PauseUpgrade();
        _upgrader.CloseUpgradeMenu();
    }

    private void ShowInventory()
    {
        for (int i = 0; i < GlobalRepository.Inventory.Items.Length; i++)
        {
            _inventoryItemShowers[i].ShowItem(GlobalRepository.Inventory.Items[i]);
        }
    }

    private void OpenCraftsMenu()
    {
        _craftMenu.SetActive(true);
        _upgradesMenu.SetActive(false);
        _crafter.OpenCraftMenu(_craftingStation, OpenCraftsMenu);
    }

    private void OpenUpgradesMenu()
    {
        _craftMenu.SetActive(false);
        _upgradesMenu.SetActive(true);
        _upgrader.ShowUpgradeMenu(_craftingStation, OpenCraftsMenu);
    }
}
