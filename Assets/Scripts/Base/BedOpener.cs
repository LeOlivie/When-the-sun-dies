using UnityEngine;
using SaveDatas;

public class BedOpener : Savable
{
    [SerializeField] private Bed _bed = new Bed();
    [SerializeField] private HiddableButtonHandler _interractButton;
    [SerializeField] private BedScreenShower _bedScreenShower;
    [SerializeField] private SpriteRenderer _bedSpriteRenderer;

    public Bed Bed => _bed;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player")
        {
            return;
        }

        _interractButton.AddListener(OpenBedScreen);
        _bed.OnUpgradedEvent += ChangeSprite;
    }

    private void OpenBedScreen()
    {
        if (_bedScreenShower.gameObject.activeInHierarchy)
        {
            _bedScreenShower.CloseScreen();
        }
        else
        {
            _bedScreenShower.OpenBedScreen(_bed);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag != "Player")
        {
            return;
        }

        _interractButton.RemoveListener(OpenBedScreen);
        _bed.OnUpgradedEvent -= ChangeSprite;
    }

    private void ChangeSprite()
    {
        _bedSpriteRenderer.sprite = _bed.UpgradedSprite;
    }

    public override SaveData GetSaveData()
    {
        return new UpgradableSaveData(Bed);
    }

    public override void LoadSaveData(SaveData saveData)
    {
        _bed.LoadSaveData((UpgradableSaveData)saveData);
        ChangeSprite();
    }
}
