using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SellItemShower : ItemShower
{
    [SerializeField] private TextMeshProUGUI _sellOrBuyText;

    public void ShowSellableItem(Item item, string SellOrBuy)
    {
        ShowItem(item);
        _sellOrBuyText.text = SellOrBuy;

        if (SellOrBuy == "Buy")
        {
            _sellOrBuyText.color = new Color(0.7f,0.85f,0.7f,1);
        }
        else
        {
            _sellOrBuyText.color = new Color(0.77f, 0.55f, 0.55f, 1);
        }
    }
}
