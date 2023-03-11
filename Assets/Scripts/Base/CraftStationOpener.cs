using UnityEngine;

public class CraftStationOpener : MonoBehaviour
{
    [SerializeField] private ButtonHandler _interractBtn;
    [SerializeField] private CraftingStation _station;
    [SerializeField] private SpriteRenderer _stationSpriteRenderer;
    [SerializeField]  private CraftingMenuShower _menuShower;

    public CraftingStation Station => _station;

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

    private void ChangeSprite()
    {
        _stationSpriteRenderer.sprite = _station.StationData.GetSprite(_station.CurrLevel);
    }

    private void OnBtnClicked()
    {
        if(_menuShower.gameObject.activeInHierarchy)
        {
            _menuShower.CloseScreen();
            _station.OnUpgradedEvent -= ChangeSprite;
        }
        else
        {
            _menuShower.OpenCraftScreen(_station);
            _station.OnUpgradedEvent += ChangeSprite;
        }
    }

    public void LoadSaveData(SaveDatas.CraftingStationSaveData saveData)
    {
        _station.LoadSaveData(saveData);
        ChangeSprite();
    }
}