using UnityEngine;
using SaveDatas;

public class StorageOpener : MonoBehaviour
{
    [SerializeField] private Storage _storage = new Storage();
    [SerializeField] private HiddableButtonHandler _interractButton;
    [SerializeField] private StorageScreenShower _storageScreenShower;
    [SerializeField] private SpriteRenderer _storageSpriteRenderer;

    public Storage Storage => _storage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player")
        {
            return;
        }

        _interractButton.AddListener(OpenStorageScreen);
        _storage.OnUpgradedEvent += ChangeSprite;

        foreach (Item item in _storage.UpgradeRequirements)
        {
            GlobalRepository.Inventory.AddItem(item, false);
        }
    }

    private void OpenStorageScreen()
    {
        if (_storageScreenShower.gameObject.activeInHierarchy)
        {
            _storageScreenShower.CloseScreen();
        }
        else
        {
            _storageScreenShower.OpenStorageScreen(_storage);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag != "Player")
        {
            return;
        }

        _interractButton.RemoveListener(OpenStorageScreen);
        _storage.OnUpgradedEvent -= ChangeSprite;
    }

    private void ChangeSprite()
    {
        _storageSpriteRenderer.sprite = _storage.UpgradedSprite;
    }

    public void LoadSaveData(SaveDatas.StorageSaveData saveData)
    {
        _storage.LoadStorage(saveData);
        ChangeSprite();
    }
}
