using UnityEngine;

public class TradingScreenOpener : MonoBehaviour
{ 
    [System.Serializable]
    private struct TradeItemsGroup
    {
        [SerializeField] private Item[] _items;
        [SerializeField,Tooltip("Inclusive")] private int _minAmount;
        [SerializeField, Tooltip("Inclusive")] private int _maxAmount;
        
        public Item[] Items => _items;
        public int MinAmount=> _minAmount;
        public int MaxAmount => _maxAmount;
    }

    [SerializeField] private HiddableButtonHandler _interractButton;
    [SerializeField] private TraderMenuManager _traderMenuManager;
    [SerializeField] private TradeItemsGroup[] _tradeItemsGroups;
    private ItemContainer _traderItemContainer = new ItemContainer(20);

    private void Start()
    {
        /* foreach (Item item in _traderLoot)
         {
             _traderItemContainer.AddItem(item,false);
         }*/
        int spawnedItems = 0;
        foreach (TradeItemsGroup tradeItemsGroup in _tradeItemsGroups)
        {
            for (int i = 0; i < Random.Range(tradeItemsGroup.MinAmount, tradeItemsGroup.MaxAmount + 1); i++)
            {
                spawnedItems++;
                _traderItemContainer.AddItem(tradeItemsGroup.Items[Random.Range(0, tradeItemsGroup.Items.Length)], false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player")
        {
            return;
        }

        _interractButton.AddListener(OpenTradingScreen);
    }

    private void OpenTradingScreen()
    {
        _traderMenuManager.OpenTraderMenu(_traderItemContainer);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag != "Player")
        {
            return;
        }

        _interractButton.RemoveListener(OpenTradingScreen);
    }
}
