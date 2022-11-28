using UnityEngine;

public class CraftStationOpener : MonoBehaviour
{
    [SerializeField] private ButtonHandler _interractBtn;
    [SerializeField] private CraftingStation _station;
    [SerializeField] private CraftingMenuShower _menuShower;
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player")
        {
            return;
        }
        _interractBtn.AddListener(OnBtnClicked);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _interractBtn.RemoveListener(OnBtnClicked);
    }

    private void OnBtnClicked()
    {
        if(_menuShower.gameObject.activeInHierarchy)
        {
            _menuShower.CloseCraftScreen();
        }
        else
        {
            _menuShower.OpenCraftScreen(_station);
        }
    }
}