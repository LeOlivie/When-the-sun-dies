using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradingScreenOpener : MonoBehaviour
{
    [SerializeField] private HiddableButtonHandler _interractButton;
    [SerializeField] private Item[] _traderLoot;
    [SerializeField] private TraderMenuManager _traderMenuManager;
    private ItemContainer _traderItemContainer = new ItemContainer(20);

    private void Start()
    {
        foreach (Item item in _traderLoot)
        {
            _traderItemContainer.AddItem(item,false);
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
