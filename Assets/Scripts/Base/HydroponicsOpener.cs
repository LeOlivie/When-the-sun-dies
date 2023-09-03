using UnityEngine;
using SaveDatas;

public class HydroponicsOpener : Savable
{
    [SerializeField] private Hydroponics _hydropnics = new Hydroponics();
    [SerializeField] private HiddableButtonHandler _interractButton;
    [SerializeField] private HydroponicsScreenShower _hydroponicsScreenShower;
    [SerializeField] private SpriteRenderer _hydroponicsSpriteRenderer;

    public Hydroponics Hydroponics => _hydropnics;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player")
        {
            return;
        }

        _interractButton.AddListener(OpenHydroponicsScreen);
        _hydropnics.OnUpgradedEvent += ChangeSprite;
    }

    private void OpenHydroponicsScreen()
    {
        if (_hydroponicsScreenShower.gameObject.activeInHierarchy)
        {
            _hydroponicsScreenShower.CloseScreen();
        }
        else
        {
            _hydroponicsScreenShower.OpenHydroponicsScreen(_hydropnics);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag != "Player")
        {
            return;
        }

        _interractButton.RemoveListener(OpenHydroponicsScreen);
        _hydropnics.OnUpgradedEvent -= ChangeSprite;
    }

    private void ChangeSprite()
    {
        _hydroponicsSpriteRenderer.sprite = _hydropnics.UpgradedSprite;
    }

    public override SaveData GetSaveData()
    {
        return new HydroponicsSaveData(_hydropnics);
    }

    public override void LoadSaveData(SaveData saveData)
    {
        _hydropnics.LoadSaveData((HydroponicsSaveData)saveData);
        ChangeSprite();
    }
}
