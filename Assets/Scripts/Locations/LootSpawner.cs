using UnityEngine;

public class LootSpawner : MonoBehaviour
{
    [SerializeField] private LootSpawnerData _lootSpawnerData;
    [SerializeField] private GameObject _lootScreen;
    [SerializeField] private ButtonHandler _interractBtn;
    [SerializeField] private LootScreenShower _lootScreenShower;
    [SerializeField] private bool _isLooted;
    private ItemContainer _itemContainer;

    private void Start()
    {
        if(_lootSpawnerData != null)
        {
            _itemContainer = new ItemContainer(_lootSpawnerData.ContainerSize);
            Item[] items = _lootSpawnerData.GetSpawnedItems();

            foreach (Item item in items)
            {
                _itemContainer.AddItem(item, false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _interractBtn.AddListener(Interract);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _interractBtn.RemoveListener(Interract);
    }

    private void Interract()
    {
        if (!_lootScreen.activeInHierarchy)
        {
            _lootScreenShower.OpenLootScreen(_itemContainer, _lootSpawnerData.ContainerName, _isLooted, OnLooted, _lootSpawnerData.LootTime);
        }
        else
        {
            _lootScreenShower.CloseLootScreen();
        }
    }

    private void OnLooted()
    {
        _isLooted = true;
    }
}
